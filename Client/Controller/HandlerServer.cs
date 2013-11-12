using System;
using System.Collections.Generic;
using System.Diagnostics;
using Common;

namespace Client.Controller
{
	public class HandlerServer : Handler
	{
		private List<Message> list;

		public HandlerServer (View.ViewManager view, 
		               			Model.ModelManager modelManager, List<Common.Message> list) :
		base(view, modelManager)
		{
			this.list = list;
		}
		/// <summary>
		/// Sets the list. We get a new one every second, so when the method ExecuteCommand is called we have
		/// to set the list with all the messages from the server again.
		/// </summary>
		public void SetList (List<Message> list)
		{
			this.list = list;
		}

		override internal void HandleEvent (View.UserEvent userEvent)
		{
			try {
				if (modelManager.GetServerManager ().IsConnected ()) 
				{

					foreach (Message m in list) {
						switch (m.GetMsgType ()) {
						case Message.Type.SKK:
							{
								ServerKickMsg skk = (ServerKickMsg)m;
								modelManager.GetServerManager ().Disconnect ();
								View.PageLauncher launcherView = viewManager.LoadLauncherPage ();
								launcherView.SetStatusMessage ("You were kicked from server. " +
									"  Reason: " + skk.GetText ()
								);
								break;
							}

						case Message.Type.SCC://server "create character" response
							{
								ServerCreateCharacterResponseMsg scc = (ServerCreateCharacterResponseMsg)m;
								viewManager.SetStatusMessage (scc.GetText ());
								break;
							}
						case Message.Type.SSC://server sends your character to you
							{
								ServerSendCharacterMsg ssc = (ServerSendCharacterMsg)m;
								modelManager.GetCharacterManager ().SetMyCharacter (ssc.GetCharacter ());
							/*	View.PageAccount accountView = viewManager.LoadAccountPage (modelManager.GetAccountManager ().GetMyAccount ());
								accountView.SetCharacter (modelManager.GetCharacterManager ().GetMyCharacter ());
								accountView.ShowDefault ();*/
								viewManager.TriggerEvent(View.UserEvent.UPDATE_CURRENT_PAGE);//begär att updatera
								break;
							}
						case Message.Type.SLR://login response from server
							View.PageLauncher launcher = viewManager.LoadLauncherPage ();
							try {
								ServerLoginResponseMsg slr = (ServerLoginResponseMsg)m;

								if (slr.IsAccepted ()) {
									
									launcher.PassLogin ();
									launcher.SetStatusMessage ("Login successful.");

								} else if (!slr.IsAccepted ()) {
									launcher.FailLogin ();
									launcher.SetStatusMessage ("Login failed: " + slr.GetText ());
									ClientDisconnectMsg dm = new ClientDisconnectMsg ("Disconnect-request from client.");
									try {
										modelManager.GetServerManager ().SendAndDisconnect (dm);
									} catch (NWOException ne) {
										handleLostConnection (ne);
										return;
									}

								}
							} catch (NWOException e) {
								launcher.SetStatusMessage ("Login failed: " + e.Message);
								ClientDisconnectMsg dm = new ClientDisconnectMsg ("Disconnect-request from client.");
								try {
									modelManager.GetServerManager ().SendAndDisconnect(dm);
								} catch (NWOException ne) {
									handleLostConnection (ne);
									return;
								}
							}
							break;

						case Message.Type.SSA://recieve account from server

							ServerSendAccountMsg ssa = (ServerSendAccountMsg)m;
							modelManager.GetAccountManager ().SetMyAccount (ssa.GetAccount ());//now this is the clients acc)
							viewManager.TriggerEvent (View.UserEvent.ACCOUNT_SHOW_DEFAULT);
							break;

						case Message.Type.SCA://recieve create account response
						{
							launcher = viewManager.LoadLauncherPage ();
							ServerCreateAccountResponseMsg sca = (ServerCreateAccountResponseMsg)m;

							if (sca.IsAccepted ()) {
								launcher.PassRegistration ();
								launcher.ShowLogin ();
								launcher.SetStatusMessage ("Account created!");
							} else if (!sca.IsAccepted ()) {
								launcher.FailRegistrationEmail ();
								launcher.SetStatusMessage ("Registration failed: " + sca.GetText ());
							}
							ClientDisconnectMsg dm = new ClientDisconnectMsg ("Disconnect-request from client.");
							try {
								modelManager.GetServerManager ().SendAndDisconnect(dm);
							} catch (NWOException ne) {
								handleLostConnection (ne);
								return;
							}
						}	
						break;

						case Message.Type.STA: // train attribute response
						{
							ServerTrainAttributeResponseMsg sta = (ServerTrainAttributeResponseMsg)m;
                            View.Page currentPage = viewManager.GetActivePage();

                            if (sta.GetAccepted())
                            {
                                if (currentPage is View.PageCharacter)
                                {
                                    View.PageCharacter charPage = (View.PageCharacter)currentPage;
                                    charPage.SetAttributeStatusMessage(sta.GetAttributeType(), "Training...");
                                }

                                currentPage.SetStatusMessage("Training " + sta.GetAttributeType() + "...");

                            }

                            else if (!sta.GetAccepted())
                                viewManager.GetActivePage().SetStatusMessage("Can't train " + sta.GetAttributeType() + ": " + sta.GetText());
						}


							break;
						case Message.Type.STR: // train attribute result
						{
							ServerTrainAttributeResultMsg str = (ServerTrainAttributeResultMsg) m;
                            View.Page currentPage = viewManager.GetActivePage();

							View.PageCharacter charPage = viewManager.GetCharacterPage();
                            charPage.ClearAllAttributeStatusMessages();

							if(str.GetResult() < 1)
							    currentPage.SetStatusMessage
									("You are done training " + str.GetAttributeType() +
									 ", but your stats were not improved.");

							else
								currentPage.SetStatusMessage
									("You are done training " + str.GetAttributeType() +
								     ". Your " + str.GetAttributeType() + " increased by " + str.GetResult());
                            
						}
							break;
						case Message.Type.SUS: // use skill response
						{	
							ServerUseSkillResponseMsg sus = (ServerUseSkillResponseMsg) m;
							if(sus.GetAccepted())
								viewManager.GetActivePage().SetStatusMessage("You are " + sus.GetSkillType() + "...");
							else 
								viewManager.GetActivePage().SetStatusMessage("You can't " + sus.GetSkillType() + ".");
						}
							break;
						case Message.Type.SUR: // use skill result, only for notification, server sends the character
							ServerUseSkillResultMsg sur = (ServerUseSkillResultMsg) m;
							viewManager.GetActivePage().SetStatusMessage(
								"Result from " + sur.GetSkillType() + ": " + sur.GetResultDescription());
							break;
						case Message.Type.TXT:
							//TextMsg tm = (TextMsg)m;
							 
							break;

						case Message.Type.SAI:
						{
							ServerAreaMsg sam = (ServerAreaMsg)m;
							modelManager.GetWorldManager().SetFocusedArea(sam.GetArea());
							viewManager.TriggerEvent(View.UserEvent.MAP_CONTEXT);

							break;
						}
						
						case Message.Type.SSD:
						{
							ServerShutdownMsg ss = (ServerShutdownMsg)m;
							modelManager.GetServerManager ().Disconnect ();
							View.PageLauncher launcherView = viewManager.LoadLauncherPage ();
							if (ss.GetText ().Length > 0)
								launcherView.SetStatusMessage ("The server was shut down.  Reason: " + ss.GetText ());
							else
								launcherView.SetStatusMessage ("The server was shut down.");
							break;
						}

						case Message.Type.STM:
						{
							ServerTerrainMatrixMsg stm = (ServerTerrainMatrixMsg)m;
							modelManager.GetWorldManager ().SetTerrainMatrix(stm.GetTerrainMatrix());
							viewManager.TriggerEvent(View.UserEvent.UPDATE_CURRENT_PAGE);
							break;
						}

						case Message.Type.SAR:
						{
							ServerCharacterCurrentAreaMsg sar = (ServerCharacterCurrentAreaMsg)m;
							modelManager.GetWorldManager().SetCurrentArea (sar.GetCurrentArea());

							if(viewManager.GetActivePage() is View.PageMap)
								viewManager.TriggerEvent(View.UserEvent.MAP_MOVE_CHARACTER);
							break;
						}

						case Message.Type.SMC: // Is character able to move?
						{
							ServerMoveCharacterResponseMsg smc = (ServerMoveCharacterResponseMsg)m;

							if (smc.IsAccepted()) {
								modelManager.GetWorldManager().SetCurrentArea (
									modelManager.GetWorldManager().GetFocusedArea()
								);
								viewManager.TriggerEvent(View.UserEvent.MAP_MOVE_CHARACTER);
							} else {
								viewManager.GetActivePage().SetStatusMessage(smc.GetText());
							}
							break;
						}

                        case Message.Type.CHT:
                        {
                            ChatMsg cht = (ChatMsg)m;
							View.PageMain mainPage = viewManager.GetMainPage();

							if(cht.GetName () == "Server")
								mainPage.AddChatInfoMessage(cht.GetText());
							else
                            	mainPage.AddChatMessage(cht.GetName(), cht.GetText(), cht.GetChatType());



                            break;
                        }

						default:
							Debug.WriteLine ("Server has sent a message that is unknown to HandlerServer.");
							break;
						}
					}
				}
			} catch (NWOException e) {
				viewManager.SetStatusMessage (e.Message);
			} catch (Exception e) {
				viewManager.SetStatusMessage ("An unexpected condition occurred.");
				Console.WriteLine (e);
			}
		}
	}
}


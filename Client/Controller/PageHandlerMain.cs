using System;
using Client.View;
using Common;

namespace Client.Controller
{
	public class PageHandlerMain : PageHandler
	{

		public PageHandlerMain (View.ViewManager view, Model.ModelManager modelManager) :
		base(view, modelManager)
		{

		}

		override internal void SetView ()
		{
			page = viewManager.LoadMainPage (modelManager.GetCharacterManager().GetMyCharacter());
			PageMain mainPage = (PageMain)page;
			mainPage.LoadMap(modelManager.GetWorldManager ().GetTerrainMatrix ());
		}

		override internal void HandleEvent (View.UserEvent userEvent)
		{
			PageMain mainView = (PageMain) page;
			switch (userEvent) {
				case View.UserEvent.MAIN_SHOW: mainView.ShowDefault(); break;
				case View.UserEvent.MAIN_QUIT: quit(); break;
				case View.UserEvent.MAIN_HUNT: useSkill(SkillType.HUNTING); break;
				case View.UserEvent.MAIN_SEND_CHAT: sendChatMessage(); break;
			}
		}

		/// <summary>
		/// Sends use skill request to the server.
		/// </summary>
		/// <param name='type'>
		/// skilltype.
		/// </param>
		private void useSkill (Common.SkillType type)
		{
			ClientUseSkillRequestMsg cus = new ClientUseSkillRequestMsg (type);
			try {
				modelManager.GetServerManager ().Send (cus);
			} catch (NWOException nwo_e) {
				handleLostConnection(nwo_e);
			}
		}

		/// <summary>
		/// Sends the chat message.
		/// </summary>
		private void sendChatMessage ()
		{
			PageMain mainPage = (PageMain)page;
			ChatMsg cht;
			string text = mainPage.GetEntryChatMessage ();

			// Check if empty message
			if (text == "" || text == null)
				return;

			// If command
			if (text [0] == '/') {
				// Try typ split message
				try {
					string chatType = text.Substring (0, text.IndexOf (" "));
					chatType.ToLower ();

					switch (chatType)
					{
						case "/pm":
						{
							text = text.Substring (text.IndexOf (" ") + 1);
							string reciever = text.Substring (0, text.IndexOf (" "));

							text = text.Substring (text.IndexOf (" ") + 1);

							cht = new ChatMsg (reciever, text, ChatMsg.ChatType.PRIVATE);
							break;
						}

						default:
							mainPage.AddChatInfoMessage ("Unknown command.");
							return;
					}
				} catch (Exception) {
					mainPage.AddChatInfoMessage ("Unknown command.");
					return;
				}
			}

			else
			{
				cht = new ChatMsg("", text, ChatMsg.ChatType.PUBLIC);
			}


			try {
				modelManager.GetServerManager ().Send (cht);
			} catch (NWOException nwo_e) {
				handleLostConnection(nwo_e);
			}
		}

		override public void UpdatePage()
		{

		}
		/// <summary>
		/// Quit game, return to default account page.
		/// </summary>
		private void quit ()
		{
			viewManager.TriggerEvent(UserEvent.ACCOUNT_SHOW_DEFAULT);
		}
	}
}


using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading;
using Server.Dba;
using Server.Model;
using Server.View;
using Common;
using System.Diagnostics;

namespace Server.Controller
{
	class ServerController
	{
		// Main thread starts here
		public static void Main (string[] args)
		{
			new ServerController ();
		}

		// Attributes
		private DbaMongo database;
		private ConnectionHandler ch;
		private Terminal terminal;
		private GameWorld gameWorld;
		private bool running = true;
        private Stopwatch time;

		// Variables used for sleeping.
        private static readonly long TARGET_UPDATES_PER_SECOND = 20;
		private static readonly long TARGET_LOOP_TIME_IN_MILLISECONDS = 1000 / TARGET_UPDATES_PER_SECOND;
		private long timeInMilliseconds;

		/// <summary>
		/// Constructor
		/// Runs main server loop
		/// </summary>
		public ServerController ()
		{
            
            // Try load database
			try 
            {
				this.database = new DbaMongo ("mongodb://localhost/nwo");
				this.gameWorld = new GameWorld (LoadAllCharacters (), loadWorldMap ());
			} catch (NWOException e) {
				Console.WriteLine(e);
				Console.WriteLine (" !!! Fatal error !!!\n Failed to load database.");
                Environment.Exit(-1);
			}
            
            // Init time
            time = new Stopwatch();
            time.Start();

            // Start terminal
            this.terminal = new Terminal();

            // Start connectionHandler
            this.ch = new ConnectionHandler();
            ch.Start();

            terminal.Write("--------------------------------------------");
            terminal.Write(" NWO Server running on ip " + ch.GetIpAddress());
            terminal.Write("--------------------------------------------\n");

			while (running) 
            {
				// Handle incoming connection requests from clients
				ch.HandleClientConnectionRequests ();
          
				// Handle incoming messages from clients
				getAllClientMessages ();
        
                // Execute commands from Terminal
				executeCommandsFromQueue ();
              
                // Update Game World
                updateGameWorld();
            
                // Handle disconnected 
                ch.HandleDisconnectedClients();

                // Sleep tight
				sleep ();

			}

            // Save data in database
            saveCharactersToDatabase();

			try {
				saveWorldMap (gameWorld.GetWorldMap ());
			} catch (NWOException e) {
				Console.WriteLine ("Error saving world map: " + e.Message);
			}

        }

		/// <summary>
		/// Handle all messages recieved from the clients
		/// </summary>
		private void getAllClientMessages ()
		{
            List<Client> clients = ch.GetClientList();
			foreach (Client client in clients) 
            {
				// Get all messages for the client
				List<Message> messages = client.RecievAll ();

				// Go through every message recieved and perform action
				foreach (Message m in messages) 
                {

					switch (m.GetMsgType ()) 
                    {
					    // Client login request
					    case Message.Type.CLR:
						{
							ClientLoginRequestMsg clr = (ClientLoginRequestMsg)m;

							//Extract email and password
							string email = clr.GetEmail ();
							string password = clr.GetPassword ();
							bool accepted = false;
							string text = "";
							ServerLoginResponseMsg slr;

							// Verify email and password
							DbaAccount account = null;
							try {
								account = database.GetAccount (email, password);
								switch (account.GetStatus ()) {
								case IdentityStatus.Denied:
									accepted = false;
									text = "Wrong username or password!";
									break;

								case IdentityStatus.Access:
									accepted = true;
									text = "Correct credentials, you are now authorized!";

									client.SetAuthenticated (true);
									terminal.WriteDebug ("Player " + client.GetClientID () + " is authenticated.");
									break;
								}
							} catch (NWOException) {
								text = "Error occured when trying to recieve information from server database.";
								terminal.WriteDebug (text);
							} finally {
								// Send login response to client
                                try
                                {
                                    slr = new ServerLoginResponseMsg(accepted, text);
                                    client.Send(slr);
                                }
                                catch (NWOException e)
                                {
                                    ch.disconnectedClients.Add(client);
                                    terminal.Write(e.Message);
                                }
							}

							if (accepted) 
                            {

                                // Check if there already is someone authenticated on the same account,
                                // If so, kick the old one.
                                for (int i = 0; i < clients.Count; i++)
                                {
                                    Client c = clients[i];
                                    if (c.GetAccount() != null)
                                    {
                                        if (c.GetAccount().Email == email)
                                        {
                                            try
                                            {
                                                string kickText = "You were logged in from somewhere else.";
                                                ServerKickMsg kickMsg = new ServerKickMsg(kickText);
                                                c.Send(kickMsg);
                                                ch.disconnectedClients.Add(c);
                                            }
                                            catch (NWOException e)
                                            {
                                                ch.disconnectedClients.Add(c);
                                                terminal.Write(e.Message);
                                            }
                                        }
                                    }
                                }

                                // Set the clients account to the matching account on database
                                client.SetAccount((ServerAccount)account.GetIdentity());

                                // Load the character from gameworld
                                ServerCharacter character = gameWorld.characterManager.GetCharacterFromAccountId(client.GetAccount()._id.ToString());
                                client.GetAccount().Character = character;

								// Send the account to client
                                try
                                {
                                    ServerSendAccountMsg ssa = new ServerSendAccountMsg(account.GetIdentity().GetAccount());
                                    client.Send(ssa);

                                    if (client.GetAccount().GetAccount().HasCharacter)
                                    {
                                        // Send the character to client
                                        ServerSendCharacterMsg ssc = new ServerSendCharacterMsg(client.GetAccount().Character.GetCharacter());
                                        client.Send(ssc);

										// Send the terrain matrix to client.
										ServerTerrainMatrixMsg tmx = new ServerTerrainMatrixMsg (gameWorld.GetWorldMap().GenerateTerrainMatrix());
										client.Send (tmx);

										Area currentArea = gameWorld.GetWorldMap ().GetArea (client.GetAccount().Character.Position);
										ServerCharacterCurrentAreaMsg sar = new ServerCharacterCurrentAreaMsg(currentArea);
										client.Send (sar);
                                    }
                                }
                                catch (NWOException e)
                                {
                                    ch.disconnectedClients.Add(client);
                                    terminal.Write(e.Message);
                                }
							}// End if(accepted)
							break;
						}

					    // client create account request
					    case Message.Type.CCA:
						{
							ClientCreateAccountRequestMsg cca = (ClientCreateAccountRequestMsg)m;

							//extract requested email and password
							string email = cca.GetEmail ();
							string password = cca.GetPassword ();

							//Don't Verify email and password, it cannot exist
							DbaAccountManager am = database.GetAccountManager ();

							bool accepted = false;
							string text = "";

							try {
								am.RegisterNew (email, password);
								accepted = true;
								text = "";
							} catch (NWOException e) {
								accepted = false;
								text = e.Message;
							}

                            try
                            {
                                ServerCreateAccountResponseMsg sca = new ServerCreateAccountResponseMsg(accepted, text);
                                client.Send(sca);
                            }
                            catch (NWOException e)
                            {
                                ch.disconnectedClients.Add(client);
                                terminal.Write(e.Message);
                            }
							break;
						}

					    // Create character message
					    case Message.Type.CCC:
						{
                            ClientCreateCharacterRequestMsg ccc = (ClientCreateCharacterRequestMsg)m;

							// extract requested character data
							string name = ccc.GetName ();
							string accountId = client.GetAccount ()._id.ToString ();

							// check if character with the same name already exists
							bool accepted = true;
							string text = "";
						
							if (gameWorld.characterManager.GetCharacterFromAccountId (accountId) == null) {
								if (gameWorld.characterManager.GetCharacterFromName (name) == null) {
									if (Regex.IsMatch (name, @"^([a-zA-Z ]{3,20})$")) {
										accepted = true;
										text = "Created character.";
									} else {
										accepted = false;
										text = "Invalid character name.";
									}
								} else {
									accepted = false;
									text = "A character with that name already exists. ";
								}
							} else {
								accepted = false;
								text = "Your account already has a registered character. ";
							}

							ServerCreateCharacterResponseMsg scc = new ServerCreateCharacterResponseMsg (accepted, text);
                            try
                            {
                                client.Send(scc);
                            }
                            catch (NWOException e)
                            {
                                ch.disconnectedClients.Add(client);
                                terminal.Write(e.Message);
                            }

							// If accepted: send the new character
							if (accepted) 
                            {
								// create new character with the requested data
								ServerCharacter newCharacter = new ServerCharacter (name, gameWorld.GetWorldMap ().DefaultStartLocation);
								newCharacter.SetAccountId (accountId);

								// Add it to the gameworld and the account
								ServerAccount account = client.GetAccount();
								gameWorld.characterManager.AddCharacter (newCharacter);
								account.Character = newCharacter;
								database.GetAccountManager().Modify(account);
								//database.GetCharacterManager().CreateCharacter(client.GetAccount());

								// send the new character to client
								ServerSendCharacterMsg ssc = new ServerSendCharacterMsg (newCharacter.GetCharacter ());
                                try
                                {
                                    client.Send(ssc);

									// Send the terrain matrix to client.
									ServerTerrainMatrixMsg tmx = new ServerTerrainMatrixMsg (gameWorld.GetWorldMap().GenerateTerrainMatrix());
									client.Send (tmx);

									Area currentArea = gameWorld.GetWorldMap ().GetArea (client.GetAccount().Character.Position);
									ServerCharacterCurrentAreaMsg sar = new ServerCharacterCurrentAreaMsg(currentArea);
									client.Send (sar);


									// Send the character's current area.
									//Area currentArea = gameWorld.GetWorldMap ().GetArea (newCharacter.Position);
									//ServerCharacterCurrentAreaMsg sar = new ServerCharacterCurrentAreaMsg(currentArea);
									//client.Send (sar);
                                }
                                catch (NWOException e)
                                {
                                    ch.disconnectedClients.Add(client);
                                    terminal.Write(e.Message);
                                }
							}
							break;
						}

                        // Delete character message
                        case Message.Type.CDL:
                        {
                            ServerAccount account = client.GetAccount();
                            if (account == null)
                                break;

                            ServerCharacter character = account.Character;
                            if (character == null)
                                break;

                            // Delete character from GameWorld:
                            gameWorld.characterManager.DeleteCharacter(character.GetAccountId());

                            // Delete char from account
                            account.Character = null;

                            // Update account on db
                            database.GetAccountManager().Modify(account);

                            break;
                        }

					    // Text message
					    case Message.Type.TXT:
						{
							TextMsg msg = (TextMsg)m;

							string text = msg.GetText ();

							terminal.Write("Player " + client.GetClientID () + " wrote:\n " + text);
							break;
						}

					    // Client disconnect
					    case Message.Type.CDC:
						{

							ClientDisconnectMsg cdm = (ClientDisconnectMsg)m;

							string text = cdm.GetText ();

							terminal.WriteDebug("Player " + client.GetClientID () + " has disconnected: " + text);

                            ch.disconnectedClients.Add(client);
							break;
						}
					
						// ClientTrainAttributeRequestMsg
						case Message.Type.CTA:	
						{
                            terminal.WriteDebug("Recieved Train attribute request message.");
							ClientTrainAttributeRequestMsg cta = (ClientTrainAttributeRequestMsg)m;
                            TrainAttributeAction action = new TrainAttributeAction(cta.GetAttributeType());
                            
                            client.GetAccount().Character.EnqueueAction(action);
							break;
						}

                        //ClientUseSkillRequestMsg
                        case Message.Type.CUS:
                        {
                            ClientUseSkillRequestMsg cus = (ClientUseSkillRequestMsg)m;
                            UseSkillAction action = new UseSkillAction(cus.GetSkillType());

                            client.GetAccount().Character.EnqueueAction(action);
                            break;
                        }

                        // Client Area Info message
						case Message.Type.CAI:
						{
							terminal.WriteDebug("Recieved Area info message request from client.");
							ClientAreaInfoRequestMsg cmc = (ClientAreaInfoRequestMsg)m;
							Area area = gameWorld.GetWorldMap ().GetArea (cmc.GetPosition());
							ServerAreaMsg sam = new ServerAreaMsg(area);
                            try
                            {
                                client.Send(sam);
                            }
                            catch (NWOException e)
                            {
                                ch.disconnectedClients.Add(client);
                                terminal.Write(e.Message);
                            }
                            break;
						}

                        // Client Move Character message
						case Message.Type.CMC:
						{
						 	ClientMoveCharacterMsg cmc = (ClientMoveCharacterMsg)m;
							terminal.WriteDebug("Recieved Move character message request from client.");
							// Send the character's current area.
							try{	
								gameWorld.GetWorldMap().MoveCharacterToPosition(client.GetAccount().Character, cmc.GetPosition());

								Area currentArea = gameWorld.GetWorldMap ().GetArea(cmc.GetPosition());
								ServerCharacterCurrentAreaMsg sar = new ServerCharacterCurrentAreaMsg(currentArea);
								
								try
								{
									client.Send(sar);
								}
								catch (NWOException e)
								{
									ch.disconnectedClients.Add(client);
									terminal.Write(e.Message);
								}

							} catch(Exception){}
							
                            break;
						}

                        // ChatMsg
                        case Message.Type.CHT:
                        {
                            ChatMsg cht = (ChatMsg)m;
                            string nameTo = cht.GetName();
                            string text = cht.GetText();
                            string nameFrom = client.GetAccount().Character.Name;
                            
                            if (text == "")
                                break;

                            // Send message to all
                            if (nameTo == "")
                            {
                                if (terminal.showChat)
                                    terminal.Write("Player " + client.GetClientID() + " (" + nameFrom + "): " + text);

                                ChatMsg response = new ChatMsg(nameFrom, text, ChatMsg.ChatType.PUBLIC);
                                foreach (Client c in ch.GetClientList())
                                {
                                    try
                                    {
                                        c.Send(response);
                                    }
                                    catch (NWOException e)
                                    {
                                        ch.disconnectedClients.Add(c);
                                        terminal.Write(e.Message);
                                    }
                                }
                            }

                            // Send message to one person
                            else
                            {
                                ServerCharacter charTo = gameWorld.characterManager.GetCharacterFromName(nameTo);
                                Client c = ch.GetClientFromCharacter(charTo);
                                ChatMsg response;

                                // Reciever does not exist
                                if (c == null)
                                {
                                    response = new ChatMsg("Server", "Unknown player.", ChatMsg.ChatType.PRIVATE);
                                    try
                                    {
                                        client.Send(response);
                                    }
                                    catch (NWOException e)
                                    {
                                        ch.disconnectedClients.Add(client);
                                        terminal.Write(e.Message);
                                    }
                                }

                                // Reciever exists
                                else
                                {
                                    response = new ChatMsg(nameFrom, text, ChatMsg.ChatType.PRIVATE);
                                    ChatMsg toSender = new ChatMsg("To " + nameTo, text, ChatMsg.ChatType.PRIVATE);
                                    try
                                    {
                                        client.Send(toSender);
                                    }
                                    catch (NWOException e)
                                    {
                                        ch.disconnectedClients.Add(client);
                                        terminal.Write(e.Message);
                                    }
                                    try
                                    {
                                        c.Send(response);
                                    }
                                    catch (NWOException e)
                                    {
                                        ch.disconnectedClients.Add(c);
                                        terminal.Write(e.Message);
                                    }
                                }
                            }

                            break;
                        }

					}// End switch (m.GetMsgType())
				}// End foreach (Message m in messages)
			}// End foreach(Client c in clients)
		}

		/// <summary>
		/// Execute all commands from the command queue from the terminal
		/// </summary>
		private void executeCommandsFromQueue ()
		{
			Command command;

			// Loop while there is a command to dequeue
			while (terminal.commandQueue.TryDequeue(out command)) 
            {

				/* Check command type */

				// Kick a player
				if (command is CommandKickClient) 
                {
					CommandKickClient cmd = (CommandKickClient)command;
					try 
                    {
						ch.KickClient (cmd.getPlayerID (), cmd.getText ());
						terminal.Write ("Kicked player " + cmd.getPlayerID () + ".");
					} 
                    catch (NoSuchClientException) 
                    {
						terminal.Write ("Player does not exist.");
					}
				}

                // Kick all players
                else if (command is CommandKickAllClients) {
					if (ch.GetClientList ().Count == 0) {
						terminal.Write ("No players connected.");
					} else {
						terminal.Write ("Kicked " + ch.GetClientList ().Count + " players.");

						CommandKickAllClients cmd = (CommandKickAllClients)command;
						ch.KickAllClients (cmd.getText ());
					}
				}

                // Shutdown server
                else if (command is CommandShutdown) {
					CommandShutdown cmd = (CommandShutdown)command;
					shutdown (cmd.getText ());

					terminal.Write ("Server is shutting down.\n");
				}

                // Send text to a player
                else if (command is CommandSendText) {
					CommandSendText cmd = (CommandSendText)command;
					try {
						ch.SendClientTextMessage (cmd.getPlayerID (), cmd.getText ());
						terminal.Write ("Message sent.");
					} catch (NoSuchClientException) {
						terminal.Write ("Player " + cmd.getPlayerID () + " does not exist.");
					}
				}

                // Send text to all players
                else if (command is CommandSendBroadcastText) {
					if (ch.GetClientList ().Count == 0) {
						terminal.Write ("No players connected.");
					} else {
						CommandSendBroadcastText cmd = (CommandSendBroadcastText)command;
						ch.SendBroadcastTextMessage (cmd.getText ());

						terminal.Write ("Message sent.");
					}
				}

                // List all clients
                else if (command is CommandListAllClients) {
					terminal.Write ("----------------------------------------------------------------------------");
                    terminal.Write (" SID\tIP\t\tAuth\tEmail\t\t\tCharacter\tOnline");
					terminal.Write ("----------------------------------------------------------------------------");
					foreach (Client c in ch.GetClientList()) {
						string auth, characterName = "";
                        string email = "";
                        ServerAccount account;
                        ServerCharacter character;

                        account = c.GetAccount();
                        if (account != null)
                        {
                            email = account.Email;
                            if (email.Length > 20)
                            {
                                email = email.Insert(20, "...");
                                email = email.Remove(23);
                            }

                            email = String.Format("{0,-23}", email);
                            character = account.Character;
                            if (character != null)
                            {
                                characterName = character.GetCharacter().Name;
                            }
                        }

						if (c.GetAuthenticated ())
							auth = "YES";
						else
							auth = "NO";

                        terminal.Write(" " + c.GetClientID() + "\t" + c.GetIP() + "\t" + auth + "\t" + email + "\t" + characterName);
                        
					}
					terminal.Write (" ");
				}

                // List all characters
                else if (command is CommandListAllAccounts)
                {
                    terminal.Write("---------------------------------------------------");
                    terminal.Write("<Account email>\t\t\t   <Character>");
                    terminal.Write("---------------------------------------------------");
                    foreach (ServerAccount a in loadAllAccounts())
                    {
                        string charName = "-";
                        if(a.Character != null)
                            charName = a.Character.Name;

                        terminal.Write(String.Format(" {0,-40}", a.Email) + charName);
                    }
                }
			}
		}

        /// <summary>
        /// Update GameWorld
        /// </summary>
        private void updateGameWorld()
        {
            // Update character actions
            long currentTime = time.ElapsedMilliseconds;

            foreach (ServerCharacter character in gameWorld.characterManager.GetCharacters())
            {
                Server.Model.Action newFinishedAction;
                Server.Model.Action newPerformingAction;

                // Update the characters action
                character.UpdateAction(currentTime, out newPerformingAction, out newFinishedAction);

                // If character client is online, send messages to client
                Client client = ch.GetClientFromCharacter(character);
                if (client != null)
                {
                    // Send finished action message to client
                    if (newFinishedAction != null)
                    {
                        if (newFinishedAction is TrainAttributeAction)
                        {
                            // Get Action result
                            TrainAttributeAction taa = (TrainAttributeAction)newFinishedAction;

                            int result;
                            taa.GetAttributeGainResult(out result);

                            // Send result Message
                            try
                            {
                                ServerTrainAttributeResultMsg str =
                                    new ServerTrainAttributeResultMsg(taa.attributeType, result);

                                client.Send(str);

                                // Send Character
                                ServerSendCharacterMsg ssc = new ServerSendCharacterMsg(character.GetCharacter());
                                client.Send(ssc);
                            }
                            catch (NWOException e)
                            {
                                ch.disconnectedClients.Add(client);
                                terminal.Write(e.Message);
                            }
                        }
                        else if (newFinishedAction is UseSkillAction)
                        {
                            // Get Action result
                            UseSkillAction usa = (UseSkillAction)newFinishedAction;

                            int skillGain;
                            string result;
                            usa.GetSkillUseResult(out result, out skillGain);

                            // Send result Message
                            try
                            {
                                ServerUseSkillResultMsg sur =
                                    new ServerUseSkillResultMsg(usa.skillType, result);

                                client.Send(sur);

                                // Send Character
                                ServerSendCharacterMsg ssc = new ServerSendCharacterMsg(character.GetCharacter());
                                client.Send(ssc);
                            }
                            catch (NWOException e)
                            {
                                ch.disconnectedClients.Add(client);
                                terminal.Write(e.Message);
                            }
                        }
                        else
                        {
                            throw new NWOException("Unhandeled action type!");
                        }
                    }

                    // Send performing action message to client
                    if (newPerformingAction != null)
                    {
                        if(newPerformingAction is TrainAttributeAction)
                        {
                            // Downcast action to TrainAttributeAction
                            TrainAttributeAction taa = (TrainAttributeAction)newPerformingAction;

                            ServerTrainAttributeResponseMsg sta;
                            sta = new ServerTrainAttributeResponseMsg(taa.attributeType, true, "");
                            
                            try
                            {
                                client.Send(sta);
                            }
                            catch (NWOException e)
                            {
                                ch.disconnectedClients.Add(client);
                                terminal.Write(e.Message);
                            }
                        }
                        else if (newPerformingAction is UseSkillAction)
                        {
                            // Downcast action to UseSkillAction
                            UseSkillAction usa = (UseSkillAction)newPerformingAction;

                            ServerUseSkillResponseMsg sus;
                            sus = new ServerUseSkillResponseMsg(usa.skillType, true, "");

                            try
                            {
                                client.Send(sus);
                            }
                            catch (NWOException e)
                            {
                                ch.disconnectedClients.Add(client);
                                terminal.Write(e.Message);
                            }
                        }
                        else
                        {
                            throw new NWOException("Unhandeled action type!");
                        }
                    }
                }// End if(client != null)
            }// End foreach(ServerCharacter character)
        }

        /// <returns>
        /// List of server Characters
        /// </returns>
		private List<ServerCharacter> LoadAllCharacters ()
		{
            List<ServerAccount> accountList = loadAllAccounts();
            List<ServerCharacter> characterList = new List<ServerCharacter> ();

			ServerCharacter tempCharacter;

			foreach (ServerAccount account in accountList) {
				tempCharacter = (ServerCharacter)account.Character;

				if (tempCharacter != null) {
					tempCharacter.SetAccountId (account._id.ToString ());
					characterList.Add (tempCharacter);
				}
			}

			return characterList;
		}

		/// <summary>
		/// Loads the world map from database, or generates a new one if none in database.
		/// </summary>
		/// <returns>
		/// The world map.
		/// </returns>
		private WorldMap loadWorldMap ()
		{
			try {
				return database.GetWorldMapManager ().Get ();
			} catch (NWOException) {
				return new WorldMap(10, 10);
			}
		}

		/// <summary>
		/// Saves the world map to database.
		/// </summary>
		/// <param name='worldMap'>
		/// World map.
		/// </param>
		/// <exception cref="NWOException">
		/// In case the world map cannot be saved.
		/// </exception>
		private void saveWorldMap (WorldMap worldMap)
		{
			database.GetWorldMapManager ().Save (worldMap);
		}

        /// <summary>
        /// Load all Accounts from the database
        /// </summary>
        /// <returns>
        /// List of ServerAccounts
        /// </returns>
        private List<ServerAccount> loadAllAccounts()
        {
            DbaAccountManager accountManager = database.GetAccountManager();
            if (accountManager.GetAccounts() == null)
                throw new NWOException();

            return accountManager.GetAccounts();
        }

        /// <summary>
        /// Load all Characters from database
        /// </summary>
        /// <returns>
        /// List of server Characters
        /// </returns>
		private List<ServerCharacter> loadAllCharacters ()
		{
            List<ServerAccount> accountList = loadAllAccounts();
			List<ServerCharacter> characterList = new List<ServerCharacter> ();

			ServerCharacter tempCharacter;

			foreach (ServerAccount account in accountList) {
				tempCharacter = (ServerCharacter)account.Character;

				if (tempCharacter != null) {
					tempCharacter.SetAccountId (account._id.ToString ());
					characterList.Add (tempCharacter);
				}
			}
			return characterList;
		}

        /// <summary>
        /// Save all characters to database
        /// </summary>
        private void saveCharactersToDatabase()
        {
            List<ServerAccount> accounts = loadAllAccounts();
            foreach (ServerAccount a in accounts)
            {
                a.Character = gameWorld.characterManager.GetCharacterFromAccountId(a._id.ToString());
                database.GetAccountManager().Modify(a);
            }
        }

        /// <summary>
        /// Send shutdown message to all clients, 
        /// remove them and set running to false
        /// </summary>
        /// <param name="text"></param>		 
        private void shutdown(string text)
        {
            // Disconnect all clients and stop listening to new clients
            ch.Disconnect(text);

            // Stop run main loop
            running = false;
        }

        /// <summary>
        /// Sleep tight
        /// </summary>
        private void sleep()
        {
            long elapsedTime = ((DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond) - timeInMilliseconds);
            long targetSleepTime = TARGET_LOOP_TIME_IN_MILLISECONDS - elapsedTime;

            if (targetSleepTime > 0)
                Thread.Sleep((int)targetSleepTime);

            timeInMilliseconds = (DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond);
        }

	}
}

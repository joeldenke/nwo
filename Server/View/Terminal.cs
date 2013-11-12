using System;
using System.Threading;
using System.Collections.Concurrent;
using Server.Controller;

namespace Server.View
{
	/// <summary>
	/// The server gui
	/// </summary>
	public class Terminal
	{
        public ConcurrentQueue<Command> commandQueue;
        public bool showChat = true;
        private bool running;
		private Thread thread;

        /// <summary>
        /// Constructor, starts thread
        /// </summary>
		public Terminal ()
		{
			running = true;
            commandQueue = new ConcurrentQueue<Command>();

			// Start thread
			thread = new Thread(new ThreadStart(this.Run));
			thread.Start();
		}

        /// <summary>
        /// Write to the terminal
        /// </summary>
        /// <param name="output"></param>
        public void Write(string output)
        {
            Console.WriteLine(" > " + output);
        }

        /// <summary>
        /// Write to terminal debug mode
        /// </summary>
        /// <param name="output"></param>
        public void WriteDebug(string output)
        {
            Console.WriteLine(" # " + output);
        }

		/// <summary>
		/// Thread runs this method
        /// Read user input, add command to queue
		/// </summary>
		private void Run()
		{
			string input;
			string[] inputs = new string[10];
            
			while(running)
			{
				input = Console.In.ReadLine();
				inputs = input.Split(" ".ToCharArray(), 10);

				// Command: exit
				if(inputs[0] == "exit")
				{
					// Stop this running loop
					running=false;

					// Send shutdown command
					CommandShutdown cmd = new CommandShutdown("Server is shuting down.");
                    commandQueue.Enqueue(cmd);
				}

				// Command: send
				else if(inputs[0] == "send")
				{
                    string usageInfo = "Usage:\n"
                        + " 1) send all <message>\n"
                        + " 2) send <id> <message>\n";

                    // More then one word input
                    if (inputs.Length > 1)
                    {
                        if (inputs[1] == "all")
                        {
                            if (inputs.Length > 2)
                            {
                                string text = "";
                                for (int i = 2; i < inputs.Length; i++)
                                    text += inputs[i] + " ";

                                CommandSendBroadcastText cmd = new CommandSendBroadcastText(text);
                                commandQueue.Enqueue(cmd);
                            }
                            else
                            {
                                Write(usageInfo);
                            }
                        }
                        else
                        {
                            if (inputs.Length > 2)
                            {
                                try
                                {
                                    string text = "";
                                    int playerID = Convert.ToInt32(inputs[1]);

                                    for (int i = 2; i < inputs.Length; i++)
                                        text += inputs[i] + " ";

                                    CommandSendText cmd = new CommandSendText(playerID, text);
                                    commandQueue.Enqueue(cmd);
                                }
                                catch (FormatException)
                                {
                                    Write(usageInfo);
                                }
                            }
                            else
                            {
                                Write(usageInfo);
                            }
                        }
                    }
                    // if !(inputs.Length > 1)
                    else
                    {
                        Write(usageInfo);
                    }
				}
				// Command: kick
				else if(inputs[0] == "kick")
				{
                    string usageInfo = "Usage:\n"
                        + " 1) kick all\n"
                        + " 2) kick all <message>\n"
                        + " 3) kick <id>\n"
                        + " 4) kick <id> <message>\n";

                    // If there is more than one parameter
                    if (inputs.Length > 1)
                    {
                        // Kick all players
                        if (inputs[1] == "all")
                        {
                            string text = "";

                            // If a text message is included
                            if (inputs.Length > 2)
                            {
                                for (int i = 2; i < inputs.Length; i++)
                                    text += inputs[i] + " ";
                            }

                            // Enqueue
                            CommandKickAllClients cmd = new CommandKickAllClients(text);
                            commandQueue.Enqueue(cmd);
                        }
                        // Kick a player
                        else
                        {
                            int playerID = 0;

                            try
                            {
                                playerID = Convert.ToInt32(inputs[1]);
                                string text = "";

                                // If a text message is included
                                if (inputs.Length > 2)
                                {
                                    for (int i = 2; i < inputs.Length; i++)
                                        text += inputs[i] + " ";
                                }
                                
                                // Enqueue
                                CommandKickClient cmd = new CommandKickClient(playerID, text);
                                commandQueue.Enqueue(cmd);
                            }
                            catch (FormatException)
                            {
                                Write(usageInfo);
                            }
                        }
                    }
                    // else if !(inputs.Length >= 1)
                    else
                    {
                        Write(usageInfo);
                    }

                        
				}
				// Command: list
				else if(inputs[0] == "clients")
				{
					CommandListAllClients cmd = new CommandListAllClients();
					commandQueue.Enqueue(cmd);
				}

                // Command: characters
                else if (inputs[0] == "accounts")
                {
                    CommandListAllAccounts cmd = new CommandListAllAccounts();
                    commandQueue.Enqueue(cmd);
                }

                // Command: help
                else if (inputs[0] == "help")
                {
                    Write("---------------------------");
                    Write("Command usage\tInfo");
                    Write("---------------------------");
                    Write("help\t\t\tShows this help text.");
                    Write("exit\t\t\tDisconnect all clients and shut down the server.");
                    Write("send all <message>\tSend text <message> to all players.");
                    Write("send <id> <message>\tSend text <message> to player with id <id>.");
                    Write("kick all <message>\tKick all players with text <message>.");
                    Write("kick <id> <message>\tKick player <id> with text <message>.");
                    Write("clients\t\tShows a list of all connected clients.");
                    Write("accounts\t\tShows a list of all characters on the server.");
                    Write("chat\t\t\tToggle to display player chat or not");
                    Write("");
                }
                else if (inputs[0] == "chat")
                {
                    if (showChat)
                    {
                        Write("Display player chat disabled.");
                        showChat = false;
                    }
                    else
                    {
                        Write("Display player chat enabled.");
                        showChat = true;
                    }
                }
                // Unknown command
                else if (inputs[0] != "")
                {
                    Write("Unknown command. Type 'help' to list avaiable commands.\n");
                }
			}

			// Stop thread
			thread.Abort();

			Write("Server terminal aborted!\n");

		}// void RunThread()

	}
}


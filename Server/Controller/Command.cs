using System;
//These are different commands that are invoked by the terminal View.
namespace Server.Controller
{
	public abstract class Command
	{

	}

	public class CommandKickClient : Command
	{
		private int playerID;
		private string text;

		public CommandKickClient(int playerID, string text)
		{
			this.playerID = playerID;
			this.text = text;
		}

		public int getPlayerID()
		{
			return playerID;
		}

		public string getText()
		{
			return text;
		}
	}

	public class CommandKickAllClients : Command
	{
		private string text;
		
		public CommandKickAllClients(string text)
		{
			this.text = text;
		}
		
		public string getText()
		{
			return text;
		}
	}

	public class CommandShutdown : Command
	{
		private string text;
		
		public CommandShutdown(string text)
		{
			this.text = text;
		}

		public string getText()
		{
			return text;
		}
	}

	public class CommandSendText : Command
	{
		private int playerID;
		private string text;
		
		public CommandSendText(int playerID, string text)
		{
			this.playerID = playerID;
			this.text = text;
		}
		
		public int getPlayerID()
		{
			return playerID;
		}
		
		public string getText()
		{
			return text;
		}
	}

	public class CommandSendBroadcastText : Command
	{
		private string text;
		
		public CommandSendBroadcastText(string text)
		{
			this.text = text;
		}

		public string getText()
		{
			return text;
		}
	}

	public class CommandListAllClients : Command
	{

	}

    public class CommandListAllAccounts : Command
    {

    }
}


using System;

namespace Client.Model
{
	public class ModelManager
	{
		private CharacterManager characterManager;
		private AccountManager accountManager;
		private ServerManager serverManager;
		private WorldManager worldManager;

		public ModelManager (string ip, int port)
		{
			characterManager = new CharacterManager();
			accountManager = new AccountManager();
			serverManager = new ServerManager(ip, port);
			worldManager = new WorldManager();
		}

		public CharacterManager GetCharacterManager ()
		{
			return characterManager;
		}

		public AccountManager GetAccountManager ()
		{
			return accountManager;
		}

		public ServerManager GetServerManager ()
		{
			return serverManager;
		}

		public WorldManager GetWorldManager ()
		{
			return worldManager;
		}
	}
}


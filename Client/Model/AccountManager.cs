using System;
using Common;

namespace Client.Model
{
	public class AccountManager
	{
		private Account myAccount;
		public AccountManager ()
		{
		}

		public void SetMyAccount (Account account)
		{
			myAccount = account;
		}
		public Account GetMyAccount ()
		{
			return myAccount;
		}
	}
}


using System;
using Common;
using MongoDB.Bson;

namespace Server.Model
{
	public class ServerAccount
	{
		public string Password;
		public ObjectId _id;
		private ServerCharacter character;
		private Account account;

        /// <summary>
        /// 
        /// </summary>
		public ServerCharacter Character {
			get {
				return character;
			}
			set {
				ensureAccount();
				account.HasCharacter = (value != null);
				character = value;
			}
		}

        /// <summary>
        /// 
        /// </summary>
		public string Email {
			get {
				return account.Email;
			}
			set {
				ensureAccount();
				account.Email = value;
			}
		}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="email"></param>
        /// <param name="password"></param>
		public ServerAccount (string email, string password)
		{
			account = new Account(email, false);
			this.Password = password;
			Character = null;
		}

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
		public Account GetAccount ()
		{
			return account;
		}

        /// <summary>
        /// 
        /// </summary>
		private void ensureAccount ()
		{
			if (account == null)
				account = new Account ("", false);
		}
	}
}

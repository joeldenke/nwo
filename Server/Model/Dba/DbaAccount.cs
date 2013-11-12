using System;
using System.Diagnostics;
using MongoDB.Driver;
using Common;
using Server.Model;

public enum IdentityStatus {
	Access,
	Denied
};

namespace Server.Dba
{
	/// <summary>
	/// Handle Account authorizations
	/// </summary>
	public class DbaAccount
	{
		private ServerAccount identity = null;
		private DbaMongo dba;
		private IdentityStatus status = IdentityStatus.Denied;

		/// <summary>
		/// Constructor which create identity if account exists and nothing if not
		/// </summary>
		/// <param name="db">Instance of database</param>
		/// <param name="email">Email</param>
		/// <param name="password">Password</param>
		public DbaAccount (DbaMongo db, string email, string password)
		{
			Debug.Assert (db != null && email != null && password != null);

			dba = db;
			Identify (email, password);
		}

		/// <summary>
		/// Identify Account credentials given
		/// </summary>
		/// <param name="email">Email</param>
		/// <param name="password">Password</param>
		public void Identify (string email, string password)
		{
			Debug.Assert (email != null && password != null);

			var query = new QueryDocument{{"Email", email},{"Password", Crypt.GenHash(password)}};

			MongoCollection<ServerAccount> c = dba.GetCollection<ServerAccount>("accounts");
			ServerAccount a = c.FindOne (query);

			if (a is ServerAccount) {
				status = IdentityStatus.Access;
				identity = a;
			}
		}

		/// <summary>
		/// Get current identity status
		/// </summary>
		/// <returns>Enumeration of current status</returns>
		public IdentityStatus GetStatus ()
		{
			return status;
		}

		/// <summary>
		/// Verify current account
		/// </summary>
		public bool Verify ()
		{
			return true;
		}

		/// <summary>
		/// Clear identity
		/// </summary>
		public void ClearIdentity ()
		{
			identity = null;
		}

		/// <summary>
		/// Get account from current identity
		/// </summary>
		/// <returns>Null if none and Account instance else</returns>
		public ServerAccount GetIdentity ()
		{
			return identity;
		}
	}
}
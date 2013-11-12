using System;
using System.Diagnostics;
using MongoDB.Driver;
using MongoDB.Bson;
using Common;
using Server.Model;
using System.Collections.Generic;

namespace Server.Dba
{
	/// <summary>
	/// Handle accounts in MongoDB database collection. Possible to modify, delete and register
	/// 	accounts.
	/// </summary>
	public class DbaAccountManager
	{
		MongoCollection<ServerAccount> accounts;
		DbaMongo mongo;

		/// <summary>
		/// Constructor which fetch account collection from database
		/// </summary>
		/// <param name="db">Instance of Database abstraction class</param>
		public DbaAccountManager (DbaMongo db)
		{
			Debug.Assert (db != null);
			accounts = db.GetCollection<ServerAccount>("accounts");
			mongo = db;
		}

		/// <summary>
		/// Get all accounts from collection
		/// </summary>
		public List<ServerAccount> GetAccounts ()
		{
			return mongo.GetListFromCollection<ServerAccount>(accounts);
		}

		/// <summary>
		/// Register new account
		/// </summary>
		/// <returns>True or false if email do not exists, else throws NWOException</returns>
		public void RegisterNew (string email, string password)
		{
			Debug.Assert (email != null && password != null);

			var query = new QueryDocument("Email", email);

			if (accounts.FindOne(query) == null) {
				try {
					accounts.Insert(new ServerAccount (email, Crypt.GenHash(password)));
				} catch (MongoException me) {
					throw new NWOException("Failed to add account!", me);
				}
			} else {
				throw new NWOException("Account with email " + email + " already exist!");
			}
		}

		/// <summary>
		/// Modify current account
		/// </summary>
		/// <returns>True on update success, else false</returns>
		public bool Modify (ServerAccount user)
		{
			Debug.Assert (user != null);
			try {
				/// Important to have Id in the item object (See @Entity in Common.Models)
				/// If not, it will insert a new item.
				accounts.Save(user);
				return true;
			} catch (MongoException me) {
				Console.WriteLine (me.ToString());
				return false;
			}
		}

		/// <summary>
		/// Delete an existing account
		/// </summary>
		/// <returns>True on delete success, else false</returns>
		public bool Delete (ServerAccount user)
		{
			Debug.Assert (user != null);
			IMongoQuery query = new QueryDocument("_id", user._id);

			try {
				accounts.Remove(query);
				return true;
			} catch (MongoException me) {
				Console.WriteLine (me.ToString());
				return false;
			}
		}
	}
}


using System;
using System.Diagnostics;
using System.Text.RegularExpressions;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using MongoDB.Configuration;
using Common;
using System.Collections.Generic;

namespace Server.Dba
{
	/// <summary>
	/// Database abstraction layer of MongoDB
	/// </summary>
    public class DbaMongo
	{
		MongoServer server;
		MongoDatabase db;
		string dbName;

		/// <summary>
		/// Constructor which connecto to MongoDB and goes into nwo database
		/// </summary>
        public DbaMongo (string connectionString)
		{
			/*
			 * @TODO in SPRINT 2
				MongoConfigurationBuilder mcb = new MongoConfigurationBuilder();
				mcb.ReadConnectionStringFromAppSettings("test");
				string connectionString = mcb.BuildConfiguration().ConnectionString;
				Console.WriteLine(connectionString);
			 */

			try {
				//String database = Regex.Matches(connectionString, @"^mongodb://.*[/]+(.)*[?]?")[0];
				MongoUrl url = MongoUrl.Create(connectionString);
				dbName = url.DatabaseName;

				if (dbName == null) {
					dbName = "nwo";
				} else {
					connectionString = connectionString.Replace(dbName, "");
				}

				server = new MongoClient(connectionString).GetServer();
			} catch (MongoException t) {
				throw new NWOException("Failed to connect to database!", t);
			}
        }

		/// <summary>
		/// Get current server connection to MongoDB
		/// </summary>
		/// <returns>MongoServer instance</returns>
		public MongoServer GetConnection ()
		{
			if (server.State == MongoServerState.Disconnected)
				server.Connect();

			return server;
		}

		/// <summary>
		/// Create (if not exists) and return database
		/// </summary>
		/// <param name="database">Database name</param>
		/// <returns>Instance of database</returns>
		public MongoDatabase GetDatabase (string database)
		{
			if (database == null) {
				database = dbName;
			}

			try {
				if (db is MongoDatabase) {
					return db;
				} else {
					db = server.GetDatabase(database);
					return db;
				}
			} catch (MongoException me) {
				throw new NWOException ("Failed to get database'" + database + "' from server.", me);
			}
		}

		public List<T> GetListFromCollection<T>(MongoCollection<T> col)
		{
			try {
				MongoCursor cursor = col.FindAll();
				List<T> list = new List<T>();
				foreach(T obj in cursor)
					list.Add(obj);

				return list;
			} catch (MongoException) {
				return null;
			}
		}

		/// <summary>
		/// Gets the newest entry from collection.
		/// </summary>
		/// <returns>
		/// The newest from entry collection.
		/// </returns>
		/// <param name='collection'>
		/// Collection.
		/// </param>
		public T GetNewestFromCollection<T> (MongoCollection<T> collection)
		{
			try {
				MongoCursor cursor = collection.FindAll().SetSortOrder(SortBy.Descending("_id")).SetLimit(1);
				foreach(T obj in cursor) {
					return obj;
				}
				throw new NWOException ("Collection is empty.");

			} catch (MongoException e) {
				Debug.WriteLine(e);
				throw new NWOException ("Unable to retrieve newest entry from collection.");
			}
		}

		/// <summary>
		/// Get a user specified database collection
		/// </summary>
		/// <param name="name">Name of collection</param>
		/// <returns>Instance of the collection</returns>
		public MongoCollection<T> GetCollection<T>(string name)
		{
			return GetDatabase(null).GetCollection<T>(name);
		}

		/// <summary>
		/// Get database authorization
		/// </summary>
		/// <param name="email">Email</param>
		/// <param name="password">Password</param>
		/// <returns>Instance of account</returns>
		public DbaAccount GetAccount (string email, string password)
		{
			return new DbaAccount(this, email, password);
		}

		/// <summary>
		/// Get database account manager
		/// </summary>
		/// <returns>Instance of account manager</returns>
		public DbaAccountManager GetAccountManager()
		{
			return new DbaAccountManager(this);
		}

		public DbaWorldMapManager GetWorldMapManager()
		{
			return new DbaWorldMapManager(this);
		}
    }
}


using System;
using System.Diagnostics;
using MongoDB.Driver;
using MongoDB.Bson;
using Common;
using Server.Model;
using System.Collections.Generic;

namespace Server.Dba
{
	public class DbaWorldMapManager
	{
		private MongoCollection<WorldMap> worldMaps;
		private DbaMongo mongo;

		/// <summary>
		/// Initializes a new instance of the <see cref="Server.Dba.DbaWorldMapManager"/> class.
		/// </summary>
		/// <param name='db'>
		/// Db.
		/// </param>
		public DbaWorldMapManager (DbaMongo db)
		{
			Debug.Assert (db != null);
			worldMaps = db.GetCollection<WorldMap> ("worldmaps");
			mongo = db;
		}

		/// <summary>
		/// Get worldMap from database.
		/// </summary>
		public WorldMap Get ()
		{
			return mongo.GetNewestFromCollection<WorldMap>(worldMaps);
		}

		/// <summary>
		/// Save the specified worldMap.
		/// </summary>
		/// <param name='worldMap'>
		/// World map.
		/// </param>
		public void Save (WorldMap worldMap)
		{
			Debug.Assert (worldMap != null);

			try {
				worldMaps.Save(worldMap);
			} catch (MongoException e) {
				Debug.WriteLine(e);
				throw new NWOException("Unable to save game world." + e.Message);
			}
		}

		/// <summary>
		/// Delete worldMap from database.
		/// </summary>
		public void Delete (WorldMap worldMap)
		{
			Debug.Assert (worldMap != null);

			IMongoQuery query = new QueryDocument ("_id", worldMap._id);
			try {
				worldMaps.Remove (query);

			} catch (MongoException e) {
				Debug.WriteLine(e);
				throw new NWOException("Unable to delete game world from database. " + e.Message);
			}
		}
	}
}


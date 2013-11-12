using System;
using NUnit.Framework;
using Server.Dba;
using Server.Model;
using Common;
using MongoDB.Bson;

namespace TestServer
{
	[TestFixture]
	[Ignore]
	public class TestDbaWorldMapManager
	{
		public DbaMongo db;
		public DbaWorldMapManager manager;

		[TestFixtureSetUp]
		public void setup ()
		{
			try {
				db = new DbaMongo ("mongodb://localhost/nwotest");
				manager = db.GetWorldMapManager ();
			} catch (Exception e) {
				Console.WriteLine (e);
				throw e;
			}
		}

		[Test]
		public void ShouldSaveLoadAndRemoveWorldMap ()
		{
			WorldMap worldMap = new WorldMap (4, 5);
			Position position = new Position(2, 3);
			worldMap.GetArea(position).EstateType = AreaEstateType.RUINS;

			manager.Save (worldMap);
			worldMap = null;

			worldMap = manager.Get();
            Console.WriteLine(worldMap._id);
			Assert.IsTrue (worldMap.GetArea(position).EstateType == AreaEstateType.RUINS); 

			manager.Delete (worldMap);

			Assert.Pass();
		}

	}
}
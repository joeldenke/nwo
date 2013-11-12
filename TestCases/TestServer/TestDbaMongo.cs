using System;
using NUnit.Framework;
using MongoDB.Driver;
using Server.Dba;

namespace TestServer
{
	[TestFixture()]
	public class TestDbaMongo
	{
		private DbaMongo mongo = new DbaMongo("mongodb://localhost/nwo");
		private MongoServer connection;
		private MongoDatabase db;

		[Test()]
		public void ShouldGetValidMongoDBConnection()
		{
			connection = mongo.GetConnection();
			Assert.IsInstanceOf<MongoServer>(connection);
		}

		[Test()]
		public void ShouldGetValidDatabase () 
		{
			db = mongo.GetDatabase("nwo");
			Assert.IsNotNull (db);
		}
	}
}
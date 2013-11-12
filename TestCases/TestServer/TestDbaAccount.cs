using System;
using NUnit.Framework;
using MongoDB.Driver;
using Common;
using Server.Dba;
using Server.Model;

namespace TestServer
{
	[TestFixture()]
	public class TestDbaAccount
	{
		public DbaAccount a = new DbaMongo("mongodb://localhost/nwo").GetAccount("lol@lol.com", "lolipop"); 

		[Test()]
		public void ShouldValidateAsAccessedAccountIfExists()
		{
			Account ac = a.GetIdentity().GetAccount ();
			Assert.IsInstanceOf<ServerAccount>(ac);
			Assert.AreEqual(IdentityStatus.Access, a.GetStatus ());
		}

		[Test()]
		public void ShouldValidateTrueOrFalseFromVerify()
		{
			Assert.AreEqual(typeof(bool), a.Verify().GetType());
		}
	}
}


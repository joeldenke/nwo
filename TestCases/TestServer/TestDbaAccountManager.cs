using System;
using NUnit.Framework;
using Server.Dba;
using Server.Model;
using Common;

namespace TestServer
{
	[TestFixture()]
	public class TestAccountManager
	{
		public DbaMongo db = new DbaMongo("mongodb://localhost/nwo");
		public DbaAccountManager ma;

		[Test()]
		public void ShouldInsertAccountIntoCollection()
		{
			ma = db.GetAccountManager ();
			try{
				ma.RegisterNew ("test@test.com", "testing");
			}catch(NWOException){}
			ServerAccount u = db.GetAccount("test@test.com", "testing").GetIdentity();
			Assert.IsNotNull (u);
			Assert.IsTrue (ma.Delete(u));
		}

		[Test()]
		public void ShouldLoginAndUpdateAccountInCollection()
		{
			try{
				ma.RegisterNew ("test@test.com", "testing");
			}catch(NWOException){}			
			ServerAccount u = db.GetAccount("test@test.com", "testing").GetIdentity();
			Assert.IsNotNull (u);
			u.Password = Crypt.GenHash("testing");
			Assert.IsTrue (ma.Modify(u));
			Assert.IsTrue (ma.Delete(u));
		}

		[Test()]
		public void ShouldRemoveAccountFromCollectionIfIdExists()
		{
			try{
				ma.RegisterNew ("test@test.com", "testing");
			}catch(NWOException){}	
			ServerAccount u = db.GetAccount("test@test.com", "testing").GetIdentity();
			Assert.IsInstanceOf<ServerAccount>(u);
			Assert.IsTrue (ma.Delete(u));
		}
	}
}
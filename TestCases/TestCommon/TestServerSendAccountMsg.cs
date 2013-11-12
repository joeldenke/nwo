using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

using NUnit.Framework;
using Common;

namespace TestCommon
{
	[TestFixture()]
	public class TestServerSendAccountMsg
	{
		Account a;
		MessageManager mm;
		MemoryStream ms;
		BinaryFormatter bf;

		[SetUp()]
		public void InitTestAccount()
		{
			a = new Account("test@test.com", true);
			mm = new MessageManager();
			ms = new MemoryStream();
			bf = new BinaryFormatter();

		}

		//
		// TEST PACK
		//
		[Test()]
		public void ShouldCorrectlyPackAccountMsg()
		{
			ServerSendAccountMsg ssa = new ServerSendAccountMsg(a);
			bf.Serialize(ms, a);
			string data = System.Convert.ToBase64String( ms.ToArray () );

			string expected = "SSA" + data + "\x001B";

			Assert.IsTrue(mm.Pack (ssa) == expected);
		}

		// 
		// TEST UNPACK
		//
		[Test()]
		public void ShouldUnpackCorrectEmail()
		{
			ServerSendAccountMsg ssa1 = new ServerSendAccountMsg(a);
			string rawString = mm.Pack (ssa1);

			ServerSendAccountMsg ssa = (ServerSendAccountMsg) mm.Unpack(rawString);

			Assert.IsTrue ( ssa.GetAccount().Email == "test@test.com" );
		}

		[Test()]
		public void ShouldUnpackCorrectHasCharacterBool()
		{
			bf.Serialize(ms, a);
			string data = System.Convert.ToBase64String( ms.ToArray () );
			
			string rawString = "SSA" + data + "\x001B";
			
			ServerSendAccountMsg ssa = (ServerSendAccountMsg) mm.Unpack(rawString);

			Assert.IsTrue ( ssa.GetAccount().HasCharacter == true );
		}

	}
}


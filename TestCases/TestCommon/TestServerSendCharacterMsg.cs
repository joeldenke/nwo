using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

using NUnit.Framework;
using Common;


namespace TestCommon
{
	[TestFixture()]
	public class TestServerSendCharacterMsg
	{
		Character c;
		MessageManager mm;
		MemoryStream ms;
		BinaryFormatter bf;
		
		[SetUp()]
		public void InitCharacter()
		{
			c = new Character("Pelle");
			mm = new MessageManager();
			ms = new MemoryStream();
			bf = new BinaryFormatter();
			
		}
		
		//
		// TEST PACK
		//
		[Test()]
		public void ShouldCorrectlyPackCharacterMsg()
		{
			ServerSendCharacterMsg ssc = new ServerSendCharacterMsg(c);
			bf.Serialize(ms, c);
			string data = System.Convert.ToBase64String( ms.ToArray () );

			string expected = "SSC" + data + "\x001B";
			
			Assert.IsTrue(mm.Pack (ssc) == expected);
		}
		
		// 
		// TEST UNPACK
		//
		[Test()]
		public void ShouldUnpackCorrectName()
		{
			bf.Serialize(ms, c);
			string data = System.Convert.ToBase64String( ms.ToArray () );
			
			string rawString = "SSC" + data + "\x001B";
			
			ServerSendCharacterMsg ssc = (ServerSendCharacterMsg) mm.Unpack(rawString);

			Assert.IsTrue ( ssc.GetCharacter().Name == "Pelle" );
		}

	}
}


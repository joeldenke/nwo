using System;
using NUnit.Framework;
using Common;

namespace TestCommon
{
	[TestFixture()]
	public class TestClientCreateAccountRequestMsg
	{
		MessageManager mm = new MessageManager();
		/*
		 * TEST PACK
		 */
		
		[Test()]
		public void shouldCorrectlyPackLoginRequestMessage ()
		{
			ClientCreateAccountRequestMsg cca = 
				new ClientCreateAccountRequestMsg("abc@hotmail.com", "basketboll123");
			
			Assert.IsTrue (mm.Pack (cca) == "CCAabc@hotmail.com\x0002basketboll123\x001B");
		}
		
		/*
		 * TEST UNPACK
		 */
		
		[Test()]
		public void shouldCorrectlyUnpackEmailAddr ()
		{
			ClientCreateAccountRequestMsg cca = 
				(ClientCreateAccountRequestMsg) mm.Unpack("CCAabc@hotmail.com\x0002basketboll123\x001B");

			Assert.IsTrue(cca.GetEmail() == "abc@hotmail.com");
		}
		
		[Test()]
		public void shouldCorrectlyUnpackPassword ()
		{
			ClientCreateAccountRequestMsg cca = 
				(ClientCreateAccountRequestMsg) mm.Unpack ("CCAabc@hotmail.com\x0002basketboll123\x001B");

			Assert.IsTrue(cca.GetPassword() == "basketboll123");
		}
		
		
		/*
		 * TEST GETTYPE
		 */
		
		[Test()]
		public void shouldReturnCcaType ()
		{
			Message m = mm.Unpack("CCAabc@hotmail.com\x0002basketboll123\x001B");

			Assert.IsTrue (m.GetMsgType() == Message.Type.CCA);
		}
	}
}


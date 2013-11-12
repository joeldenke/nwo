using System;
using NUnit.Framework;
using Common;

namespace TestCommon
{
	[TestFixture()]
	public class TestClientLoginRequestMsg
	{
		MessageManager mm = new MessageManager();

		/*
		 * TEST PACK
		 */

		[Test()]
		public void shouldCorrectlyPackLoginRequestMessage ()
		{
			ClientLoginRequestMsg clr = new ClientLoginRequestMsg("abc@hotmail.com", "basketboll123");

			Assert.IsTrue (mm.Pack (clr) == "CLRabc@hotmail.com\x0002basketboll123\x001B");
		}

		/*
		 * TEST UNPACK
		 */

		[Test()]
		public void shouldCorrectlyUnpackEmailAddr ()
		{
			ClientLoginRequestMsg clr = (ClientLoginRequestMsg) mm.Unpack("CLRabc@hotmail.com\x0002basketboll123\x001B");

			Assert.IsTrue(clr.GetEmail() == "abc@hotmail.com");
		}

		[Test()]
		public void shouldCorrectlyUnpackPassword ()
		{
			ClientLoginRequestMsg clr = (ClientLoginRequestMsg) mm.Unpack("CLRabc@hotmail.com\x0002basketboll123\x001B");

			Assert.IsTrue(clr.GetPassword() == "basketboll123");
		}


		/*
		 * TEST GETTYPE
		 */

		[Test()]
		public void shouldReturnClrType ()
		{
			Message m = mm.Unpack("CLRabc@hotmail.com\x0002basketboll123\x001B");
			Assert.IsTrue (m.GetMsgType() == Message.Type.CLR);
		}
	}
}


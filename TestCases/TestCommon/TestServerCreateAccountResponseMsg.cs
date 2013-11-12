using System;
using NUnit.Framework;
using Common;

namespace TestCommon
{
	[TestFixture()]
	public class TestServerCreateAccountResponseMsg
	{

		MessageManager mm = new MessageManager();

		/*
		 * TEST PACK
		 */
		
		[Test()]
		public void shouldCorrectlyPackLoginResponseMessage ()
		{
			ServerCreateAccountResponseMsg sca = new ServerCreateAccountResponseMsg(false, "Wrong password");

			Assert.IsTrue (mm.Pack(sca) == "SCAFalse\x0002Wrong password\x001B");
		}
		
		
		[Test()]
		public void shouldCorrectlyPackEmptyMessage ()
		{
			ServerCreateAccountResponseMsg sca = new ServerCreateAccountResponseMsg(false, "");
			Assert.IsTrue (mm.Pack(sca) == "SCAFalse\x0002\x001B");
		}
		
		/*
		 * TEST UNPACK
		 */
		
		[Test()]
		public void shouldCorrectlyUnpackAcceptBool ()
		{
			ServerCreateAccountResponseMsg sca = (ServerCreateAccountResponseMsg) mm.Unpack("SCATrue\x0002Success\x001B");

			Assert.IsTrue(sca.IsAccepted());
		}
		
		[Test()]
		public void shouldCorrectlyUnpackText ()
		{
			ServerCreateAccountResponseMsg sca = (ServerCreateAccountResponseMsg) mm.Unpack("SCATrue\x0002Success\x001B");

			Assert.IsTrue(sca.GetText() == "Success");
		}
		
		[Test()]
		public void shouldCorrectlyUnpackEmptyText ()
		{
			ServerCreateAccountResponseMsg sca = (ServerCreateAccountResponseMsg) mm.Unpack("SCATrue\x0002\x001B");

			Assert.IsTrue(sca.GetText() == "");
		}
		
		
		/*
		 * TEST GETTYPE
		 */
		
		[Test()]
		public void shouldReturnScaType ()
		{
			Message m = mm.Unpack("SCATrue\x0002Hej\x001B");
			Assert.IsTrue (m.GetMsgType() == Message.Type.SCA);
		}

		[Test()]
		public void shouldReturnScaTypeWhenEmptyText ()
		{
			Message m = mm.Unpack("SCATrue\x0002Hej\x001B");
			Assert.IsTrue (m.GetMsgType() == Message.Type.SCA);
		}
	}
}


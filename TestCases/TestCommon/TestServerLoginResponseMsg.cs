using System;
using NUnit.Framework;
using Common;

namespace TestCommon
{
	[TestFixture()]
	public class TestServerLoginResponseMsg
	{
		MessageManager mm = new MessageManager();

		/*
		 * TEST PACK
		 */
		
		[Test()]
		public void shouldCorrectlyPackLoginResponseMessage ()
		{
			ServerLoginResponseMsg slr = new ServerLoginResponseMsg(false, "Wrong password");
			
			Assert.IsTrue (mm.Pack (slr) == "SLRFalse\x0002Wrong password\x001B");
		}

		
		[Test()]
		public void shouldCorrectlyPackEmptyMessage ()
		{
			ServerLoginResponseMsg slr = new ServerLoginResponseMsg(false, "");

			Assert.IsTrue (mm.Pack (slr) == "SLRFalse\x0002\x001B");
		}
		
		/*
		 * TEST UNPACK
		 */
		
		[Test()]
		public void shouldCorrectlyUnpackAcceptBool ()
		{
			ServerLoginResponseMsg slr = (ServerLoginResponseMsg) mm.Unpack("SLRTrue\x0002Success\x001Bs");
			
			Assert.IsTrue(slr.IsAccepted());
		}
		
		[Test()]
		public void shouldCorrectlyUnpackText ()
		{
			ServerLoginResponseMsg slr = (ServerLoginResponseMsg) mm.Unpack("SLRTrue\x0002Success\x001Bs");

			Assert.IsTrue(slr.GetText() == "Success");
		}
		
		[Test()]
		public void shouldCorrectlyUnpackEmptyText ()
		{
			ServerLoginResponseMsg slr = (ServerLoginResponseMsg) mm.Unpack("SLRTrue\x0002\x001B");

			Assert.IsTrue(slr.GetText() == "");
		}

		
		/*
		 * TEST GETTYPE
		 */
		
		[Test()]
		public void shouldReturnSlrType ()
		{
			Message m = mm.Unpack("SLRTrue\x0002Hej\x001B");

			Assert.IsTrue (m.GetMsgType() == Message.Type.SLR);
		}
		
		[Test()]
		public void shouldReturnSlrTypeWhenEmptyText ()
		{
			Message m = mm.Unpack("SLRTrue\x0002\x001B");

			Assert.IsTrue (m.GetMsgType() == Message.Type.SLR);
		}

	}
}


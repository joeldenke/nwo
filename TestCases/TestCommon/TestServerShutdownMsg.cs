using NUnit.Framework;
using System;
using Common;

namespace TestCommon
{
	[TestFixture()]
	public class TestServerShutdownMsg
	{
		MessageManager mm = new MessageManager();

		[Test()]
		public void shouldCorrectlyPackTestServerShutdownMsg()
		{
			ServerShutdownMsg sdm = new ServerShutdownMsg("Maintainance");
						
			Assert.IsTrue (mm.Pack(sdm) == "SSDMaintainance\x001B");
		}
		
		[Test()]
		public void shouldCorrectlyUnpackServerShutdownMsg()
		{
			ServerShutdownMsg cdm = (ServerShutdownMsg) mm.Unpack ("SSD   \x001B");
						
			Assert.IsTrue(cdm.GetText() == "   ");
		}
		
		[Test()]
		public void shouldReturnSsdType()
		{
			Message m = mm.Unpack("SSDBabian\x001B");
			Assert.IsTrue (m.GetMsgType() == Message.Type.SSD);
		}
	}
}


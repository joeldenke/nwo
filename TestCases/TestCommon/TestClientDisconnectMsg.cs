using NUnit.Framework;
using System;
using Common;

namespace TestCommon
{
	[TestFixture()]
	public class TestClientDisconnectMsg
	{
		MessageManager mm = new MessageManager();
		[Test()]
		public void shouldCorrectlyPackClientDisconnectMsg()
		{
			ClientDisconnectMsg cdm = new ClientDisconnectMsg("DC");

			Assert.IsTrue (mm.Pack(cdm) == "CDCDC\x001B");
		}

		[Test()]
		public void shouldCorrectlyUnpackClientDisconnectMsg()
		{
			ClientDisconnectMsg cdm = (ClientDisconnectMsg) mm.Unpack("CDC \x001B");

			Assert.IsTrue(cdm.GetText() == " ");
		}

		[Test()]
		public void shouldReturnCdcType ()
		{
			Message m = mm.Unpack("CDCHFHFIHY/YT/(YWT\x001B");

			Assert.IsTrue (m.GetMsgType() == Message.Type.CDC);
		}
	}
}


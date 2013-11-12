using NUnit.Framework;
using System;
using Common;

namespace TestCommon
{
	[TestFixture()]
	public class TestServerKickMsg
	{
		MessageManager mm = new MessageManager();

		[Test()]
		public void shouldCorrectlyPackServerKickMsg()
		{
			ServerKickMsg skm = new ServerKickMsg("you are a douche!#¤");

			Assert.IsTrue (mm.Pack (skm) == "SKKyou are a douche!#¤\x001B");
		}
		
		[Test()]
		public void shouldCorrectlyUnpackServerKickMsg()
		{
			ServerKickMsg skm = (ServerKickMsg) mm.Unpack("SKK   \x001B");

			Assert.IsTrue(skm.GetText() == "   ");
		}
		
		[Test()]
		public void shouldReturnSkkType ()
		{
			ServerKickMsg skm = (ServerKickMsg) mm.Unpack("SKK   \x001B");
			Assert.IsTrue (skm.GetMsgType() == Message.Type.SKK);
		}
	}
}


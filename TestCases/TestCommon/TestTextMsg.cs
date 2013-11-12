using NUnit.Framework;
using System;
using Common;

namespace TestCommon
{

	[TestFixture()]
	public class TestTextMsg
	{
		MessageManager mm = new MessageManager();

		[Test()]
		public void shouldCorrectlyPackTextMessage ()
		{
			TextMsg tm = new TextMsg("HEJ");
			Assert.IsTrue (mm.Pack (tm) == "TXTHEJ\x001B");
		}

		[Test()]
		public void shouldCorrectlyPackEmptyTextMessage ()
		{
			TextMsg tm = new TextMsg("");
			Assert.IsTrue (mm.Pack(tm) == "TXT\x001B");
		}

		[Test()]
		public void shouldCorrectlyUnpackTextMessage ()
		{
			TextMsg tm = (TextMsg) mm.Unpack("TXTHEJ\x001B");
			Assert.IsTrue(tm.GetText() == "HEJ");
		}

		[Test()]
		public void shouldCorrectlyUnpackEmptyTextMessage()
		{
			TextMsg tm = (TextMsg) mm.Unpack("TXT\x001B");

			Assert.IsTrue(tm.GetText() == "");
		}

		[Test()]
		public void shouldReturnTxtType ()
		{
			Message m = mm.Unpack("TXT\x001BHEJ");
			Assert.IsTrue (m.GetMsgType() == Message.Type.TXT);
		}

		[Test()]
		public void shouldReturnTxtTypeWhenEmptyMessage ()
		{
			Message m = mm.Unpack("TXT\x001B");
			Assert.IsTrue (m.GetMsgType() == Message.Type.TXT);
		}
	}
}


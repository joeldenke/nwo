using System;
using NUnit.Framework;
using Common;

namespace TestCommon
{
	[TestFixture()]
	public class TestClientUseSkillRequestMsg
	{
		MessageManager mm = new MessageManager();

		/*
		 * Test Pack
		 */

		[Test()]
		public void shouldCorrectlyPackUseSkillRequestMsg ()
		{
			ClientUseSkillRequestMsg cus = new ClientUseSkillRequestMsg(SkillType.HUNTING);

			Assert.IsTrue (mm.Pack(cus) == "CUSHUNTING\x001B");
		}

		[Test()]
		public void shouldCorrectlyUnPackUseSkillRequestMsg ()
		{
			ClientUseSkillRequestMsg cus = (ClientUseSkillRequestMsg)mm.Unpack ("CUSHUNTING\x001B");

			Assert.IsTrue (cus.GetSkillType () == SkillType.HUNTING);
			Assert.IsTrue (cus.GetMsgType() == Message.Type.CUS);
		}
	}
}


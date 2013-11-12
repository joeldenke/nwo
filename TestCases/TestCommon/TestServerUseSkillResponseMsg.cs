using System;
using NUnit.Framework;
using Common;

namespace TestCommon
{
	[TestFixture()]
	public class TestServerUseSkillResponseMsg
	{
		MessageManager mm = new MessageManager();

		[Test()]
		public void shouldCorrectlyPackUseSkillResponseMsg ()
		{
			ServerUseSkillResponseMsg sus = new ServerUseSkillResponseMsg (SkillType.HUNTING, true, "");

			Assert.IsTrue(mm.Pack(sus) == "SUSHUNTING\x0002True\x0002\x001B");

			ServerUseSkillResponseMsg sus2 = new ServerUseSkillResponseMsg 
				(SkillType.HUNTING, false, "not available");

			Assert.IsTrue(mm.Pack (sus2) == "SUSHUNTING\x0002False\x0002not available\x001B");
		}

		[Test()]
		public void shouldCorrectlyUnPackUseSkillResponseMsg ()
		{

			ServerUseSkillResponseMsg sus = (ServerUseSkillResponseMsg) 
				mm.Unpack("SUSFARMING\x0002True\x0002\x001B");

			Assert.IsTrue(sus.GetAccepted());
			Assert.IsTrue (sus.GetSkillType() == SkillType.FARMING);
			Assert.IsTrue(sus.GetMsgType() == Message.Type.SUS);
		}

		[Test()]
		public void shouldCorrectlyUnPackUseSkillResponseMsg2 ()
		{

			ServerUseSkillResponseMsg sus = (ServerUseSkillResponseMsg) 
				mm.Unpack("SUSFARMING\x0002True\x0002You can\x001B");

			Assert.IsTrue(sus.GetAccepted());
			Assert.IsTrue(sus.GetText() == "You can");
			Assert.IsTrue (sus.GetSkillType() == SkillType.FARMING);
			Assert.IsTrue(sus.GetMsgType() == Message.Type.SUS);
		}
	}
}


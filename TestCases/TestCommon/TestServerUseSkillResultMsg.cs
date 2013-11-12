using System;
using Common;
using NUnit.Framework;

namespace TestCommon
{
	[TestFixture()]
	public class TestServerUseSkillResultMsg
	{
		MessageManager mm = new MessageManager();

		[Test()]
		public void shouldCorrectlyPackUseSkillResultMsg ()
		{
			ServerUseSkillResultMsg sur = new ServerUseSkillResultMsg
				(SkillType.CRAFTING, "You crafted an axe!");

			Assert.IsTrue(mm.Pack(sur) == "SURCRAFTING\x0002You crafted an axe!\x001B");
		}

		[Test()]
		public void shouldCorrectlyUnPackUseSkillResultMsg ()
		{
			ServerUseSkillResultMsg sur = (ServerUseSkillResultMsg)
				mm.Unpack ("SURMAGIC\x0002You got a magic rock!\x001B");

			Assert.IsTrue (sur.GetSkillType() == SkillType.MAGIC);
			Assert.IsTrue (sur.GetMsgType() == Message.Type.SUR);
			Assert.IsTrue (sur.GetResultDescription() == "You got a magic rock!");
		}
	}
}


using System;
using NUnit.Framework;
using Server.Dba;
using Server.Model;
using Common;

namespace TestServer
{
	[TestFixture()]
	public class TestServerCharacter
	{
		public DbaMongo db = new DbaMongo("mongodb://localhost/nwo");
		public DbaAccountManager ma;

		[Test()]
		public void ShouldCreateNewCharacterOnAccount()
		{
			ma = db.GetAccountManager ();
			try{
				ma.RegisterNew ("joel@joel.com", "testing");
			}catch(NWOException){}

			ServerAccount account = db.GetAccount("joel@joel.com", "testing").GetIdentity();

			account.Character = new ServerCharacter("test", new Position (0, 0));
			Assert.IsNotNull(account);
			Assert.IsTrue (ma.Modify(account));
			Assert.IsTrue (ma.Delete(account));
		}

		[Test()]
		public void ShouldInsertAttribute()
		{
			ma = db.GetAccountManager ();
			try{
				ma.RegisterNew ("joel@joel.com", "testing");
			}catch(NWOException){}

			ServerAccount account = db.GetAccount("joel@joel.com", "testing").GetIdentity();
			Assert.IsNotNull(account);

			account.Character = new ServerCharacter("test", new Position (0, 0));
			account.Character.AttributeSet = new AttributeSet();
			account.Character.AttributeSet.Attributes[(int)AttributeType.AGILITY] = 100;

			Assert.IsTrue (ma.Modify(account));
			Assert.IsTrue (ma.Delete(account));
		}

		[Test()]
		public void ShouldUpdateAllSkillsAndAttributesProperly()
		{
			ma = db.GetAccountManager ();
			try{
				ma.RegisterNew ("joel@joel.com", "testing");
			}catch(NWOException){}

			ServerAccount account = db.GetAccount("joel@joel.com", "testing").GetIdentity();
			Assert.IsNotNull(account);

			account.Character = new ServerCharacter("test", new Position (0, 0));
			account.Character.AttributeSet = new AttributeSet();
			account.Character.SkillSet = new SkillSet();
			account.Character.EnhanceSkill(SkillType.HUNTING, 99);
			account.Character.EnhanceSkill(SkillType.MINING, 99);
			account.Character.EnhanceSkill(SkillType.MELEE, 99);
			account.Character.EnhanceSkill(SkillType.BUILDING, 99);
			account.Character.EnhanceSkill(SkillType.WOODCUTTING, 99);
			account.Character.EnhanceSkill(SkillType.FARMING, 99);
			account.Character.EnhanceSkill(SkillType.MAGIC, 99);
			account.Character.EnhanceSkill(SkillType.RANGED, 99);
			account.Character.EnhanceSkill(SkillType.STEALTH, 99);
			account.Character.EnhanceSkill(SkillType.DIPLOMACY, 99);
			account.Character.EnhanceSkill(SkillType.PROPHECY, 99);
			account.Character.EnhanceSkill(SkillType.CRAFTING, 99);
			account.Character.EnhanceAttribute(AttributeType.STRENGTH, 19);
			account.Character.EnhanceAttribute(AttributeType.ENDURANCE, 19);
			account.Character.EnhanceAttribute(AttributeType.AGILITY, 19);
			account.Character.EnhanceAttribute(AttributeType.INTELLIGENCE, 19);
			account.Character.EnhanceAttribute(AttributeType.PERCEPTION, 19);
			account.Character.EnhanceAttribute(AttributeType.WILLPOWER, 19);

			Assert.AreEqual(35, account.Character.GetCharacter().GetAttributeTotalValue(AttributeType.STRENGTH));
			Assert.AreEqual(55, account.Character.GetCharacter().GetAttributeTotalValue(AttributeType.ENDURANCE));
			Assert.AreEqual(40, account.Character.GetCharacter().GetAttributeTotalValue(AttributeType.AGILITY));
			Assert.AreEqual(45, account.Character.GetCharacter().GetAttributeTotalValue(AttributeType.INTELLIGENCE));
			Assert.AreEqual(45, account.Character.GetCharacter().GetAttributeTotalValue(AttributeType.PERCEPTION));
			Assert.AreEqual(35, account.Character.GetCharacter().GetAttributeTotalValue(AttributeType.WILLPOWER));

			Assert.AreEqual(119, account.Character.GetCharacter().GetSkillTotalValue(SkillType.HUNTING));

			Assert.IsTrue (ma.Modify(account));
			Assert.IsTrue (ma.Delete(account));
		}
	}
}


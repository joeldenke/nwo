using System;
using NUnit.Framework;
using Common;

namespace TestCommon
{
	[TestFixture]
	public class TestArea
	{
		private Position position;
		private Area area;

		[TestFixtureSetUp]
		public void setup ()
		{
			position = new Position(10, 20);
			area = new Area(position, AreaTerrainType.SWAMP, AreaEstateType.CITY);
		}

		[Test]
		public void shouldMapConstructorValuesToPublicAttributes()
		{
			Assert.IsTrue(area.Position.X == position.X);
			Assert.IsTrue(area.Position.Y == position.Y);

			Assert.IsTrue(area.TerrainType == AreaTerrainType.SWAMP);
			Assert.IsTrue(area.EstateType == AreaEstateType.CITY);
		}

		[Test]
		[Ignore]
		public void shouldRegisterGivenModifierValue()
		{
			area.SetModifier(SkillType.HUNTING, 10);
			Assert.IsTrue(area.GetModifier(SkillType.HUNTING) == 10);
		}
	}
}


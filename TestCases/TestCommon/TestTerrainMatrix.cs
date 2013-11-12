using System;
using NUnit.Framework;
using Common;

namespace TestCommon
{
	[TestFixture]
	public class TestTerrainMatrix
	{
		private AreaTerrainType[,] terrains;
		private TerrainMatrix matrix;

		[TestFixtureSetUp]
		public void setup ()
		{
			terrains = new AreaTerrainType[4, 5];
			for (int y = 0; y < 4; y++)
				for (int x = 0; x < 5; x++)
					terrains [y, x] = AreaTerrainType.PLAINS;

			matrix = new TerrainMatrix (terrains, 5, 4);
		}

		[Test]
		public void shouldHaveConsistentWidth ()
		{
			Assert.IsTrue (matrix.GetWidth () == 5);
		}

		[Test]
		public void shouldHaveConsistentHeight ()
		{
			Assert.IsTrue (matrix.GetHeight () == 4);
		}

		[Test]
		public void shouldContainCorrectAmountOfCells ()
		{
			int cellCounter = 0;
			foreach (AreaTerrainType t in matrix.GetMatrix())
				cellCounter++;

			Assert.IsTrue (cellCounter == matrix.GetWidth() * matrix.GetHeight ());
		}
	}
}


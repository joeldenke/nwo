using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

using NUnit.Framework;
using Common;

namespace TestCommon
{
	public class TestServerTerrainMatrixMsg
	{
		private TerrainMatrix matrix;
		private MessageManager mm;
		private MemoryStream ms;
		private BinaryFormatter bf;

		[TestFixtureSetUp]
		public void Setup ()
		{
			AreaTerrainType[,] terrains = new AreaTerrainType[4, 5];
			for (int x = 0; x < 4; x++)
				for (int y = 0; y < 5; y++)
					terrains [x, y] = AreaTerrainType.FOREST;

			terrains [2, 3] = AreaTerrainType.JUNGLE;

			matrix = new TerrainMatrix (terrains, 5, 4);

			mm = new MessageManager();
			ms = new MemoryStream();
			bf = new BinaryFormatter();
		}

		[Test]
		public void ShouldCorrectlyPackMsg()
		{
			bf.Serialize(ms, matrix);
			string data = System.Convert.ToBase64String( ms.ToArray () );
			string expected = "TMX" + data + mm.End;

			ServerTerrainMatrixMsg tmx = new ServerTerrainMatrixMsg(matrix);

			Assert.IsTrue(mm.Pack (tmx) == expected);
		}

		[Test]
		public void ShouldCorrectlyUnpackMsg()
		{
			bf.Serialize(ms, matrix);
			string data = System.Convert.ToBase64String( ms.ToArray () );
			string rawString = "TMX" + data + mm.End;
			
			ServerTerrainMatrixMsg tmx = (ServerTerrainMatrixMsg) mm.Unpack(rawString);

			Assert.IsTrue ( tmx.GetTerrainMatrix ().GetMatrix()[1, 3] == AreaTerrainType.FOREST );
			Assert.IsTrue ( tmx.GetTerrainMatrix ().GetMatrix()[2, 3] == AreaTerrainType.JUNGLE );
		}
	}
}


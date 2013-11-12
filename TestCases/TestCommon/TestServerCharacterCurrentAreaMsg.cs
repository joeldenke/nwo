using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

using NUnit.Framework;
using Common;

namespace TestCommon
{
	public class TestServerCharacterCurrentAreaMsg
	{
		private Area currentArea;
		private MessageManager mm;
		private MemoryStream ms;
		private BinaryFormatter bf;

		[TestFixtureSetUp]
		public void Setup ()
		{
			currentArea = new Area (new Position (2, 4), AreaTerrainType.JUNGLE, AreaEstateType.RUINS);
			mm = new MessageManager();
			ms = new MemoryStream();
			bf = new BinaryFormatter();
		}

		[Test]
		public void ShouldCorrectlyPackMsg()
		{
			bf.Serialize(ms, currentArea);
			string data = System.Convert.ToBase64String( ms.ToArray () );
			string expected = "CUA" + data + mm.End;

			ServerCharacterCurrentAreaMsg cua = new ServerCharacterCurrentAreaMsg(currentArea);

			Assert.IsTrue(mm.Pack (cua) == expected);
		}

		[Test]
		public void ShouldCorrectlyUnpackMsg()
		{
			bf.Serialize(ms, currentArea);
			string data = System.Convert.ToBase64String( ms.ToArray () );
			string rawString = "CUA" + data + mm.End;
			
			ServerCharacterCurrentAreaMsg cua = (ServerCharacterCurrentAreaMsg) mm.Unpack(rawString);

			Assert.IsTrue ( cua.GetCurrentArea().TerrainType == AreaTerrainType.JUNGLE );
			Assert.IsTrue ( cua.GetCurrentArea().EstateType == AreaEstateType.RUINS );
		}
	}
}


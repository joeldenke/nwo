using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

using NUnit.Framework;
using Common;

namespace TestCommon
{
	public class TestClientMoveCharacterMsg
	{
		private Position position;
		private MessageManager mm;

		[TestFixtureSetUp]
		public void Setup ()
		{
			mm = new MessageManager();
		}

		[Test]
		public void ShouldCorrectlyPackMsg()
		{
			position = new Position(2, 3);
			string expected = "CMC" + position.X + mm.Sep + position.Y + mm.End;

			ClientMoveCharacterMsg cmc = new ClientMoveCharacterMsg(position);

			Assert.IsTrue(mm.Pack (cmc) == expected);
		}

		[Test]
		public void ShouldCorrectlyUnpackMsg()
		{
			position = new Position(1, 5);
			string rawString = "CMC" + position.X + mm.Sep + position.Y + mm.End;
			
			ClientMoveCharacterMsg cmc = (ClientMoveCharacterMsg) mm.Unpack(rawString);

			Assert.IsTrue ( cmc.GetPosition ().X == 1);
			Assert.IsTrue ( cmc.GetPosition ().Y == 5);
		}
	}
}


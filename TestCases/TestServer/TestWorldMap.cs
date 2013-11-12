using System;
using Server.Model;
using Common;
using NUnit.Framework;

namespace TestServer
{
	[TestFixture]
	public class TestWorldMap
	{
		private WorldMap worldMap;
		private ServerCharacter serverCharacter;
		private Character character;
		private Position defaultCharacterPosition;
		private Exception startupException;

		[TestFixtureSetUp]
		public void setup ()
		{
			try {
				worldMap = new WorldMap (3, 4);
				defaultCharacterPosition = new Position (0, 0);
				serverCharacter = new ServerCharacter ("Traveler", defaultCharacterPosition);

			} catch (Exception e) {
				startupException = e;
			}
		}

		[Test]
		public void shouldHaveCreatedWorldMap ()
		{
			if (startupException != null)
				throw startupException;

			Assert.IsTrue (worldMap != null);
		}

		[Test]
		public void shouldMoveCharacterFromAreaToArea ()
		{
			Position destinationCharacterPosition = new Position (2, 1);

			worldMap.MoveCharacterToPosition (serverCharacter, destinationCharacterPosition);

			Assert.IsTrue (serverCharacter.Position.X == destinationCharacterPosition.X);
			Assert.IsTrue (serverCharacter.Position.Y == destinationCharacterPosition.Y);

			worldMap.MoveCharacterToPosition (serverCharacter, defaultCharacterPosition);

			Assert.IsTrue (serverCharacter.Position.X == defaultCharacterPosition.X);
			Assert.IsTrue (serverCharacter.Position.Y == defaultCharacterPosition.Y);
		}
	}
}


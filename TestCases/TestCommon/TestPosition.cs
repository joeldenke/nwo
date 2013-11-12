using System;
using NUnit.Framework;
using Common;

namespace TestCommon
{
	[TestFixture]
	public class TestPosition
	{
		[Test]
		public void shouldStoreXAndYValues ()
		{
			Position position = new Position (123, 456);
			Assert.IsTrue (position.X == 123);
			Assert.IsTrue (position.Y == 456);
		}
	}
}


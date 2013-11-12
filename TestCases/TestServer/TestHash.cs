using System;
using Common;
using NUnit.Framework;

namespace TestServer
{
	[TestFixture()]
	public class TestHash
	{
		[Test()]
		public void TestHashing()
		{
			Console.WriteLine (Crypt.GenHash("test"));
			Console.WriteLine (Crypt.GenHash("testing"));
		}
	}
}


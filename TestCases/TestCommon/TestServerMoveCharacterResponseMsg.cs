using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

using NUnit.Framework;
using Common;

namespace TestCommon
{
	public class TestServerMoveCharacterResponseMsg
	{
		private MessageManager mm;

		[TestFixtureSetUp]
		public void Setup ()
		{
			mm = new MessageManager();
		}

		[Test]
		public void ShouldCorrectlyPackMsg()
		{
			bool isAccepted = false;
			string text = "Some message.";
			string expected = "SMC" + isAccepted + mm.Sep + text + mm.End;

			ServerMoveCharacterResponseMsg smc = new ServerMoveCharacterResponseMsg(isAccepted, text);

			Assert.IsTrue(mm.Pack (smc) == expected);
		}

		[Test]
		public void ShouldCorrectlyUnpackMsg()
		{
			bool isAccepted = false;
			string text = "Some message.";
			string rawString = "SMC" + isAccepted + mm.Sep + text + mm.End;
			
			ServerMoveCharacterResponseMsg smc = (ServerMoveCharacterResponseMsg) mm.Unpack(rawString);

			Assert.IsTrue ( smc.IsAccepted() == isAccepted );
			Assert.IsTrue ( smc.GetText() == text );
		}
	}
}


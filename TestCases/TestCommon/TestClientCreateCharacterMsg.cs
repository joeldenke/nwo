using System;
using Common;
using NUnit.Framework;

namespace TestCommon
{
	[TestFixture()]
	public class TestClientCreateCharacterMsg
	{
		MessageManager mm = new MessageManager();
		 /*
		 * TEST PACK
		 */
		 
		 [Test()]
		 public void shouldCorrectlyPackCharacterRequestMessage ()
		 {
			ClientCreateCharacterRequestMsg ccc = 
				new ClientCreateCharacterRequestMsg("Pelle");
			
			Assert.IsTrue (mm.Pack (ccc) == "CCCPelle\x001B");
		 }
		 
		 /*
		 * TEST UNPACK
		 */
		 
		 [Test()]
		 public void shouldCorrectlyUnpackName ()
		 {
			ClientCreateCharacterRequestMsg ccc = 
				(ClientCreateCharacterRequestMsg) mm.Unpack("CCCPelle\x001B");
			
			Assert.IsTrue(ccc.GetName() == "Pelle");
		 }
		 
		 
		 /*
		 * TEST GETTYPE
		 */
		 
		 [Test()]
		 public void shouldReturnCccType ()
		 {
			Message m = mm.Unpack("CCCPelle\x001B");
			
			Assert.IsTrue (m.GetMsgType() == Message.Type.CCC);
		 }
	}
}


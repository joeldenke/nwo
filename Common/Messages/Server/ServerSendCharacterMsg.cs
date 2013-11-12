using System;

namespace Common
{
	public class ServerSendCharacterMsg : Message
	{

		private Character c;

		public ServerSendCharacterMsg (Character c)
		{
			this.c = c;
		}

		public Character GetCharacter ()
		{
			return c;
		}

		public override Type GetMsgType()
		{
			return Message.Type.SSC;
		}
	}
}


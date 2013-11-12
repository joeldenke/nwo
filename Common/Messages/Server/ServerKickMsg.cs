using System;

namespace Common
{
	public class ServerKickMsg : Message
	{
		private string text;

		public ServerKickMsg(string text)
		{
            this.text = text;
		}

		public string GetText ()
		{
			return text;
		}

        public override Type GetMsgType()
        {
            return Message.Type.SKK;
        }
	}
}


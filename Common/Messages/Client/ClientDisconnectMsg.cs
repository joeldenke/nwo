using System;

namespace Common
{
	public class ClientDisconnectMsg : Message
	{
		private string text;

		public ClientDisconnectMsg (string text)
		{
            this.text = text;
		}

		public string GetText ()
		{
			return text;
		}

        public override Type GetMsgType()
        {
            return Message.Type.CDC;
        }
	}
}


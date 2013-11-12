using System;

namespace Common
{
	public class ServerShutdownMsg : Message
	{
		private string text;

		public ServerShutdownMsg (string text)
		{
            this.text = text;
		}

		public string GetText ()
		{
			return text;
		}

        public override Type GetMsgType()
        {
            return Message.Type.SSD;
        }
	}
}


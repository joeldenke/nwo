using System;

namespace Common
{
	public class ServerLoginResponseMsg : Message
	{
		private bool accepted;
		private string text;

		public ServerLoginResponseMsg (bool accepted, string text)
		{
            this.accepted = accepted;
            this.text = text;
		}

		public bool IsAccepted ()
		{
			return accepted;
		}

		public string GetText ()
		{
			return text;
		}

        public override Type GetMsgType()
        {
            return Message.Type.SLR;
        }
	}
}


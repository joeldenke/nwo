using System;

namespace Common
{
	public class TextMsg : Message
	{
		private string text;
		public TextMsg (string text)
		{
            this.text = text;
		}

		public string GetText ()
		{
			return text;
		}

        public override Type GetMsgType()
        {
            return Message.Type.TXT;
        }
	}
}


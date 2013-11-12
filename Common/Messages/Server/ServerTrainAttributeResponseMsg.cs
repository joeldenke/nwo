using System;

namespace Common
{
    public class ServerTrainAttributeResponseMsg : Message
    {
        private AttributeType type;
        private bool accepted;
		private string text;

        public ServerTrainAttributeResponseMsg(AttributeType type, bool accepted, string text)
        {
            this.type = type;
            this.accepted = accepted;
			this.text = text;
        }

        public AttributeType GetAttributeType()
        {
            return type;
        }

        public bool GetAccepted()
        {
            return accepted;
        }

		public string GetText()
		{
			return text;
		}

        public override Type GetMsgType()
        {
            return Message.Type.STA;
        }
    }
}

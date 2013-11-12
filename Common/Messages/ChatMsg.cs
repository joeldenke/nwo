using System;

namespace Common
{
    public class ChatMsg : Message
    {
		public enum ChatType
		{
			PUBLIC,
			PRIVATE
		}

		private ChatType chatType;
        private string name;
        private string text;

		public ChatMsg(string name, string text, ChatType chatType)
        {
			this.name = name;
            this.text = text;
			this.chatType = chatType;
        }

        public string GetName()
        {
            return name;
        }

        public string GetText()
        {
            return text;
        }

		public ChatType GetChatType ()
		{
			return chatType;
		}

        public override Type GetMsgType()
        {
            return Message.Type.CHT;
        }
    }
}

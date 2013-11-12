using System;

namespace Common
{
	public class ClientCreateAccountRequestMsg : Message
	{
		private string email;
		private string password;

		public ClientCreateAccountRequestMsg (string email, string password)
		{
            this.email = email;
            this.password = password;
		}

		public string GetEmail ()
		{
			return email;
		}

		public string GetPassword ()
		{
			return password;
		}

        public override Type GetMsgType()
        {
            return Message.Type.CCA;
        }
	}
}


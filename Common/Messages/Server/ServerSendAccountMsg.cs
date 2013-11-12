using System;

namespace Common
{
	public class ServerSendAccountMsg : Message
	{
		private Account a;

		public ServerSendAccountMsg (Account a)
		{
			this.a = a;
		}

		public Account GetAccount()
		{
			return a;
		}

		public override Type GetMsgType()
		{
			return Message.Type.SSA;
		}
	}
}


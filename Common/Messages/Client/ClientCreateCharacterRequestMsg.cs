using System;

namespace Common
{
	public class ClientCreateCharacterRequestMsg : Message
	{
		private string name;

		public ClientCreateCharacterRequestMsg (string name)
		{
			this.name = name;
		}

		public string GetName ()
		{
			return name;
		}

		public override Type GetMsgType()
		{
			return Message.Type.CCC;
		}
	}
}


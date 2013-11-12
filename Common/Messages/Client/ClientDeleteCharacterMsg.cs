using System;

namespace Common
{
    public class ClientDeleteCharacterMsg : Message
    {
        public override Type GetMsgType()
        {
            return Message.Type.CDL;
        }
    }
}

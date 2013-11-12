using System;

namespace Common
{
    public class ServerTrainAttributeResultMsg : Message
    {
        AttributeType type;
        int result;

        public ServerTrainAttributeResultMsg(AttributeType type, int result)
        {
            this.type = type;
            this.result = result;
        }

        public AttributeType GetAttributeType()
        {
            return type;
        }

        public int GetResult()
        {
            return result;
        }

        public override Type GetMsgType()
        {
            return Message.Type.STR;
        }
    }
}

using System;

namespace Common
{
    public class ClientTrainAttributeRequestMsg : Message
    {
        AttributeType type;

        public ClientTrainAttributeRequestMsg(AttributeType type)
        {
            this.type = type;
        }

        public AttributeType GetAttributeType()
        {
            return type;
        }

        public override Type GetMsgType()
        {
            return Message.Type.CTA;
        }
    }
}

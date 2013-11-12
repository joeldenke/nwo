using System;

namespace Common
{
   
	public class WrongFormatInMessageException: NWOException
	{
		public WrongFormatInMessageException() {}
    	public WrongFormatInMessageException(string message) {}
		public WrongFormatInMessageException (string message, System.Exception inner){}

		// Constructor needed for serialization 
    	//when exception propagates from a remoting server to the client.
    	protected WrongFormatInMessageException(System.Runtime.Serialization.SerializationInfo info,
        System.Runtime.Serialization.StreamingContext context) {}
	}

	public abstract class Message
	{

        /// <summary>
        /// Add messages here!
        /// 
        /// !!! IMPORTANT !!! 
        /// Add in alphabetic order and update the file "Network Protocol.txt"
        /// !!!
        /// </summary>
        public enum Type
        {
            // Messages sent from client

            CAI, // ClientAreaInfoRequestMsg
            CCA, // ClientCreateAccountRequestMsg
            CCC, // ClientCreateCharacterRequestMsg
            CDC, // ClientDisconnectMsg
            CDL, // ClientDeleteCharacterMsg
            CLR, // ClientLoginRequestMsg
            CMC, // ClientMoveCharacterMsg
            CTA, // ClientTrainAttributeRequestMsg
            CUS, // ClientUseSkillRequestMsg

            // Messages sent from server

            SAI, // ServerAreaMsg
            SAR, // ServerCharacterCurrentAreaMsg
            SCA, // ServerCreateAccountResponseMsg
            SCC, // ServerCreateCharacterResponseMsg
            SKK, // ServerKickMsg
            SLR, // ServerLoginResponseMsg
            SMC, // ServerMoveCharacterResponseMsg
            SSA, // ServerSendAccountMsg
            SSC, // ServerSendCharacterMsg
            SSD, // ServerShutdownMsg
            STA, // ServerTrainAttributeResponseMsg
            STM, // ServerTerrainMatrixMsg
            STR, // ServerTrainAttributeResultMsg
            SUR, // ServerUseSkillResultMsg
            SUS, // ServerUseSkillResponseMsg

            // Messages sent from both client and server

            CHT, // ChatMsg
            TXT  // TextMsg
        };

        public abstract Type GetMsgType();
	}
}


using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace Common
{
    public class MessageManager
    {
		public string End = "\x001B";      // Endape character, terminates a message.
		public string Sep = "\x0002";      // Start-of-text character - Separator. Separates sections in messages. 

        /// <summary>
        /// Pack a message down to a string to be sent over a network steram
        /// </summary>
        /// <param name="msg"></param>
        /// <returns></returns>
        public string Pack(Message msg)
        {
			// For serialization
			MemoryStream ms = new MemoryStream();
			BinaryFormatter bf = new BinaryFormatter();

            switch (msg.GetMsgType())
            {
                // TextMsg
                case Message.Type.TXT:
                {
                    TextMsg txt = (TextMsg)msg;
                    return "TXT" + txt.GetText() + End;
                }

                // ClientDisconnectMsg
                case Message.Type.CDC:
                {
                    ClientDisconnectMsg cdc = (ClientDisconnectMsg)msg;
                    return "CDC" + cdc.GetText() + End;
                }

                // ServerKickMsg
                case Message.Type.SKK:
                {
                    ServerKickMsg skk = (ServerKickMsg)msg;
                    return "SKK" + skk.GetText() + End;
                }

                // ServerShutdownMsg
                case Message.Type.SSD:
                {
                    ServerShutdownMsg ssd = (ServerShutdownMsg)msg;
                    return "SSD" + ssd.GetText() + End;
                }

                // ClientLoginRequestMsg
                case Message.Type.CLR:
                {
                    ClientLoginRequestMsg clr = (ClientLoginRequestMsg)msg;
                    return "CLR" + clr.GetEmail() + Sep + clr.GetPassword() + End;
                }

                // ServerLoginResponseMsg
                case Message.Type.SLR:
                {
                    ServerLoginResponseMsg slr = (ServerLoginResponseMsg)msg;
                    return "SLR" + slr.IsAccepted() + Sep + slr.GetText() + End;
                }

                // ClientCreateAccountRequestMsg
                case Message.Type.CCA:
                {
                    ClientCreateAccountRequestMsg cca = (ClientCreateAccountRequestMsg)msg;
                    return "CCA" + cca.GetEmail() + Sep + cca.GetPassword() + End;
                }

                // ServerCreateAccountResponseMsg
                case Message.Type.SCA:
                {
                    ServerCreateAccountResponseMsg sca = (ServerCreateAccountResponseMsg)msg;
                    return "SCA" + sca.IsAccepted() + Sep + sca.GetText() + End;
                }

                // ClientCreateCharacterRequestMsg
				case Message.Type.CCC:
                {
					ClientCreateCharacterRequestMsg ccc = (ClientCreateCharacterRequestMsg)msg;
					return "CCC" + ccc.GetName() + End;
                }

                // ServerCreateCharacterResponseMsg
                case Message.Type.SCC:
                {
                    ServerCreateCharacterResponseMsg scc = (ServerCreateCharacterResponseMsg)msg;
                    return "SCC" + scc.IsAccepted() + Sep + scc.GetText() + End;
                }

                // ServerSendAccountMsg
                case Message.Type.SSA:
                {
                    ServerSendAccountMsg ssa = (ServerSendAccountMsg)msg;

                    // Serialize account
                    bf.Serialize(ms, ssa.GetAccount());
                    string account = System.Convert.ToBase64String(ms.ToArray());

                    return "SSA" + account + End;
                }

                // ServerSendCharacterMsg
                case Message.Type.SSC:
                {
                    ServerSendCharacterMsg ssc = (ServerSendCharacterMsg)msg;

                    // Serialize character
                    bf.Serialize(ms, ssc.GetCharacter());
                    string character = System.Convert.ToBase64String(ms.ToArray());
                    return "SSC" + character + End;
                }

                // ClientDeleteCharacterMsg
                case Message.Type.CDL:
                {
                        return "CDL" + End;
                }

                // ClientTrainAttributeRequestMsg
                case Message.Type.CTA:
                {
                    ClientTrainAttributeRequestMsg cta = (ClientTrainAttributeRequestMsg)msg;
                    return "CTA" + cta.GetAttributeType().ToString() + End;
                }

				// ClientUseSkillRequestMsg
				case Message.Type.CUS:
				{
					ClientUseSkillRequestMsg cus = (ClientUseSkillRequestMsg)msg;
					return "CUS" + cus.GetSkillType().ToString() + End;
				}

				// ServerUseSkillResponseMsg
				case Message.Type.SUS:
				{
					ServerUseSkillResponseMsg spa = (ServerUseSkillResponseMsg)msg;
					return "SUS" + spa.GetSkillType().ToString() + Sep + spa.GetAccepted() + Sep + spa.GetText() + End;
				}

				//ServerUseSkillResultMsg
				case Message.Type.SUR:
				{	
					ServerUseSkillResultMsg sur = (ServerUseSkillResultMsg)msg;
					
					return "SUR" + sur.GetSkillType().ToString() + Sep + sur.GetResultDescription() + End;
				}

                // ServerTrainAttributeResponseMsg
                case Message.Type.STA:
                {
                    ServerTrainAttributeResponseMsg sta = (ServerTrainAttributeResponseMsg)msg;
                    return "STA" + sta.GetAttributeType().ToString() + Sep + sta.GetAccepted() + Sep + sta.GetText() + End;
                }

                // ServerTrainAttributeResultMsg
                case Message.Type.STR:
                {
                    ServerTrainAttributeResultMsg str = (ServerTrainAttributeResultMsg)msg;
                    return "STR" + str.GetAttributeType().ToString() + Sep + str.GetResult().ToString() + End;
                }

				// ServerTerrainMatrixMsg
				case Message.Type.STM:
				{
					ServerTerrainMatrixMsg stm = (ServerTerrainMatrixMsg)msg;

                    bf.Serialize(ms, stm.GetTerrainMatrix ());
                    string terrainMatrix = System.Convert.ToBase64String(ms.ToArray());

                    return "STM" + terrainMatrix + End;
				}

				// ClientMoveCharacterMsg
				case Message.Type.CMC:
				{
					Position position = ((ClientMoveCharacterMsg)msg).GetPosition ();
					return "CMC" + position.X + Sep + position.Y + End;
				}

				// ServerCharacterCurrentAreaMsg
				case Message.Type.SAR:
				{
					ServerCharacterCurrentAreaMsg cua = (ServerCharacterCurrentAreaMsg)msg;

					bf.Serialize(ms, cua.GetCurrentArea());
					string currentArea = System.Convert.ToBase64String(ms.ToArray());

					return "SAR" + currentArea + End;
				}

				// ServerMoveCharacterResponseMsg
				case Message.Type.SMC:
				{
					ServerMoveCharacterResponseMsg smc = (ServerMoveCharacterResponseMsg)msg;

					return "SMC" + smc.IsAccepted() + Sep + smc.GetText() + End;
				}

                // ServerAreaMsg
				case Message.Type.SAI:
				{
					ServerAreaMsg sai = (ServerAreaMsg)msg;

					bf.Serialize(ms, sai.GetArea());
					string area = System.Convert.ToBase64String(ms.ToArray());
					return "SAI" + area + End;
				}

                // ClientAreaInfoRequestMsg
				case Message.Type.CAI:
				{
					ClientAreaInfoRequestMsg cai = (ClientAreaInfoRequestMsg)msg;

					bf.Serialize(ms, cai.GetPosition());
					string pos = System.Convert.ToBase64String(ms.ToArray());
					return "CAI" + pos + End;
				}

                // ChatMsg
                case Message.Type.CHT:
                {
                    ChatMsg cht = (ChatMsg)msg;
                    return "CHT" + cht.GetName() + Sep + cht.GetText() + Sep + cht.GetChatType().ToString() + End;
                }

                default:
                {
                    throw new NWOException("Unhandeled Message type: " + msg.GetMsgType());
                }
            }
        }

        /// <summary>
        /// Unpack a raw stirng message to a Message of proper type
        /// </summary>
        /// <param name="raw"></param>
        /// <returns></returns>
        public Message Unpack(string raw)
        {
			// Seperate three first sequence characters from raw message

			string typeString = raw.Substring (0, 3);
            raw = raw.Substring(3);

			// For deserialization
			MemoryStream ms ;
			BinaryFormatter bf = new BinaryFormatter();

            switch (typeString)
            {
                // TextMsg
                case "TXT":
                {
                    string text = raw.Substring(0, raw.IndexOf(End));
                    return new TextMsg(text);
                }

                // ClientDisconnectMsg
                case "CDC":
                {
                    string text = raw.Substring(0, raw.IndexOf(End));
                    return new ClientDisconnectMsg(text);
                }

                // ServerKickMsg
                case "SKK":
                {
                    string text = raw.Substring(0, raw.IndexOf(End));
                    return new ServerKickMsg(text);
                }

                // ServerShutdownMsg
                case "SSD":
                {
                    string text = raw.Substring(0, raw.IndexOf(End));
                    return new ServerShutdownMsg(text);
                }

                // ClientLoginRequestMsg
                case "CLR":
                {
                    string email = raw.Substring(0, raw.IndexOf(Sep));
                    raw = raw.Substring(raw.IndexOf(Sep) + 1);
                    string password = raw.Substring(0, raw.IndexOf(End));
                        
                    return new ClientLoginRequestMsg(email, password);
                }

                // ServerLoginResponseMsg
                case "SLR":
                {
                    string isAcceptedStr = raw.Substring(0, raw.IndexOf(Sep));

                    bool isAccepted = false;
                    if (isAcceptedStr == "True")
                        isAccepted = true;

                    raw = raw.Substring(raw.IndexOf(Sep) + 1);
                    string text = raw.Substring(0, raw.IndexOf(End));

                    return new ServerLoginResponseMsg(isAccepted, text);
                }

                // ClientCreateAccountRequestMsg
                case "CCA":
                {
                    string email = raw.Substring(0, raw.IndexOf(Sep));
                    raw = raw.Substring(raw.IndexOf(Sep) + 1);
                    string password = raw.Substring(0, raw.IndexOf(End));

                    return new ClientCreateAccountRequestMsg(email, password);
                }

                // ServerCreateAccountResponseMsg
                case "SCA":
                {
                    string isAcceptedStr = raw.Substring(0, raw.IndexOf(Sep));
                    bool isAccepted = false;
                    if (isAcceptedStr == "True")
                        isAccepted = true;
                    raw = raw.Substring(raw.IndexOf(Sep) + 1);
                    string text = raw.Substring(0, raw.IndexOf(End));

                    return new ServerCreateAccountResponseMsg(isAccepted, text);
                }

                // ClientCreateCharacterMsg
				case "CCC":
				{
					string name = raw.Substring(0, raw.IndexOf(End));
					return new ClientCreateCharacterRequestMsg(name);
				}

                // ServerCreateCharacterResponseMsg
				case "SCC":
				{
					string isAcceptedStr = raw.Substring(0, raw.IndexOf(Sep));
					bool isAccepted = false;
					if (isAcceptedStr == "True")
						isAccepted = true;
					raw = raw.Substring(raw.IndexOf(Sep) + 1);
					string text = raw.Substring(0, raw.IndexOf(End));
					
					return new ServerCreateCharacterResponseMsg(isAccepted, text);
				}

                // ServerSendAccountMsg
				case "SSA":
				{
					string account = raw.Substring(0, raw.IndexOf(End));
					
					byte[] b = System.Convert.FromBase64String( account );

					ms = new MemoryStream(b);
					bf = new BinaryFormatter();
									
				    return new ServerSendAccountMsg( (Account)bf.Deserialize(ms) );
				}	

                // ServerSendCharacterMsg
				case "SSC":
				{
					string character = raw.Substring(0, raw.IndexOf(End));
					
					byte[] b = System.Convert.FromBase64String(character);
					
					ms = new MemoryStream(b);
					return new ServerSendCharacterMsg( (Character)bf.Deserialize(ms) );
				}

                // ClientDeleteCharacterMsg
				case "CDL":
				{
					return new ClientDeleteCharacterMsg();
				}

                // ClientTrainAttributeRequestMsg
                case "CTA":
                {
                    string typeStr;
                    AttributeType type;

                    // Extract substrings from the raw string
                    typeStr = raw.Substring(0, raw.IndexOf(End));
                    
                    // Convert from string to expected data type
                    type = (AttributeType)Enum.Parse(typeof(AttributeType), typeStr);

                    return new ClientTrainAttributeRequestMsg(type);
                }

				// ClientUseSkillRequestMsg
				case "CUS":
				{
					string typeStr;
					SkillType type;
					
					// Extract substrings from the raw string
					typeStr = raw.Substring(0, raw.IndexOf(End));

					//convert from string to expected data type
					type = (SkillType)Enum.Parse(typeof(SkillType), typeStr);

					return new ClientUseSkillRequestMsg(type);
				}

				// ServerUseSkillResponseMsg
				case "SUS":
				{
					string typeStr, acceptedStr;
					SkillType type;
					bool accepted = false;
					string text;

					// Extract substrings from the raw string
					typeStr = raw.Substring(0, raw.IndexOf(Sep));
					raw = raw.Substring(raw.IndexOf(Sep) + 1);

					acceptedStr = raw.Substring(0, raw.IndexOf(Sep));
					raw = raw.Substring(raw.IndexOf(Sep) + 1);
					
					text = raw.Substring(0, raw.IndexOf(End));

					// Convert from string to expected data types
					type = (SkillType) Enum.Parse(typeof(SkillType), typeStr);

					if(acceptedStr == "True")
						accepted = true;

					return new ServerUseSkillResponseMsg(type, accepted, text);
				}

				// ServerUseSkillResultMsg
				case "SUR":
				{
					string typeStr, resultDescription;
					SkillType type;

					// Extract substrings from the raw string
					typeStr = raw.Substring(0, raw.IndexOf(Sep));
					raw = raw.Substring(raw.IndexOf(Sep) + 1);

					resultDescription = raw.Substring(0, raw.IndexOf(End));
					
					// Convert from string to expected data types
					type = (SkillType) Enum.Parse(typeof(SkillType), typeStr);

					return new ServerUseSkillResultMsg(type, resultDescription);
				}

                // ServerTrainAttributeResponseMsg
                case "STA":
                {
                    string typeStr, acceptedStr;
                    AttributeType type;
                    bool accepted = false;
					string text;

                    // Extract substrings from the raw string
                    typeStr = raw.Substring(0, raw.IndexOf(Sep));
                    raw = raw.Substring(raw.IndexOf(Sep) + 1);
					
					acceptedStr = raw.Substring(0, raw.IndexOf(Sep));
					raw = raw.Substring(raw.IndexOf(Sep) + 1);

                    text = raw.Substring(0, raw.IndexOf(End));

                    // Convert from string to expected data types
                    type = (AttributeType) Enum.Parse(typeof(AttributeType), typeStr);
                   
                    if (acceptedStr == "True")
                        accepted = true;

                    return new ServerTrainAttributeResponseMsg(type, accepted, text);
                }

                // ServerTrainAttributeResultMsg
                case "STR":
                {
                    string typeStr, resultStr;
                    AttributeType type;
                    int result;

                    // Extract substrings from the raw string
                    typeStr = raw.Substring(0, raw.IndexOf(Sep));
                    raw = raw.Substring(raw.IndexOf(Sep) + 1);
                    resultStr = raw.Substring(0, raw.IndexOf(End));

                    // Convert from string to expected data types
                    type = (AttributeType)Enum.Parse(typeof(AttributeType), typeStr);
                    result = Convert.ToInt32(resultStr);

                    return new ServerTrainAttributeResultMsg(type, result);
                }

				// ServerTerrainMatrixMsg
				case "STM":
				{
					string terrainMatrix = raw.Substring(0, raw.IndexOf(End));
					byte[] b = System.Convert.FromBase64String(terrainMatrix);
					
					ms = new MemoryStream(b);

					return new ServerTerrainMatrixMsg( (TerrainMatrix)bf.Deserialize(ms) );
				}

				// ClientMoveCharacterMsg
				case "CMC":
				{
					string position = raw.Substring(0, raw.IndexOf(End));
					string[] xy = position.Split(Sep.ToCharArray());
					return new ClientMoveCharacterMsg(new Position(Int32.Parse(xy[0]), Int32.Parse(xy[1])));
				}

				// ServerCharacterCurrentAreaMsg
				case "SAR":
				{
					string currentArea = raw.Substring(0, raw.IndexOf(End));
					byte[] b = System.Convert.FromBase64String(currentArea);

					ms = new MemoryStream(b);

					return new ServerCharacterCurrentAreaMsg( (Area)bf.Deserialize(ms) );
				}

				// ServerAreaMsg
				case "SAI":
				{
					string area = raw.Substring(0, raw.IndexOf(End));
					byte[] b = System.Convert.FromBase64String(area);

					ms = new MemoryStream(b);

					return new ServerAreaMsg((Area)bf.Deserialize(ms));
				}

				// ClientAreaInfoRequestMsg
				case "CAI":
				{
					string pos = raw.Substring(0, raw.IndexOf(End));
					byte[] b = System.Convert.FromBase64String(pos);

					ms = new MemoryStream(b);

					return new ClientAreaInfoRequestMsg((Position)bf.Deserialize(ms));
				}

				// ServerMoveCharacterResponseMsg
				case "SMC":
				{
					string[] data = raw.Substring(0, raw.IndexOf(End)).Split(Sep.ToCharArray());
					return new ServerMoveCharacterResponseMsg(Boolean.Parse(data[0]),data[1]);
				}

                // ChatMsg
                case "CHT":
                {
                    string name = raw.Substring(0, raw.IndexOf(Sep));
                    raw = raw.Substring(raw.IndexOf(Sep) + 1);
                    string text = raw.Substring(0, raw.IndexOf(Sep));
					raw = raw.Substring(raw.IndexOf(Sep) + 1);
					ChatMsg.ChatType chatType = (ChatMsg.ChatType)Enum.Parse( typeof(ChatMsg.ChatType), raw.Substring(0, raw.IndexOf(End)), true );


                    return new ChatMsg(name, text, chatType);
                }

                default:
                {
                    throw new NWOException("Unhandeled Message type: " + typeString);
                }

            }// end Switch
        } 
    }
}

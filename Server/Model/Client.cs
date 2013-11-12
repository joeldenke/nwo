using System.Collections.Generic;
using System.Net.Sockets;
using System.Net;
using Common;
using System;

namespace Server.Model
{
    /// <summary>
    /// Represents a client connection
    /// </summary>
	public class Client
	{
        private ServerAccount account;
        private MessageManager messageManager;
		private TcpClient tcpClient;
		private NetworkStream clientStream;
		private int clientID;
		private readonly int maxReadBytes = 4096*4;
        private bool authenticated = false;
        private bool timeout = false;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="tcpc"></param>
        /// <param name="clientID"></param>
        /// <param name="messageManager"></param>
		public Client(TcpClient tcpc, int clientID, MessageManager messageManager)
		{
            this.account = null;
			this.tcpClient = tcpc;
			this.clientStream = tcpClient.GetStream();
			this.clientID = clientID;
			this.messageManager = messageManager;
		}

        /// <summary>
        /// Set an accounnt to the Client
        /// </summary>
        /// <param name="account"></param>
        public void SetAccount(ServerAccount account)
        {
            this.account = account;
        }

        /// <summary>
        /// Get the account associated to the client
        /// </summary>
        /// <returns>
        /// The account objenct
        /// NOTE: null will be returned if no account object is set
        /// </returns>
        public ServerAccount GetAccount()
        {
            return account;
        }

        /// <summary>
        /// Send a message to the client
        /// </summary>
        /// <param name="msg"></param>
        public void Send(Message msg)
        {
            try
            {
                // TCPSEND
                byte[] data = System.Text.Encoding.ASCII.GetBytes(messageManager.Pack(msg));
                NetworkStream stream = tcpClient.GetStream();

                stream.Write(data, 0, data.Length);

                timeout = false;
            }
            catch (Exception)
            {
                timeout = true;
                throw new NWOException("Client " + clientID + " (" + GetIP() + ") timed out.");
            }
        }

        /// <summary>
        /// Recive all messages from the stream and put them in a list
        /// </summary>
        /// <returns></returns>
		public List<Message> RecievAll()
		{
			List<Message> recievedMessages = new List<Message>();

			// If there is anything to read
			if(clientStream.DataAvailable)
			{
				byte[] byteMsg = new byte[maxReadBytes];
				int bytesRead = clientStream.Read(byteMsg, 0, maxReadBytes);

				// Client disconnected
				if(bytesRead == 0)
				{

				}
				else
				{
					// Encode from byte to character string
					string readString = System.Text.Encoding.ASCII.GetString(byteMsg, 0, bytesRead);

					// TODO: CHECK FOR ESC CHAR AND SPLIT INTO MESSAGES

					recievedMessages.Add( messageManager.Unpack(readString) );
				}
			}
			return recievedMessages;
		}
	
        /// <summary>
        /// Get the tcpClient
        /// </summary>
        /// <returns></returns>
		public TcpClient GetTcpClient()
		{
			return tcpClient;
		}

        /// <summary>
        /// Get client id
        /// </summary>
        /// <returns></returns>
		public int GetClientID()
		{
			return clientID;
		}

        /// <summary>
        /// Close connection to client
        /// </summary>
		public void Close()
		{
			tcpClient.Close();
		}

        /// <summary>
        /// Set this when client is authenticated by log in
        /// </summary>
        /// <param name="value">
        /// True when logged in
        /// False when not logged in
        /// </param>
        public void SetAuthenticated(bool value)
        {
            authenticated = value;
        }

        /// <summary>
        /// Get if client is authenticated
        /// </summary>
        /// <returns>
        /// True if authenticated, false if not
        /// </returns>
        public bool GetAuthenticated()
        {
            return authenticated;
        }

        /// <summary>
        /// Get the IP address from the client
        /// </summary>
        /// <returns>
        /// The IP address as a string
        /// </returns>
        public string GetIP()
        {
            String ip = "Unknown";
            try
            {
                ip = ((IPEndPoint)tcpClient.Client.RemoteEndPoint).Address.ToString();
            }
            catch (Exception) { }

            return ip;
        }

        /// <summary>
        /// Get if client has timed out
        /// </summary>
        /// <returns></returns>
        public bool GetTimeout()
        {
            return timeout;
        }

        /// <summary>
        /// Set value for timeout
        /// </summary>
        /// <param name="value"></param>
        public void SetTimeout(bool value)
        {
            timeout = value;
        }
    }
}


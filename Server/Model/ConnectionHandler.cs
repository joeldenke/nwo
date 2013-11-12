using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System;
using Server.Dba;
using Common;

namespace Server.Model
{
    /// <summary>
    /// Handles incoming connection requests from clients
    /// and comminucation with clients
    /// </summary>
	public class ConnectionHandler
	{
		public readonly int maxClients = 100;
		public readonly int portNr = 2003;
        public MessageManager messageManager;
        public List<Client> disconnectedClients;

		private List<Client> clients;
        private TcpListener tcpListener;
		
        /// <summary>
        /// Constructor
        /// </summary>
		public ConnectionHandler()
		{
			clients = new List<Client>();
			tcpListener = new TcpListener(IPAddress.Any, portNr);
			messageManager = new MessageManager();
            disconnectedClients = new List<Client>();
		}

        /// <summary>
        /// Start accepting incoming connections
        /// </summary>
		public void Start()
		{
            // Start listening to incoming connections
            tcpListener.Start();
		}

        /// <summary>
        /// Disconnect all clients and stop listening to client requests
        /// </summary>
        public void Disconnect(string text)
        {
            // Shutdown all clients
            ServerShutdownMsg sdmsg = new ServerShutdownMsg(text);

            foreach (Client c in clients)
            {
                // Send shutdown message
                try
                {
                    c.Send(sdmsg);
                }
                catch (NWOException) { }

                // Close client connection
                disconnectedClients.Add(c);
            }

            // Stop listening to incoming connections
            tcpListener.Stop();
        }

        /// <summary>
        /// Remove all disconnected clients
        /// </summary>
        public void HandleDisconnectedClients()
        {
            while (disconnectedClients.Count != 0)
            {
                Client c = disconnectedClients[0];
                disconnectedClients.Remove(c);
                clients.Remove(c);
                c.Close();
            }
        }

        /// <summary>
        /// Get all clients in a list
        /// </summary>
        /// <returns></returns>
		public List<Client> GetClientList()
		{
			return clients;
		}

        /// <summary>
        /// Listen for and accept incoming connection requests
        /// </summary>
		public void HandleClientConnectionRequests()
		{
			// Do while there are pendning connection requests
			while(tcpListener.Pending())
			{
                // Check if there the max client number is reached
                if (clients.Count >= maxClients)
                    return;

				// Accept a client tcp request (BLOCKS!)
				TcpClient tcpClient = this.tcpListener.AcceptTcpClient();
				
				// Add the new client
				AddClient(tcpClient);
			}
		}

        /// <summary>
        /// Send a text message to the client with id clientID
        /// Throws: InvalidClientException if no such client exists
        /// </summary>
        /// <param name="clientID"></param>
        /// <param name="text"></param>
		public void SendClientTextMessage(int clientID, string text)
		{
			if(text.Length == 0)
				return;

			Client c = GetClient(clientID);

			ChatMsg tmsg = new ChatMsg("NWO",text,ChatMsg.ChatType.PRIVATE);

			// Send the text message to the client
            try
            {
                c.Send(tmsg);
            }
            catch (NWOException)
            {
                disconnectedClients.Add(c);
            }
		}

        /// <summary>
        /// Get the client with id clientID
        /// Throws: InvalidClientException if no such client exists
        /// </summary>
        /// <param name="clientID"></param>
        /// <returns>
        /// Returns the client with id clientID
        /// </returns>
		public Client GetClient(int clientID)
		{
			foreach(Client c in clients)
			{
				if(c.GetClientID() == clientID)
					return c;
			}

			throw new NoSuchClientException();
		}

        /// <summary>
        /// Broadcast a message to all clients
        /// </summary>
        /// <param name="text"></param>
		public void SendBroadcastTextMessage(string text)
		{
			if(text.Length == 0)
				return;

			foreach(Client c in clients)
			{
				ChatMsg tmsg = new ChatMsg("NWO",text,ChatMsg.ChatType.PUBLIC);

				// Send the text message to the client
                try
                {
                    c.Send(tmsg);
                }
                catch (NWOException)
                {
                    disconnectedClients.Add(c);
                }
			}
		}

        /// <summary>
        /// Kick client with id clientID
        /// Throws NoSuchClientException if client doesnt exist
        /// </summary>
        /// <param name="clientID"></param>
        /// <param name="text"></param>
        public void KickClient(int clientID, string text)
        {
            Client c = GetClient(clientID);

            ServerKickMsg kmsg = new ServerKickMsg(text);

            // Send the text message to the client
            try
            {
                c.Send(kmsg);
            }
            catch (NWOException) { }

            // Close connection
            disconnectedClients.Add(c);
          
        }

        /// <summary>
        /// Kick all clients on the server
        /// </summary>
        /// <param name="text"></param>
		public void KickAllClients(string text)
		{
			foreach(Client c in clients)
			{
                KickClient(c.GetClientID(), text);
			}
		}

        /// <summary>
        /// Get the IP address the server is running on
        /// </summary>
        /// <returns></returns>
        public string GetIpAddress()
        {
            IPHostEntry host;
            string localIP = "?";
            host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (IPAddress ip in host.AddressList)
            {
                if (ip.AddressFamily.ToString() == "InterNetwork")
                {
                    localIP = ip.ToString();
                }
            }
            return localIP;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="c"></param>
        /// <returns></returns>
        public Client GetClientFromCharacter(ServerCharacter character)
        {
            if (character == null)
                return null;

            foreach (Client client in clients)
            {
                if (client.GetAccount() != null)
                {
                    if (client.GetAccount().Character == character)
                        return client;
                }
            }
            return null;
        }

        /// <summary>
        /// Add a client to the list
        /// </summary>
        /// <param name="tcpC"></param>
        /// <returns></returns>
        private Client AddClient(TcpClient tcpC)
        {
            // Check if max clients is reached
            if (clients.Count >= maxClients)
                return null;

            // Create and add the client to the client list
            Client c = new Client(tcpC, GetAvailableID(), messageManager);
            clients.Add(c);

            return c;
        }

        /// <summary>
        /// Returns the next available free client ID
        /// </summary>
        /// <returns></returns>
        private int GetAvailableID()
        {
            // Create an array with maxClient buckets
            // False if ID is free
            // True if ID is occupied
            bool[] usedIDs = new bool[maxClients + 1];

            // Fill buckets with all used ID's
            foreach (Client c in clients)
            {
                usedIDs[c.GetClientID()] = true;
            }

            // Return the first unused id
            int i = 1;
            while (usedIDs[i])
                i++;
            return i;
        }

	}
}


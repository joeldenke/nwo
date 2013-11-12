using Common;
using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Net.Sockets;
using System.Threading;

namespace Client.Model
{
	public class ServerManager
	{
		private enum ConnectionStatus
		{
			CONNECTING,
			CONNECTED,
			DISCONNECTED,
			FAILED
		}

		private static readonly int MAX_SIZE = 4096*4;
		private string ip;
		private int port;
		private MessageManager mm;
		private ConcurrentQueue<Message> sendQueue;
		private Mutex sendLock;
		private ConcurrentQueue<Message> receiveQueue;
		private ConnectionStatus connectionStatus;
		private Mutex connectionStatusLock;
		private NWOException connectionException;
		private Thread connectionThread;
		private TcpClient socket;
		private NetworkStream stream;

		/// <summary>
		/// Initializes a new instance of the <see cref="Client.Model.ServerManager"/> class.
		/// </summary>
		public ServerManager (string ip, int port)
		{
			this.ip = ip;
			this.port = port;

			sendQueue = new ConcurrentQueue<Message> ();
			sendLock = new Mutex ();
			receiveQueue = new ConcurrentQueue<Message> ();

			mm = new MessageManager ();

			connectionStatusLock = new Mutex ();
			setConnectionStatus (ConnectionStatus.DISCONNECTED);
			connectionThread = new Thread (handleConnection);
			connectionThread.IsBackground = true;
			connectionThread.Start ();
		}

		/// <summary>
		/// Connect to server, if not already connected.
		/// </summary>
		public void Connect ()
		{
			if (getConnectionStatus () != ConnectionStatus.CONNECTED)
				setConnectionStatus (ConnectionStatus.CONNECTING);
		}

		/// <summary>
		/// Disconnect from server.
		/// </summary>
		public void Disconnect ()
		{
			if (getConnectionStatus () != ConnectionStatus.DISCONNECTED)
				setConnectionStatus (ConnectionStatus.DISCONNECTED);
		}

		/// <summary>
		/// Send message and disconnect, if not already disconnected.
		/// </summary>
		/// <exception cref="NWOException">
		/// In case the message could not be sent an exception is thrown.
		/// </exception>
		public void SendAndDisconnect (Message message)
		{
			if (getConnectionStatus () != ConnectionStatus.DISCONNECTED) {
				send (message);
				setConnectionStatus (ConnectionStatus.DISCONNECTED);
			}
		}

		/// <summary>
		/// Send message to server.
		/// </summary>
		/// <param name='msg'>
		/// Message.
		/// </param>
		/// <exception cref="NWOException">
		/// Any connection exception which has occurred since sending last time will be thrown when used. 
		/// </exception>
		public void Send (Message message)
		{
			if (connectionException != null) {
				NWOException exception = connectionException;
				connectionException = null;
				throw exception;
			}

			sendQueue.Enqueue (message);
		}

		/// <summary>
		/// Receives all received messages from server.
		/// </summary>
		/// <returns>
		/// List of received messages.
		/// </returns>
		public List<Message> ReceiveAll ()
		{
			List<Message> list = new List<Message> ();
			Message message;
			while (receiveQueue.TryDequeue (out message))
				list.Add (message);
			return list;
		}

		/// <summary>
		/// Determines whether there is a server connection.
		/// </summary>
		/// <returns>
		/// <c>true</c> if this instance is connected; otherwise, <c>false</c>.
		/// </returns>
		/// <exception cref="NWOException">
		/// In case the connection is starting, pending, failed or terminated, an exception is thrown.
		/// </exception>
		public bool IsConnected ()
		{
			switch (getConnectionStatus ()) {
			case ConnectionStatus.CONNECTING:
				throw new NWOException ("Connecting ...");

			case ConnectionStatus.FAILED:
				throw new NWOException ("Failed to connect to server.");

			case ConnectionStatus.CONNECTED:
				return true;

			case ConnectionStatus.DISCONNECTED:
				return false;

			default:
				throw new NWOException ("Unrecognized connection status.");
			}
		}

		/// <summary>
		/// Panic server manager.
		/// </summary>
		/// <description>
		/// Forcibly terminates this instance. Any interaction with this instance after a call to this method will
		/// result in undefined behaviour.
		/// </description>
		public void Panic ()
		{
			connectionThread.Abort ();
		}

		private void handleConnection ()
		{
			while (true) {
				blockUntilConnecting ();
				try {
					connect ();
					sendAndRecieve ();
					socket.Close ();

				} catch (NWOException e) {
					connectionException = e;
					setConnectionStatus (ConnectionStatus.FAILED);
				}
			}
		}

		private void blockUntilConnecting ()
		{
			while (getConnectionStatus () != ConnectionStatus.CONNECTING)
				Thread.Sleep (1000);
		}

		private void connect ()
		{
			try {
				socket = new TcpClient (ip, port);
				stream = socket.GetStream ();
				setConnectionStatus (ConnectionStatus.CONNECTED);

			} catch (Exception e) {
				Debug.WriteLine (e);
				throw new NWOException ("Unable to connect.");
			}
		}

		private void sendAndRecieve ()
		{
			while (getConnectionStatus () == ConnectionStatus.CONNECTED) {
				sendFromQueue (sendQueue);
				receiveToQueue (receiveQueue);
				Thread.Sleep (10);
			}
		}

		public void sendFromQueue (ConcurrentQueue<Message> queue)
		{
			Message message;
			while (queue.TryDequeue (out message))
				send (message);
		}

		public void send (Message message)
		{
			sendLock.WaitOne ();
			try {
				Byte[] data = System.Text.Encoding.ASCII.GetBytes (mm.Pack (message));
				stream.Write (data, 0, data.Length);

			} catch (Exception e) {
				Debug.WriteLine (e);
				throw new NWOException ("Unable to send data to server.");

			} finally {
				sendLock.ReleaseMutex ();
			}
		}

		public void receiveToQueue (ConcurrentQueue<Message> queue)
		{
			try {
				Byte[] input = new Byte[MAX_SIZE];
				String inputEncoded;
				int bytesRead;
				string[] packedMessages;

				while (stream.DataAvailable) {
					bytesRead = stream.Read (input, 0, input.Length);
					inputEncoded = System.Text.Encoding.ASCII.GetString (input, 0, bytesRead);
					packedMessages = inputEncoded.Split (new Char[] { '\x001B' }, StringSplitOptions.RemoveEmptyEntries);

					foreach (string packedMessage in packedMessages) {
						Message message = mm.Unpack (packedMessage + '\x001B');
						queue.Enqueue (message);
					}
				}
			} catch (Exception e) {
				Debug.WriteLine (e);
				throw new NWOException ("Unable to receive data from server.");
			}
		}

		private ConnectionStatus getConnectionStatus ()
		{
			ConnectionStatus status;
			connectionStatusLock.WaitOne ();
			{
				status = connectionStatus;
			}
			connectionStatusLock.ReleaseMutex ();
			return status;
		}

		private void setConnectionStatus (ConnectionStatus status)
		{
			connectionStatusLock.WaitOne ();
			{
				connectionStatus = status;
			}
			connectionStatusLock.ReleaseMutex ();
		}
	}
}


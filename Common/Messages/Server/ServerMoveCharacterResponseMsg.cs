using System;

namespace Common
{
	/// <summary>
	/// Server move character response message.
	/// </summary>
	public class ServerMoveCharacterResponseMsg : Message
	{
		private bool accepted;
		private string text;

		/// <summary>
		/// Initializes a new instance of the <see cref="Common.ServerMoveCharacterResponseMsg"/> class.
		/// </summary>
		/// <param name='accepted'>
		/// Accepted.
		/// </param>
		/// <param name='text'>
		/// Text.
		/// </param>
		public ServerMoveCharacterResponseMsg (bool accepted, string text)
		{
            this.accepted = accepted;
			this.text = text;
		}

		/// <summary>
		/// Determines whether the requested movement was accepted.
		/// </summary>
		/// <returns>
		/// <c>true</c> if accepted; otherwise, <c>false</c>.
		/// </returns>
		public bool IsAccepted ()
		{
			return accepted;
		}

		/// <summary>
		/// Gets the text message.
		/// </summary>
		/// <returns>
		/// The text.
		/// </returns>
		public String GetText()
		{
			return text;
		}

		/// <summary>
		/// Gets the type of the message.
		/// </summary>
		/// <returns>
		/// The message type.
		/// </returns>
		public override Type GetMsgType ()
		{
			return Message.Type.SMC;
		}
	}
}


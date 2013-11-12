using System;

namespace Common
{
	/// <summary>
	/// Client move character message.
	/// </summary>
	public class ClientMoveCharacterMsg : Message
	{
		private Position position;

		/// <summary>
		/// Initializes a new instance of the <see cref="Common.ClientMoveCharacterMsg"/> class.
		/// </summary>
		/// <param name='position'>
		/// Position.
		/// </param>
		public ClientMoveCharacterMsg (Position position)
		{
			this.position = position;
		}

		/// <summary>
		/// Gets the position.
		/// </summary>
		/// <returns>
		/// The position.
		/// </returns>
		public Position GetPosition ()
		{
			return position;
		}

		/// <summary>
		/// Gets the type of the message.
		/// </summary>
		/// <returns>
		/// The message type.
		/// </returns>
        public override Type GetMsgType()
        {
            return Message.Type.CMC;
        }
	}
}


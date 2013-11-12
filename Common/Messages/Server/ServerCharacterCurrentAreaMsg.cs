using System;

namespace Common
{
	/// <summary>
	/// Server character current area message.
	/// </summary>
	public class ServerCharacterCurrentAreaMsg : Message
	{
		private Area currentArea;

		/// <summary>
		/// Initializes a new instance of the <see cref="Common.ServerCharacterCurrentAreaMsg"/> class.
		/// </summary>
		/// <param name='area'>
		/// Area.
		/// </param>
		public ServerCharacterCurrentAreaMsg (Area area)
		{
			this.currentArea = area;
		}

		/// <summary>
		/// Gets the current client character area.
		/// </summary>
		/// <returns>
		/// The current area.
		/// </returns>
		public Area GetCurrentArea()
		{
			return currentArea;
		}

		/// <summary>
		/// Gets the type of the message.
		/// </summary>
		/// <returns>
		/// The message type.
		/// </returns>
		public override Type GetMsgType ()
		{
			return Message.Type.SAR;
		}
	}
}


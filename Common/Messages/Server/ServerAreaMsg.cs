using System;

namespace Common
{
	/// <summary>
	/// Server character current area message.
	/// </summary>
	public class ServerAreaMsg : Message
	{
		private Area area;

		/// <summary>
		/// Initializes a new instance of the <see cref="Common.ServerAreaMsg"/> class.
		/// </summary>
		/// <param name='area'>
		/// Area.
		/// </param>
		public ServerAreaMsg (Area area)
		{
			this.area = area;
		}

		/// <summary>
		/// Gets the client requested area.
		/// </summary>
		/// <returns>
		/// The current area.
		/// </returns>
		public Area GetArea()
		{
			return area;
		}

		/// <summary>
		/// Gets the type of the message.
		/// </summary>
		/// <returns>
		/// The message type.
		/// </returns>
		public override Type GetMsgType ()
		{
			return Message.Type.SAI;
		}
	}
}


using System;

namespace Common
{
	public class ClientUseSkillRequestMsg : Message
	{
		private SkillType type;

		/// <summary>
		/// Initializes a new instance of the <see cref="Common.ClientUseSkillRequestMsg"/> class.
		/// </summary>
		/// <param name='type'>
		/// Type.
		/// </param>
		public ClientUseSkillRequestMsg (SkillType type)
		{
			this.type = type;
		}

		/// <summary>
		/// Gets the type of the action.
		/// </summary>
		/// <returns>
		/// The action type.
		/// </returns>
		public SkillType GetSkillType ()
		{
			return type;
		}

		/// <summary>
		/// Gets the type of the message.
		/// </summary>
		/// <returns>
		/// The message type.
		/// </returns>
		public override Type GetMsgType ()
		{
			return Message.Type.CUS;
		}
	}
}


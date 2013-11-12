using System;

namespace Common
{
	public class ServerUseSkillResponseMsg : Message
	{
		private SkillType type;
		private bool accepted;
		private string text;

		/// <summary>
		/// Initializes a new instance of the <see cref="Common.ServerUseSkillResponseMsg"/> class.
		/// </summary>
		/// <param name='type'>
		/// Type.
		/// </param>
		/// <param name='accepted'>
		/// Accepted.
		/// </param>
		/// <param name='text'>
		/// Text.
		/// </param>
		public ServerUseSkillResponseMsg (SkillType type, bool accepted, string text)
		{
			this.type = type;
			this.accepted = accepted;
			this.text = text;
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
		/// Gets a boolean "accepted".
		/// </summary>
		/// <returns>
		/// A boolean indicating if the USESKILL request is accepted.
		/// </returns>
		public bool GetAccepted ()
		{
			return accepted;
		}

		/// <summary>
		/// Gets the text.
		/// </summary>
		/// <returns>
		/// Reason for denied/granted request.
		/// </returns>
		public string GetText ()
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
			return Message.Type.SUS;
		}
	}
}


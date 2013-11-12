using System;

namespace Common
{
	public class ServerUseSkillResultMsg : Message
	{
		private SkillType type;
		private string resultDescription; //or comment, whatever...

		/// <summary>
		/// Initializes a new instance of the <see cref="Common.ServerUseSkillResultMsg"/> class.
		/// </summary>
		/// <param name='type'>
		/// Type.
		/// </param>
		/// <param name='resultDescription'>
		/// Result description. eg "you got 100 food"
		/// </param>
		public ServerUseSkillResultMsg (SkillType type, string resultDescription)
		{
			this.type = type;
			this.resultDescription = resultDescription;
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
		/// Gets the result description.
		/// </summary>
		/// <returns>
		/// The result description.
		/// </returns>
		public string GetResultDescription ()
		{
			return resultDescription;
		}

		/// <summary>
		/// Gets the type of the message.
		/// </summary>
		/// <returns>
		/// The message type.
		/// </returns>
		public override Type GetMsgType ()
		{
			return Message.Type.SUR;
		}
	}
}


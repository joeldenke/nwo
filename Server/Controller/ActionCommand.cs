using System;

namespace Server
{
	public class ActionCommand
	{
		private string Name;
		public DateTime FinishTime {get; set;}

		public ActionCommand (string actionName)
		{
			Name = actionName;
		}
	}
}
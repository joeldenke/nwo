using System;
using System.Collections.Generic;

namespace Server
{
	public class ActionQue
	{
		private Queue<ActionCommand> actions;
		private bool running = false;

		public ActionQue ()
		{
			actions = new Queue<ActionCommand>();
		}
		/// <summary>
		/// Does the action.
		/// </summary>
		/// <param name='actionName'>
		/// Action name.
		/// </param>
		public void DoAction (string actionName)
		{
			DateTime dt = DateTime.Now;
			dt.AddMinutes(1);

			ActionCommand action = new ActionCommand(actionName);
			action.FinishTime = dt;
			actions.Enqueue(action);
		}

		// @TODO: Uncertain how to solve if the queue goes empty
		// Should work if you add new action if nothing is in the current queue.
		public void run ()
		{
			if (!running) {
				running = true;

				while (running) {
					if (actions.Count == 0) {
						running = false;
					} else {
						ActionCommand action = actions.Peek();

						if (DateTime.Now > action.FinishTime) {
							actions.Dequeue();
						}
					}
				}
			}
		}
	}
}


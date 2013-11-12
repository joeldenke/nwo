using System;
using Common;

namespace Client.Controller
{
	public abstract class Handler
	{
		protected View.ViewManager viewManager;
		protected Model.ModelManager modelManager;

		public Handler (View.ViewManager view, Model.ModelManager model)
		{
			this.viewManager = view;
			this.modelManager = model;
		}
		/// <summary>
		/// Handles the event.
		/// </summary>
		/// <param name='userEvent'>
		/// User event.
		/// </param>
		internal abstract void HandleEvent(View.UserEvent userEvent);

		/// <summary>
		/// Handles the lost connection. Closes connection, and 
		/// gets us back to the launcher view.
		/// </summary>
		/// <param name='ne'>
		/// Ne.
		/// </param>
		protected void handleLostConnection (NWOException ne)
		{
			modelManager.GetServerManager ().Disconnect ();
			View.PageLauncher launcherView = viewManager.LoadLauncherPage ();
			launcherView.SetStatusMessage (
				"Lost communication with server. " + "  Reason: " + ne.Message);
		}
	}
}


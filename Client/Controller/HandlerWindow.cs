using System;
using System.Text.RegularExpressions;
using Common;

namespace Client.Controller
{
	public class HandlerWindow : Handler
	{
		public HandlerWindow (View.ViewManager view, Model.ModelManager model) :
			base(view, model){}

		override internal void HandleEvent (View.UserEvent userEvent)
		{
			ClientDisconnectMsg dm = new ClientDisconnectMsg ("Disconnect-request from client.");
			try {
				modelManager.GetServerManager ().SendAndDisconnect (dm);
			} catch {
			}
			modelManager.GetServerManager ().Panic ();
			viewManager.Destroy ();
		}

	}
}


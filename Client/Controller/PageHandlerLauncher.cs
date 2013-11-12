using System;
using System.Diagnostics;
using System.Text.RegularExpressions;
using Common;

namespace Client.Controller
{
	public class PageHandlerLauncher : PageHandler
	{
		private static string REGEX_EMAIL = @"^[\w!#$%&'*+\-/=?\^_`{|}~]+(\.[\w!#$%&'*+\-/=?\^_`{|}~]+)*" + "@" + @"((([\-\w]+\.)+[a-zA-Z]{2,4})|(([0-9]{1,3}\.){3}[0-9]{1,3}))$";

		public PageHandlerLauncher (View.ViewManager view, Model.ModelManager modelManager) :
			base(view, modelManager)
		{
		}

		override internal void SetView ()
		{
			page = viewManager.LoadLauncherPage ();
		}

		override internal void HandleEvent (View.UserEvent userEvent)
		{
			View.PageLauncher launcher = (View.PageLauncher) page;
			switch (userEvent) {
			case View.UserEvent.LAUNCHER_SHOW_LOGIN:
				launcher.ShowLogin ();
				break;
			case View.UserEvent.LAUNCHER_SHOW_REGISTRATION:
				launcher.ShowRegistration ();
				break;
			case View.UserEvent.LAUNCHER_LOGIN:
				login ();
				break;
			case View.UserEvent.LAUNCHER_REGISTER:
				register ();
				break;
			case View.UserEvent.LAUNCHER_QUIT:
				viewManager.TriggerEvent (View.UserEvent.WINDOW_QUIT);
				break;
			}
		}

		override public void UpdatePage()
		{

		}
		/// <summary>
		/// Connect via socket.
		/// Register user to server.
		/// </summary>
		private void register ()
		{
			View.PageLauncher launcher = (View.PageLauncher) page;
			launcher.ResetEntryColors ();

			if (!Regex.IsMatch (launcher.GetRegistrationEmail (), REGEX_EMAIL)) {
				launcher.FailRegistrationEmail ();
				launcher.SetStatusMessage ("The e-mail you provided is not vaild.");
				return;
			}

			if (launcher.GetRegistrationPassword1 () != launcher.GetRegistrationPassword2 ()) {
				launcher.FailRegistrationPassword ();
				launcher.SetStatusMessage ("The provided passwords do not match.");
				return;
			}

			if (launcher.GetRegistrationPassword1 ().Length < 6) {
				launcher.FailRegistrationPassword ();
				launcher.SetStatusMessage ("The password has to be at least six characters long.");
				return;
			}

			launcher.SetStatusMessage ("Creating account ...");

			Message cca = new ClientCreateAccountRequestMsg (launcher.GetRegistrationEmail (), launcher.GetRegistrationPassword1 ());
			try {
				modelManager.GetServerManager ().Connect ();
				modelManager.GetServerManager ().Send (cca);
			} catch (NWOException e) {
				launcher.SetStatusMessage (e.Message);
			} catch (Exception e) {
				launcher.SetStatusMessage ("An unexpected condition occurred.");
				Debug.WriteLine (e);
			}
		}
		/// <summary>
		/// Login to server.
		/// Connect via socket.
		/// Send req msg.
		/// </summary>
		private void login ()
		{
			View.PageLauncher launcher = (View.PageLauncher) page;
			launcher.ResetEntryColors ();

			if (!Regex.IsMatch (launcher.GetLoginEmail (), REGEX_EMAIL) || launcher.GetLoginPassword ().Length < 6) {
				launcher.FailLogin ();
				launcher.SetStatusMessage ("The credentials you provided are not vaild.");
				return;
			}

			Message clr = new ClientLoginRequestMsg (launcher.GetLoginEmail (), launcher.GetLoginPassword ());
			try {
				modelManager.GetServerManager ().Connect ();
				modelManager.GetServerManager ().Send (clr);
			} catch (NWOException e) {
				launcher.SetStatusMessage (e.Message);
			} catch (Exception e) {
				launcher.SetStatusMessage ("An unexpected condition occurred.");
				Debug.WriteLine (e);
			}
		}
	}
}


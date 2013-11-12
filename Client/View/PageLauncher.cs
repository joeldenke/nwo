using System;

namespace Client.View
{
	using Gtk;
	using Gdk;

	/// <summary>
	/// The Launcher view.
	/// </summary>
	/// <description>
	/// The launcher the user utilizes in order to login and register an account.
	/// </description>
	public sealed class PageLauncher : Page
	{
		private ActionEvent actionEvent;

		private Fixed background;
		private Label title;
		private Label message;
		private EventBox container;

		private Fixed loginContainer;
		private Entry loginEntryEmail;
		private Entry loginEntryPassword;
		private EventBox loginButtonShowRegistration;

		private Fixed registrationContainer;
		private Entry registrationEntryEmail;
		private Entry registrationEntryPassword1;
		private Entry registrationEntryPassword2;
		private EventBox registrationButtonShowLogin;
		private EventBox registrationButtonRegister;

		/// <summary>
		/// Initializes a new instance of the <see cref="Client.View.ViewLauncher"/> class.
		/// </summary>
		public PageLauncher (ActionEvent actionEvent)
		{
			this.actionEvent = actionEvent;

			background = new Fixed ();
			background.SetSizeRequest (640, 480);
			{
				background.Put (WidgetFactory.CreateImage ("Graphics/login_bg.png", 640, 480), 0, 0);
				background.Put (title = WidgetFactory.CreateLabel ("", false), 20, 140);
				background.Put (message = WidgetFactory.CreateLabel ("", false), 20, 444);

				container = new EventBox ();
				container.VisibleWindow = false;
				container.KeyReleaseEvent += keyHandler;
				{
					loginContainer = new Fixed ();
					{
						VBox loginBox = new VBox (false, 0);
						loginBox.SetSizeRequest (240, 240);
						loginBox.BorderWidth = 0;
						{
							loginBox.PackStart (WidgetFactory.CreateLabel ("E-mail", false), false, false, 5);
							loginBox.PackStart (loginEntryEmail = WidgetFactory.CreateEntryField (), false, false, 0);
							loginBox.PackStart (WidgetFactory.CreateLabel ("Password", false), false, false, 5);
							loginBox.PackStart (loginEntryPassword = WidgetFactory.CreateEntryPassword (), false, false, 0);
							loginBox.PackStart (WidgetFactory.CreateSpacer (5), false, false, 0);

							HBox boxButtons = new HBox (false, 0);
							{
								boxButtons.PackStart (WidgetFactory.CreateButtonText ("Login", false, this.actionEvent, UserEvent.LAUNCHER_LOGIN), true, true, 0);
								boxButtons.PackStart (WidgetFactory.CreateSpacer (5), false, false, 0);
								boxButtons.PackEnd (loginButtonShowRegistration = WidgetFactory.CreateButtonText ("Register", false, actionEvent, UserEvent.LAUNCHER_SHOW_REGISTRATION), false, false, 0);
							}
							loginBox.PackStart (boxButtons, false, false, 0);
						}
						loginContainer.Put (loginBox, 20, 160);
					}

					registrationContainer = new Fixed ();
					{
						VBox registrationBox = new VBox (false, 0);
						registrationBox.SetSizeRequest (240, 240);
						registrationBox.BorderWidth = 0;
						{
							registrationBox.PackStart (WidgetFactory.CreateLabel ("E-mail", false), false, false, 5);
							registrationBox.PackStart (registrationEntryEmail = WidgetFactory.CreateEntryField (), false, false, 0);
							registrationBox.PackStart (WidgetFactory.CreateLabel ("Password", false), false, false, 5);
							registrationBox.PackStart (registrationEntryPassword1 = WidgetFactory.CreateEntryPassword (), false, false, 0);
							registrationBox.PackStart (WidgetFactory.CreateLabel ("Re-enter password", false), false, false, 5);
							registrationBox.PackStart (registrationEntryPassword2 = WidgetFactory.CreateEntryPassword (), false, false, 0);
							registrationBox.PackStart (WidgetFactory.CreateSpacer (5), false, false, 0);

							HBox boxButtons = new HBox (false, 0);
							{
								boxButtons.PackStart (registrationButtonShowLogin = WidgetFactory.CreateButtonText ("Cancel", false, actionEvent, UserEvent.LAUNCHER_SHOW_LOGIN), false, false, 0);
								boxButtons.PackStart (WidgetFactory.CreateSpacer (5), false, false, 0);
								boxButtons.PackEnd (registrationButtonRegister = WidgetFactory.CreateButtonText ("Register", false, actionEvent, UserEvent.LAUNCHER_REGISTER), true, true, 0);
							}
							registrationBox.PackStart (boxButtons, false, false, 0);
						}
						registrationContainer.Put (registrationBox, 20, 160);
					}
				}
				background.Put (container, 0, 0);

				container.Add (loginContainer); // Set default container.
			}
		}

		/// <summary>
		///  Gets the GTK# container representing the View. 
		/// </summary>
		/// <returns>
		///  GTK# Widget container. 
		/// </returns>
		public Widget GetContainer ()
		{
			return background;
		}

		/// <summary>
		/// Gets the e-mail address entered in the login screen.
		/// </summary>
		/// <returns>
		/// E-mail address.
		/// </returns>
		public string GetLoginEmail ()
		{
			return loginEntryEmail.Text;
		}

		/// <summary>
		/// Gets the password entered in the login screen.
		/// </summary>
		/// <returns>
		/// Password.
		/// </returns>
		public string GetLoginPassword ()
		{
			return loginEntryPassword.Text;
		}

		/// <summary>
		/// Gets the e-mail entered in the registration screen.
		/// </summary>
		/// <returns>
		/// E-mail.
		/// </returns>
		public string GetRegistrationEmail ()
		{
			return registrationEntryEmail.Text;
		}

		/// <summary>
		/// Gets the first password entered in the registration screen.
		/// </summary>
		/// <returns>
		/// Password.
		/// </returns>
		public string GetRegistrationPassword1 ()
		{
			return registrationEntryPassword1.Text;
		}

		/// <summary>
		/// Gets the second password entered in the registration screen.
		/// </summary>
		/// <returns>
		/// Password.
		/// </returns>
		public string GetRegistrationPassword2 ()
		{
			return registrationEntryPassword2.Text;
		}

		/// <summary>
		///  Determines whether this screen desires the window to be resizable or not. 
		/// </summary>
		/// <returns>
		/// <c>true</c> if this instance is window resizable; otherwise, <c>false</c>.
		/// </returns>
		public bool GetWindowRequestResizable ()
		{
			return false;
		}

		/// <summary>
		/// Sets the launcher status message.
		/// </summary>
		/// <param name='message'>
		/// Message.
		/// </param>
		public void SetStatusMessage (string message)
		{
			this.message.Text = message;
		}

		/// <summary>
		/// Resets the entry field colors.
		/// </summary>
		public void ResetEntryColors ()
		{
			if (container.Child == registrationContainer) {
				registrationEntryEmail.ModifyBase (StateType.Normal, WidgetFactory.COLOR_ENTRY_BASE);
				registrationEntryPassword1.ModifyBase (StateType.Normal, WidgetFactory.COLOR_ENTRY_BASE);
				registrationEntryPassword2.ModifyBase (StateType.Normal, WidgetFactory.COLOR_ENTRY_BASE);
			} else {
				loginEntryEmail.ModifyBase (StateType.Normal, WidgetFactory.COLOR_ENTRY_BASE);
				loginEntryPassword.ModifyBase (StateType.Normal, WidgetFactory.COLOR_ENTRY_BASE);
			}
		}

		/// <summary>
		/// Indicate login failure.
		/// </summary>
		/// <description>
		/// Changes the color of the login entry fields.
		/// </description>
		public void FailLogin ()
		{
			loginEntryEmail.ModifyBase (StateType.Normal, WidgetFactory.COLOR_ENTRY_BASE_ERROR);
			loginEntryPassword.ModifyBase (StateType.Normal, WidgetFactory.COLOR_ENTRY_BASE_ERROR);
		}

		/// <summary>
		/// Indicate that the registration form e-mail is invalid.
		/// </summary>
		/// <description>
		/// Changes the color of the registratiom e-mail entry field.
		/// </description>
		public void FailRegistrationEmail ()
		{
			registrationEntryEmail.ModifyBase (StateType.Normal, WidgetFactory.COLOR_ENTRY_BASE_ERROR);
		}

		/// <summary>
		/// Indicate that the registration form password is invalid.
		/// </summary>
		/// <description>
		/// Changes the color of the registratiom password entry field.
		/// </description>
		public void FailRegistrationPassword ()
		{
			registrationEntryPassword1.ModifyBase (StateType.Normal, WidgetFactory.COLOR_ENTRY_BASE_ERROR);
			registrationEntryPassword2.ModifyBase (StateType.Normal, WidgetFactory.COLOR_ENTRY_BASE_ERROR);
		}

		/// <summary>
		/// Indicate login success.
		/// </summary>
		/// <description>
		/// Changes the color of the login entry fields.
		/// </description>
		public void PassLogin ()
		{
			loginEntryEmail.ModifyBase (StateType.Normal, WidgetFactory.COLOR_ENTRY_BASE_FLASH);
			loginEntryPassword.ModifyBase (StateType.Normal, WidgetFactory.COLOR_ENTRY_BASE_FLASH);
			message.Text = "";
		}

		/// <summary>
		/// Indicate that the account registration passed.
		/// </summary>
		/// <description>
		/// Changes the color of the registration entry fields.
		/// </description>
		public void PassRegistration ()
		{
			loginEntryEmail.Text = registrationEntryEmail.Text;
			loginEntryPassword.Text = "";
			message.Text = "";
		}

		/// <summary>
		/// Shows the login screen.
		/// </summary>
		public void ShowLogin ()
		{
			if (container.Child == registrationContainer) {
				title.Text = "";
				message.Text = "";
				container.Remove (registrationContainer);
				container.Add (loginContainer);
				loginEntryEmail.GrabFocus ();
			}
		}

		/// <summary>
		/// Shows the registration screen.
		/// </summary>
		public void ShowRegistration ()
		{
			if (container.Child == loginContainer) {
				title.Text = "> NWO Account registration";
				message.Text = "";
				container.Remove (loginContainer);
				container.Add (registrationContainer);
				registrationEntryEmail.Text = loginEntryEmail.Text;
				registrationEntryEmail.GrabFocus ();
			}
		}

		private void keyHandler (object o, KeyReleaseEventArgs args)
		{
			if (container.Child != registrationContainer) {
				switch (args.Event.Key) {
				case Gdk.Key.Escape:
					actionEvent (UserEvent.LAUNCHER_QUIT);
					break;
				case Gdk.Key.Return:
					if (loginButtonShowRegistration.HasFocus)
						actionEvent (UserEvent.LAUNCHER_SHOW_REGISTRATION);
					else
						actionEvent (UserEvent.LAUNCHER_LOGIN);
					break;
				}
			} else {
				switch (args.Event.Key) {
				case Gdk.Key.Escape:
					actionEvent (UserEvent.LAUNCHER_SHOW_LOGIN);
					break;
				
				case Gdk.Key.Return:
					if (registrationButtonShowLogin.HasFocus)
						actionEvent (UserEvent.LAUNCHER_SHOW_LOGIN);
					else if (registrationButtonRegister.HasFocus)
						actionEvent (UserEvent.LAUNCHER_REGISTER);
					break;
				}
			}
		}
	}
}


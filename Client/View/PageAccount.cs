using System;

namespace Client.View
{
	using Gtk;
	using Gdk;

	/// <summary>
	/// The account view.
	/// </summary>
	/// <description>
	/// The account screen used to manage the account and account character.
	/// </description>
	public sealed class PageAccount : Page
	{
		private ActionEvent actionEvent;
		private Common.Account account;
		private Common.Character character;
		private Fixed background;
		private EventBox placeholderCreate;
		private EventBox placeholderDelete;
		private EventBox placeholderButtonDelete;
		private EventBox placeholderButtonPlay;
		private EventBox placeholderImageCharacter;
		private EventBox placeholderLabelCharacter;
		private Fixed containerCreate;
		private Fixed containerDelete;
		private Label labelMessage;
		private Gtk.Image imageCharacter;
		private Label labelCharacter;
		private EventBox buttonPlay;
		private EventBox buttonCreate;
		private EventBox buttonDelete;
		private EventBox buttonDeleteCancel;
		private EventBox buttonDeleteConfirm;
		private EventBox buttonLogout;
		private Entry entryCreate;
		private Entry entryDelete;

		/// <summary>
		/// Create new account view with no created character.
		/// </summary>
		/// <param name='actionEvent'>
		/// Action event handler.
		/// </param>
		/// <param name='account'>
		/// Account object of logged in player.
		/// </param>
		public PageAccount (ActionEvent actionEvent, Common.Account account)
		{
			this.actionEvent = actionEvent;
			this.account = account;
			this.character = null;

			createWidgets ();

			ShowDefault ();
		}

		/// <summary>
		/// Create new account view with a character.
		/// </summary>
		/// <param name='actionEvent'>
		/// Action event handler.
		/// </param>
		/// <param name='account'>
		/// Account object of logged in player.
		/// </param>
		/// <param name="character">
		/// Character object of logged in player.
		/// </param>
		public PageAccount (ActionEvent actionEvent, Common.Account account, Common.Character character)
		{
			this.actionEvent = actionEvent;
			this.account = account;
			this.character = character;

			createWidgets ();

			ShowDefault ();
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
		/// Gets the name of the character to be created.
		/// </summary>
		/// <returns>
		/// The character name.
		/// </returns>
		public string GetCreateCharacterName ()
		{
			return entryCreate.Text;
		}

		/// <summary>
		/// Gets the delete confirmation text.
		/// </summary>
		/// <returns>
		/// The delete confirmation text.
		/// </returns>
		public string GetDeleteConfirmation ()
		{
			return entryDelete.Text;
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
		/// Sets character to be displayed.
		/// </summary>
		/// <param name='character'>
		/// Character.
		/// </param>
		public void SetCharacter (Common.Character character)
		{
			this.character = character;

			clearPlaceholders ();

			placeholderImageCharacter.Remove (imageCharacter);
			placeholderLabelCharacter.Remove (labelCharacter);

			imageCharacter = WidgetFactory.CreateImage ("Graphics/avatar.png", 120, 120);
			if (character != null)
				labelCharacter = WidgetFactory.CreateLabelTitle (character.Name, true);
			else 
				labelCharacter = WidgetFactory.CreateLabelTitle ("", true);
			placeholderImageCharacter.Add (imageCharacter);
			placeholderLabelCharacter.Add (labelCharacter);
		}

		/// <summary>
		/// Sets the status message.
		/// </summary>
		/// <param name='message'>
		/// Message.
		/// </param>
		public void SetStatusMessage (string message)
		{
			this.labelMessage.Text = message;
		}

		/// <summary>
		/// Shows the default widgets.
		/// </summary>
		/// <description>
		/// The default widgets will be different depending on if the account has a character or not.
		/// </description>
		public void ShowDefault ()
		{
			clearPlaceholders ();
			if (character != null) {
				placeholderButtonDelete.Add (buttonDelete);
				placeholderButtonPlay.Add (buttonPlay);
				buttonPlay.GrabFocus ();
			} else {
				placeholderCreate.Add (containerCreate);
				entryCreate.GrabFocus ();
			}
		}

		/// <summary>
		/// Shows the delete confirmation widgets.
		/// </summary>
		public void ShowDelete ()
		{
			clearPlaceholders ();
			placeholderDelete.Add (containerDelete);
			entryDelete.GrabFocus ();
		}

		public void ShowDeleting ()
		{
			clearPlaceholders ();
		}

		private void clearPlaceholders ()
		{
			if (placeholderButtonDelete.Children.Length > 0)
				placeholderButtonDelete.Remove (buttonDelete);

			if (placeholderButtonPlay.Children.Length > 0)
				placeholderButtonPlay.Remove (buttonPlay);

			if (placeholderCreate.Children.Length > 0)
				placeholderCreate.Remove (containerCreate);

			if (placeholderDelete.Children.Length > 0)
				placeholderDelete.Remove (containerDelete);
		}

		private void createWidgets ()
		{
			background = new Fixed ();
			background.KeyReleaseEvent += keyHandler;
			background.SetSizeRequest (640, 480);
			{
				// Persistent widgets.
				background.Put (WidgetFactory.CreateImage ("Graphics/account_bg.png", 640, 480), 0, 0);
				background.Put (WidgetFactory.CreateLabel (account.Email, false), 10, 10);
				background.Put (labelMessage = WidgetFactory.CreateLabel ("", false), 20, 444);
				background.Put (buttonLogout = WidgetFactory.CreateButtonText ("Logout", 80, 30, true, actionEvent, UserEvent.ACCOUNT_QUIT), 10, 390);

				// Placeholder widgets.
				background.Put (placeholderCreate = WidgetFactory.CreatePlaceholder (360, 40), 160, 310);
				background.Put (placeholderDelete = WidgetFactory.CreatePlaceholder (360, 40), 160, 350);
				background.Put (placeholderButtonDelete = WidgetFactory.CreatePlaceholder (20, 20), 400, 160);
				background.Put (placeholderButtonPlay = WidgetFactory.CreatePlaceholder (80, 30), 550, 390);
				background.Put (placeholderImageCharacter = WidgetFactory.CreatePlaceholder (120, 120), 260, 170);
				background.Put (placeholderLabelCharacter = WidgetFactory.CreatePlaceholder (180, 30), 230, 310);

				// Placable widgets.
				buttonPlay = WidgetFactory.CreateButtonText ("Play", 80, 30, true, actionEvent, UserEvent.MAIN_SHOW);
				buttonDelete = WidgetFactory.CreateButtonImage ("Graphics/icon_s_remove.png", 20, 20, 0, actionEvent, UserEvent.ACCOUNT_SHOW_DELETE_CHARACTER);

				containerCreate = new Fixed ();
				containerCreate.SetSizeRequest (360, 30);
				{
					containerCreate.Put (entryCreate = WidgetFactory.CreateEntryField (180, 30), 70, 0);
					containerCreate.Put (buttonCreate = WidgetFactory.CreateButtonText ("Create", 60, 30, true, actionEvent, UserEvent.ACCOUNT_CREATE_CHARACTER), 260, 0);
				}

				containerDelete = new Fixed ();
				containerDelete.SetSizeRequest (360, 30);
				{
					containerDelete.Put (buttonDeleteCancel = WidgetFactory.CreateButtonText ("Cancel", 60, 30, true, actionEvent, UserEvent.ACCOUNT_SHOW_DEFAULT), 0, 0);
					containerDelete.Put (entryDelete = WidgetFactory.CreateEntryFieldRed (180, 30), 70, 0);
					containerDelete.Put (buttonDeleteConfirm = WidgetFactory.CreateButtonText ("Delete", 60, 30, true, actionEvent, UserEvent.ACCOUNT_DELETE_CHARACTER), 260, 0);
				}

				if (character != null) {
					placeholderLabelCharacter.Add (labelCharacter = WidgetFactory.CreateLabelTitle (character.Name, true, 180, 30));
					placeholderImageCharacter.Add (imageCharacter = WidgetFactory.CreateImage ("Graphics/avatar.png", 120, 120));
				} else {
					placeholderLabelCharacter.Add (labelCharacter = WidgetFactory.CreateLabelTitle ("", true, 180, 30));
					placeholderImageCharacter.Add (imageCharacter = WidgetFactory.CreateImage ("Graphics/avatar_shadow.png", 120, 120));
				}
			}
		}

		private void keyHandler (object o, KeyReleaseEventArgs args)
		{
			if (args.Event.Key == Gdk.Key.Return && buttonLogout.HasFocus) {
				actionEvent (UserEvent.ACCOUNT_QUIT);
			}

			if (placeholderDelete.Child == containerDelete) {
				switch (args.Event.Key) {
				case Gdk.Key.Escape:
					actionEvent (UserEvent.ACCOUNT_SHOW_DEFAULT);
					break;
				case Gdk.Key.Return:
					if (buttonDeleteCancel.HasFocus)
						actionEvent (UserEvent.ACCOUNT_SHOW_DEFAULT);
					else if (buttonDeleteConfirm.HasFocus)
						actionEvent (UserEvent.ACCOUNT_DELETE_CHARACTER);
					break;
				}
			} else {
				switch (args.Event.Key) {
				case Gdk.Key.Escape:
					actionEvent (UserEvent.ACCOUNT_QUIT);
					break;
				case Gdk.Key.Return:
					if (buttonDelete.HasFocus)
						actionEvent (UserEvent.ACCOUNT_SHOW_DELETE_CHARACTER);
					else if (buttonPlay.HasFocus)
						actionEvent (UserEvent.MAIN_SHOW);
					else if (buttonCreate.HasFocus)
						actionEvent (UserEvent.ACCOUNT_CREATE_CHARACTER);
					break;
				}
			}
		}
	}
}


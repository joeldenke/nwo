using System;
using System.Text.RegularExpressions;
using Common;

namespace Client.Controller
{
	public class PageHandlerAccount : PageHandler
	{

		public PageHandlerAccount (View.ViewManager view, Model.ModelManager modelManager) :
		base(view, modelManager)
		{

		}
		/// <summary>
		/// Sets the coresponding view.
		/// </summary>
		override internal void SetView ()
		{
			page = viewManager.LoadAccountPage (modelManager.GetAccountManager().GetMyAccount());
		}
		/// <summary>
		/// Executes the command.
		/// Messages from the view are sent here if they start with ACCOUNT_
		/// </summary>
		/// <param name='command'>
		/// Command.
		/// </param>
		override internal void HandleEvent (View.UserEvent userEvent)
		{
			View.PageAccount accountView = (View.PageAccount) page;
			switch (userEvent) {
			case View.UserEvent.ACCOUNT_CREATE_CHARACTER:
				createCharacter ();
				break;
			case View.UserEvent.ACCOUNT_DELETE_CHARACTER:
				deleteCharacter ();
				break;
			case View.UserEvent.ACCOUNT_PLAY:
				enterGame ();
				break;

			case View.UserEvent.ACCOUNT_QUIT:
				quit ();
				break;
			case View.UserEvent.ACCOUNT_SHOW_DEFAULT:
				accountView.ShowDefault ();
				if (!modelManager.GetAccountManager ().GetMyAccount ().HasCharacter)
					viewManager.SetStatusMessage ("Enter a name and create your character.");
				else 
					viewManager.SetStatusMessage ("");
				break;
			case View.UserEvent.ACCOUNT_SHOW_DELETE_CHARACTER:
				showDeleteCharacter ();
				break;
			}
		}

		override public void UpdatePage()
		{
			View.PageAccount accountView = (View.PageAccount) page;
			accountView.SetCharacter(modelManager.GetCharacterManager().GetMyCharacter());
			accountView.ShowDefault ();
		}

		/// <summary>
		/// Sends a message to the server, asking to create character.
		/// </summary>
		private void createCharacter ()
		{
			//Common.ClientCreateCharacterRequestMsg ccr = new Common.ClientCreateCharacterRequestMsg
			//new Character(accountView.GetCreateCharacterName());
			View.PageAccount accountView = (View.PageAccount) page;
			string name = accountView.GetCreateCharacterName ().Trim ();
			if (Regex.IsMatch (name, @"^([a-zA-Z ]{3,20})$")) {
				ClientCreateCharacterRequestMsg ccr = new 
				ClientCreateCharacterRequestMsg (accountView.GetCreateCharacterName ());
				try{
					modelManager.GetServerManager ().Send (ccr);
				} catch (NWOException ne){
					handleLostConnection(ne);
				}
				accountView.SetStatusMessage ("Confirming character creation ...");
			} else {
				accountView.SetStatusMessage ("Invalid character name. The name must contain between 3 and 20 latin letters.");
			}
		}
		/// <summary>
		/// Shows the delete character UI.
		/// </summary>
		private void showDeleteCharacter ()
		{
			View.PageAccount accountView = (View.PageAccount) page;
			accountView.ShowDelete();
			accountView.SetStatusMessage ("Please type \"" + modelManager.GetCharacterManager().GetMyCharacter().Name + "\" to confirm permanent character deletion.");
		}

		private void deleteCharacter ()
		{
			View.PageAccount accountView = (View.PageAccount) page;
			if (modelManager.GetCharacterManager ().GetMyCharacter ().Name.ToUpper () == accountView.GetDeleteConfirmation ().ToUpper ()) {

		
				ClientDeleteCharacterMsg cdl = new ClientDeleteCharacterMsg();//Delete character
				try{
					modelManager.GetServerManager ().Send (cdl);
				} catch (NWOException ne){

					Console.WriteLine("delete character send bug");
					handleLostConnection(ne);
				}
				modelManager.GetCharacterManager().SetMyCharacter(null);

				accountView.ShowDeleting ();
				accountView.SetCharacter(null);
				accountView.ShowDefault();
				accountView.SetStatusMessage ("Character deleted ...");
			} else {
				accountView.SetStatusMessage ("The name \"" + accountView.GetDeleteConfirmation () + "\" does not match your character name.");
			}
		}
		/// <summary>
		/// Quit this instance. Logout.
		/// </summary>
		private void quit ()
		{
			ClientDisconnectMsg dm = new ClientDisconnectMsg ("Disconnect-request from client.");
			try {
				modelManager.GetServerManager ().SendAndDisconnect (dm);
			} catch {
			}
			viewManager.LoadLauncherPage ();
		}

		private void enterGame ()
		{
			//viewManager.LoadCharacterPage (modelManager.GetCharacterManager().GetMyCharacter());
			viewManager.TriggerEvent(View.UserEvent.MAIN_SHOW);
		}

	}
}


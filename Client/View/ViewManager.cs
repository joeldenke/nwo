using System;
using Common;
using Client.View;

namespace Client.View
{
	using Gtk;
	using Gdk;
	using GLib;

	/// <summary>
	/// Action event delegate.
	/// </summary>
	/// <description>
	/// Delegate used for forwarding user action commands.
	/// </description>
	public delegate void ActionEvent (UserEvent userEvent);

	/// <summary>
	/// Manages the visual/audial representation of the application.
	/// </summary>
	public class ViewManager
	{
		private ActionEvent actionEvent;
		private Gtk.Window window;
		private EventBox screenPlaceholder;
		private Page activePage;
		private PageAccount pageAccount = null;
		private PageCharacter pageCharacter = null;
		private PageMap pageMap = null;
		private PageMain pageMain = null;
		/// <summary>
		/// Initializes a new instance of the <see cref="Client.View.ViewManager"/> class.
		/// </summary>
		/// <param name='actionEvent'>
		/// A reference to the action event handler which ought to manage user window interaction.
		/// </param>
		public ViewManager ()
		{
			Application.Init ();

			window = new Gtk.Window ("NWO");
			window.WindowPosition = WindowPosition.Center;
			try {
				window.SetIconFromFile ("Graphics/icon.png");
			} catch {
				throw new NWOException ("Image not found: Graphics/icon.png");
			}
			window.ModifyBg (StateType.Normal, WidgetFactory.COLOR_BLACK);
			window.ModifyBase (StateType.Normal, WidgetFactory.COLOR_BLACK);
			window.ExposeEvent += delegate {
				window.ShowAll ();
			};

			window.MapEvent += delegate {
				window.ShowAll ();
			};

			screenPlaceholder = new EventBox ();
			screenPlaceholder.BorderWidth = 0;
			screenPlaceholder.VisibleWindow = false;
			screenPlaceholder.SetSizeRequest (640, 480);
			window.Add (screenPlaceholder);
		}

		/// <summary>
		/// Sets the action event handler.
		/// </summary>
		/// <param name='actionEvent'>
		/// Action event handler.
		/// </param>
		public void SetActionEventHandler (ActionEvent actionEvent)
		{
			this.actionEvent = actionEvent;

			// Poll server once every second for updates.
			GLib.Timeout.Add (1000, new GLib.TimeoutHandler (pollServer));

			window.DeleteEvent += delegate {
				this.actionEvent (UserEvent.WINDOW_QUIT);
			};
		}

		public void SetStatusMessage (string message)
		{
			if (activePage != null)
				activePage.SetStatusMessage (message);
			else
				Console.WriteLine (message);
		}

		public PageMap LoadMapPage (TerrainMatrix terrainMatrix, MapTileClicked mapTileClicked)
		{
			if (!(activePage is PageMap)) {
				if (pageMap == null)
					pageMap = new PageMap (actionEvent, terrainMatrix, mapTileClicked);
				setActivePage (pageMap);
			}
			window.SetSizeRequest (800, 600);
			return (PageMap)activePage;
		}

		/// <summary>
		/// Loads the main page.
		/// </summary>
		/// <returns>
		/// The main page.
		/// </returns>
		/// <param name='character'>
		/// Character.
		/// </param>
		public PageMain LoadMainPage (Character character)
		{
			if (!(activePage is PageMain)) {
				if (pageMain == null)
					pageMain = new PageMain (actionEvent, character);
				setActivePage (pageMain);
			}

			window.SetSizeRequest (800,600);
			return (PageMain)activePage;
		}

		public PageMain GetMainPage ()
		{
			return pageMain;
		}

		/// <summary>
		/// Loads the launcher.
		/// </summary>
		/// <returns>
		/// The launcher.
		/// </returns>
		public PageLauncher LoadLauncherPage ()
		{
			if (!(activePage is PageLauncher)) {
				setActivePage (new PageLauncher (actionEvent));
			}

			pageAccount = null;
			pageCharacter = null;
			pageMain = null;
			pageMap = null;

			window.SetSizeRequest(640,480);
			return (PageLauncher)activePage;
		}

		/// <summary>
		/// Loads the account.
		/// </summary>
		/// <returns>
		/// The account.
		/// </returns>
		/// <param name='account'>
		/// Account.
		/// </param>
		public PageAccount LoadAccountPage (Common.Account account)
		{
			if (!(activePage is PageAccount)) {
				if (pageAccount == null)
					pageAccount = new PageAccount (actionEvent, account);
				setActivePage (pageAccount);
			}

			window.SetSizeRequest(640,480);
			return (PageAccount)activePage;
		}

		/// <summary>
		/// Loads the character screen.
		/// </summary>
		/// <returns>
		/// The character screen.
		/// </returns>
		/// <param name='character'>
		/// Character.
		/// </param>
		public PageCharacter LoadCharacterPage (Common.Character character)
		{
			if (!(activePage is PageCharacter)) {
				if (pageCharacter == null)
					pageCharacter = new PageCharacter (actionEvent, character);
				setActivePage (pageCharacter);
			}

			window.SetSizeRequest(800,600);
			return (PageCharacter)activePage;
		}

		public PageCharacter GetCharacterPage ()
		{
			return pageCharacter;
		}

		/// <summary>
		/// Begin window rendering.
		/// </summary>
		/// <description>
		/// This method keeps the executing thread until the game window shuts down.
		/// </description>
		public void Run ()
		{
			window.ShowAll ();
			Application.Run ();
		}

		/// <summary>
		/// Stop window rendering and destroy window context.
		/// </summary>
		public void Destroy ()
		{
			Application.Quit ();
		}

		/// <summary>
		/// Triggers a user event.
		/// </summary>
		/// <param name='userEvent'>
		/// User event.
		/// </param>
		public void TriggerEvent (UserEvent userEvent)
		{
			actionEvent (userEvent);
		}
	
		public Page GetActivePage ()
		{
			return activePage;
		}

		private void setActivePage (Page page)
		{
			if (window.GdkWindow != null) {
				if (page.GetWindowRequestResizable () == false) {
					if (window.GdkWindow.State.HasFlag (WindowState.Maximized))
						window.Unmaximize ();
				}
			}
			window.Resizable = page.GetWindowRequestResizable ();

			if (activePage != null)
				screenPlaceholder.Remove (activePage.GetContainer ());

			activePage = page;
			screenPlaceholder.Add (page.GetContainer ());
		}

		private bool pollServer ()
		{
			actionEvent (UserEvent.SERVER_POLL_DATA);
			return true;
		}
	}
}

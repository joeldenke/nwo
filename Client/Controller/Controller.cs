using System;
using System.Diagnostics;
using Common;
using System.Collections.Generic;

namespace Client.Controller
{
	public class Controller
	{
		private View.ViewManager viewManager;
		private Model.ModelManager modelManager;

		private List<Handler> handlers = new List<Handler>(); //list of controller objects
		
		public Controller (View.ViewManager viewManager, Model.ModelManager model)
		{
			this.viewManager = viewManager;
			this.modelManager = model;
		}
		/// <summary>
		/// Start this instance.
		/// Set the action event handler for the view, load launcher page
		/// and run().
		/// </summary>
		public void Start()
		{

			viewManager.SetActionEventHandler(handleActionEvent);
			//viewManager.LoadMainPage(new Character("Joel", new Position(0, 0)));
			//Run application window management.
			viewManager.LoadLauncherPage();
			viewManager.Run ();
		}
		/// <summary>
		/// Check if handler exists.
		/// </summary>
		/// <returns>
		/// The handler.
		/// </returns>
		/// <typeparam name='T'>
		/// The 1st type parameter.
		/// </typeparam>
		private Handler existingHandler <T>()
		{
			return handlers.Find(item => item is T);
		}
		/// <summary>
		/// Gets the handler if instantiated, if not, instantiate!
		/// Also run the method SetView(), if the contructor is not 
		/// executed, that is if the handler already exists in the list.
		/// </summary>
		/// <returns>
		/// The handler.
		/// </returns>
		/// <param name='args'>
		/// Arguments.
		/// </param>
		/// <typeparam name='T'>
		/// The 1st type parameter.
		/// </typeparam>
		private Handler getHandler<T> (params object[] args)
		{
			Handler h = existingHandler<T> ();

			if (!(h is Handler)) {
				h = (Handler)Activator.CreateInstance (typeof(T), args);
				handlers.Add (h);
			} else {
				if(h is PageHandler){
					PageHandler ph = (PageHandler) h;

					if(viewManager.GetActivePage() != ph.GetPage())
						ph.SetView ();
				}
			}

			return h;
		}

		/// <summary>
		/// Handle application user GUI interaction.
		/// </summary>
		/// <param name='command'>
		/// An indication of what action the user performed.
		/// </param>
		private void handleActionEvent (View.UserEvent command)
		{
			Console.WriteLine(command);
			List<Message> messagelist;
			Handler c = null;
			object[] args = new object[] { viewManager, modelManager };

			switch (command.ToString ().Split ("_".ToCharArray ()) [0]) {
			case "MAP": c = getHandler<PageHandlerMap>(args); break;
			case "MAIN": c = getHandler<PageHandlerMain>(args); break;
			case "ACCOUNT": c = getHandler<PageHandlerAccount>(args); break;
			case "LAUNCHER":
				c = getHandler<PageHandlerLauncher>(new object[] { viewManager, modelManager});
				break;
			
			case "WINDOW":
				c = getHandler<HandlerWindow>(args);
				break;

			case "SERVER"://userEvent from timer to check incoming messages
				try {
					messagelist = modelManager.GetServerManager().ReceiveAll();
					c = getHandler<HandlerServer>(new object[] {viewManager, modelManager, messagelist});
					HandlerServer cs = (HandlerServer) c;
					cs.SetList(messagelist);
				} catch (NWOException e) {
					viewManager.SetStatusMessage (e.Message);
					return;
				}
//				c = (ControllerServer) c;
//				c.SetList(messagelist);
				break;

			case "CHARACTER":
				c = getHandler<PageHandlerCharacterPage>(args);
				break;

			case "UPDATE":
				foreach( Handler handler in handlers){
					if( handler is PageHandler){
						PageHandler ph = (PageHandler) handler;
						if(ph.GetPage() == viewManager.GetActivePage())
						{
							ph.UpdatePage();
							return;
						}
					}
				}
				return;

			default:
				Debug.WriteLine("Unhandled action event: {0}", command);
				return;
			}

			c.HandleEvent(command);
		}
	}
}


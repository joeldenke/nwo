using System;
using System.Diagnostics;

using Client.Controller;
using Common;

namespace Client
{
	public class Client
	{
		public static void Main (string[] args)
		{
			string ip;
			int port = 0;
			Controller.Controller controller = null;

			if (args.Length != 1) {
				Console.WriteLine ("Defaulting to localhost.");
				ip = "127.0.0.1";
			} else {
				ip = args [0];
				if (args.Length == 2)
					port = Int32.Parse (args[1]);
			}
			if (port == 0)
				port = 2003;

			Console.WriteLine ("NWO Client startup ...");

			try {
				try {
					View.ViewManager view = new View.ViewManager ();
					Model.ModelManager model = new Model.ModelManager (ip, port);
					controller = new Controller.Controller (view, model);
				} catch (Exception e) {
					Debug.WriteLine (e);
					Environment.Exit (-1);
				}

				controller.Start ();
			} catch (Exception e) {
				Console.WriteLine ("Fatal runtime error: " + e.Message);
				Debug.WriteLine (e);
				Environment.Exit (-2);
			}

			Console.WriteLine ("NWO Client shutdown.");
			Environment.Exit (0);
		}
	}
}


using System;

namespace Client.View
{
	using Gtk;

	/// <summary>
	/// View abstraction.
	/// </summary>
	/// <description>
	/// All application views are extensions of this abstract class.
	/// </description>
	public interface Page
	{
		/// <summary>
		/// Gets the GTK# container representing the View.
		/// </summary>
		/// <returns>
		/// GTK# Widget container.
		/// </returns>
		Widget GetContainer ();

		/// <summary>
		/// Determines whether this screen desires the window to be resizable or not.
		/// </summary>
		/// <returns>
		/// <c>true</c> if the window is to be resizable; otherwise, <c>false</c>.
		/// </returns>
		bool GetWindowRequestResizable ();

		/// <summary>
		/// Sets the status message.
		/// </summary>
		/// <param name='message'>
		/// Message.
		/// </param>
		void SetStatusMessage (string message);

	}
}


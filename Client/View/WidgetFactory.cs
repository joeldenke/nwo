using System;
using Common;

namespace Client.View
{
	using Gdk;
	using Gtk;

	/// <summary>
	/// Widget factory.
	/// </summary>
	/// <description>
	/// Used for creating Gtk# widgets used in the different views.
	/// </description>
	public sealed class WidgetFactory
	{
		// Application theme colors.
		public static readonly Color COLOR_BLACK = new Color (0, 0, 0);
		public static readonly Color COLOR_BUTTON_BASE = new Color (115, 70, 100);
		public static readonly Color COLOR_BUTTON_BASE_ACTIVE = new Color (144, 88, 126);
		public static readonly Color COLOR_BUTTON_LABEL_FG = new Color (238, 164, 237);
		public static readonly Color COLOR_ICON_BG = new Color (39, 37, 56);
		public static readonly Color COLOR_ICON_BG_HOVER = new Color (51, 54, 75);
		public static readonly Color COLOR_ICON_BG_ACTIVE = new Color (69, 73, 92);
		public static readonly Color COLOR_ENTRY_BASE = new Color (69, 70, 100);
		public static readonly Color COLOR_ENTRY_BASE_ACTIVE = new Color (85, 88, 117);
		public static readonly Color COLOR_ENTRY_BASE_ERROR = new Color (120, 19, 59);
		public static readonly Color COLOR_ENTRY_BASE_FLASH = new Color (19, 120, 59);
		public static readonly Color COLOR_ENTRY_BASE_SELECTED = new Color (115, 119, 159);
		public static readonly Color COLOR_ENTRY_TEXT = new Color (255, 255, 255);
		public static readonly Color COLOR_ENTRY_RED_BASE = new Color (137, 37, 44);
		public static readonly Color COLOR_ENTRY_RED_BASE_ACTIVE = new Color (161, 41, 48);
		public static readonly Color COLOR_ENTRY_RED_BASE_ERROR = new Color (227, 42, 49);
		public static readonly Color COLOR_ENTRY_RED_BASE_FLASH = new Color (28, 213, 206);
		public static readonly Color COLOR_ENTRY_RED_BASE_SELECTED = new Color (176, 43, 51);
		public static readonly Color COLOR_ENTRY_RED_TEXT = new Color (255, 182, 77);
		public static readonly Color COLOR_LABEL_FG = new Color (190, 192, 236);

		public static EventBox CreateButtonImage (Gtk.Image image, uint borderWidth, ActionEvent actionEvent, UserEvent command)
		{
			EventBox button = new EventBox ();
			button.CanFocus = true;
			button.ModifyBg (StateType.Normal, COLOR_BUTTON_BASE);
			{
				EventBox border = new EventBox ();
				border.BorderWidth = borderWidth;
				border.VisibleWindow = false;
				{
					border.Add (image);
				}
				button.Add (border);
			}

			button.ButtonReleaseEvent += delegate {
				actionEvent (command);
				button.ModifyBg (StateType.Normal, COLOR_BUTTON_BASE);
			};

			return button;
		}

		/// <summary>
		/// Creates a button with an image.
		/// </summary>
		/// <returns>
		/// GTK# widget.
		/// </returns>
		/// <param name='path'>
		/// Path.
		/// </param>
		/// <param name='width'>
		/// Width.
		/// </param>
		/// <param name='height'>
		/// Height.
		/// </param>
		/// <param name='borderWidth'>
		/// Border width.
		/// </param>
		/// <param name='actionEvent'>
		/// Action event handler.
		/// </param>
		/// <param name='command'>
		/// Action command.
		/// </param>
		public static EventBox CreateButtonImage (string path, int width, int height, uint borderWidth, ActionEvent actionEvent, UserEvent command)
		{
			return CreateButtonImage(CreateImage(path , width, height), borderWidth, actionEvent, command);
		}

		/// <summary>
		/// Creates a button with text.
		/// </summary>
		/// <returns>
		/// GTK# widget.
		/// </returns>
		/// <param name='text'>
		/// Text.
		/// </param>
		/// <param name='centered'>
		/// Centered.
		/// </param>
		/// <param name='actionEvent'>
		/// Action event handler.
		/// </param>
		/// <param name='command'>
		/// Action command.
		/// </param>
		public static EventBox CreateButtonText (string text, bool centered, ActionEvent actionEvent, UserEvent command)
		{
			EventBox button = new EventBox ();
			button.CanFocus = true;
			button.ModifyBg (StateType.Normal, COLOR_BUTTON_BASE);
			{
				EventBox border = new EventBox ();
				border.BorderWidth = 4;
				border.VisibleWindow = false;
				{
					Label label = new Label (text);
					label.ModifyFg (StateType.Normal, COLOR_BUTTON_LABEL_FG);
					if (!centered)
						label.SetAlignment (0, 0);
					border.Add (label);
				}
				button.Add (border);
			}

			button.ButtonPressEvent += delegate {
				button.ModifyBg (StateType.Normal, COLOR_BUTTON_BASE_ACTIVE);
			};
			button.ButtonReleaseEvent += delegate {
				actionEvent (command);
				button.ModifyBg (StateType.Normal, COLOR_BUTTON_BASE);
			};
			button.FocusInEvent += delegate {
				button.ModifyBg (StateType.Normal, COLOR_BUTTON_BASE_ACTIVE);
			};
			button.FocusOutEvent += delegate {
				button.ModifyBg (StateType.Normal, COLOR_BUTTON_BASE);
			};

			return button;
		}

		/// <summary>
		/// Creates a button with text.
		/// </summary>
		/// <returns>
		/// GTK# widget.
		/// </returns>
		/// <param name='text'>
		/// Text.
		/// </param>
		/// <param name='width'>
		/// Width.
		/// </param>
		/// <param name='height'>
		/// Height.
		/// </param>
		/// <param name='centered'>
		/// Centered.
		/// </param>
		/// <param name='actionEvent'>
		/// Action event handler.
		/// </param>
		/// <param name='command'>
		/// Action command.
		/// </param>
		public static EventBox CreateButtonText (string text, int width, int height, bool centered, ActionEvent actionEvent, UserEvent command)
		{
			EventBox button = CreateButtonText (text, centered, actionEvent, command);
			button.SetSizeRequest (width, height);
			return button;
		}

		/// <summary>
		/// Creates an entry field.
		/// </summary>
		/// <returns>
		/// GTK# widget.
		/// </returns>
		public static Entry CreateEntryField ()
		{
			Entry entry = new Entry ();
			entry.HasFrame = false;
			entry.ModifyBase (StateType.Normal, COLOR_ENTRY_BASE);
			entry.ModifyBase (StateType.Selected, COLOR_ENTRY_BASE_SELECTED);
			entry.ModifyText (StateType.Normal, COLOR_ENTRY_TEXT);
			entry.FocusInEvent += delegate {
				entry.ModifyBase (StateType.Normal, COLOR_ENTRY_BASE_ACTIVE);
			};
			entry.FocusOutEvent += delegate {
				entry.ModifyBase (StateType.Normal, COLOR_ENTRY_BASE);
			};

			return entry;
		}

		/// <summary>
		/// Creates an entry field.
		/// </summary>
		/// <returns>
		/// GTK# widget.
		/// </returns>
		/// <param name='width'>
		/// Width.
		/// </param>
		/// <param name='height'>
		/// Height.
		/// </param>
		public static Entry CreateEntryField (int width, int height)
		{
			Entry entry = CreateEntryField ();
			entry.SetSizeRequest (width, height);
			return entry;
		}

		/// <summary>
		/// Creates a red entry field.
		/// </summary>
		/// <returns>
		/// GTK# widget.
		/// </returns>
		public static Entry CreateEntryFieldRed ()
		{
			Entry entry = new Entry ();
			entry.HasFrame = false;
			entry.ModifyBase (StateType.Normal, COLOR_ENTRY_RED_BASE);
			entry.ModifyBase (StateType.Selected, COLOR_ENTRY_RED_BASE_SELECTED);
			entry.ModifyText (StateType.Normal, COLOR_ENTRY_RED_TEXT);
			entry.FocusInEvent += delegate {
				entry.ModifyBase (StateType.Normal, COLOR_ENTRY_RED_BASE_ACTIVE);
			};
			entry.FocusOutEvent += delegate {
				entry.ModifyBase (StateType.Normal, COLOR_ENTRY_RED_BASE);
			};

			return entry;
		}

		/// <summary>
		/// Creates a red entry field.
		/// </summary>
		/// <returns>
		/// GTK# widget.
		/// </returns>
		/// <param name='width'>
		/// Width.
		/// </param>
		/// <param name='height'>
		/// Height.
		/// </param>
		public static Entry CreateEntryFieldRed (int width, int height)
		{
			Entry entry = CreateEntryFieldRed ();
			entry.SetSizeRequest (width, height);
			return entry;
		}

		/// <summary>
		/// Creates a password field.
		/// </summary>
		/// <returns>
		/// GTK# widget.
		/// </returns>
		public static Entry CreateEntryPassword ()
		{
			Entry entry = WidgetFactory.CreateEntryField ();
			entry.Visibility = false;
			return entry;
		}

		/// <summary>
		/// Creates an image widget.
		/// </summary>
		/// <returns>
		/// GTK# widget.
		/// </returns>
		/// <param name='path'>
		/// Image path.
		/// </param>
		/// <param name='width'>
		/// Width.
		/// </param>
		/// <param name='height'>
		/// Height.
		/// </param>
		public static Gtk.Image CreateImage (string path, int width, int height)
		{
			try {
				return new Gtk.Image (new Pixbuf (path).ScaleSimple (width, height, InterpType.Nearest));
			} catch {
				Console.WriteLine ("Error: Unable to load the image at '{0}'.", path);
				try {
					return new Gtk.Image (new Pixbuf (Colorspace.Rgb, false, 24, width, height));
				} catch {
					return null;
				}
			}
		}

		/// <summary>
		/// Creates a label.
		/// </summary>
		/// <returns>
		/// GTK# widget.
		/// </returns>
		/// <param name='text'>
		/// Text.
		/// </param>
		/// <param name='centered'>
		/// Centered.
		/// </param>
		public static Label CreateLabel (string text, bool centered)
		{
			Label label = new Label (text);
			label.ModifyFg (StateType.Normal, COLOR_LABEL_FG);
			if (!centered)
				label.SetAlignment (0, 0);
	
			return label;
		}

		/// <summary>
		/// Creates a label.
		/// </summary>
		/// <returns>
		/// GTK# widget.
		/// </returns>
		/// <param name='text'>
		/// Text.
		/// </param>
		/// <param name='centered'>
		/// Centered.
		/// </param>
		/// <param name='width'>
		/// Width.
		/// </param>
		/// <param name='height'>
		/// Height.
		/// </param>
		public static Label CreateLabel (string text, bool centered, int width, int height)
		{
			Label label = CreateLabel (text, centered);
			label.SetSizeRequest (width, height);
	
			return label;
		}

		/// <summary>
		/// Creates a title label.
		/// </summary>
		/// <returns>
		/// GTK# widget.
		/// </returns>
		/// <param name='text'>
		/// Text.
		/// </param>
		/// <param name='centered'>
		/// Centered.
		/// </param>
		public static Label CreateLabelTitle (string text, bool centered)
		{
			Label label = new Label ("<span size=\"18000\">" + text + "</span>");
			label.UseMarkup = true;
			label.ModifyFg (StateType.Normal, COLOR_LABEL_FG);
			if (!centered)
				label.SetAlignment (0, 0);
	
			return label;
		}

		/// <summary>
		/// Creates a title label.
		/// </summary>
		/// <returns>
		/// GTK# widget.
		/// </returns>
		/// <param name='text'>
		/// Text.
		/// </param>
		/// <param name='centered'>
		/// Centered.
		/// </param>
		/// <param name='width'>
		/// Width.
		/// </param>
		/// <param name='height'>
		/// Height.
		/// </param>
		public static Label CreateLabelTitle (string text, bool centered, int width, int height)
		{
			Label label = CreateLabelTitle (text, centered);
			label.SetSizeRequest (width, height);
	
			return label;
		}

		/// <summary>
		/// Creates a map tile. The tile will call the given delegate with the provided position when clicked.
		/// </summary>
		/// <returns>
		/// The map tile.
		/// </returns>
		/// <param name='texturePath'>
		/// Texture path.
		/// </param>
		/// <param name='position'>
		/// Position.
		/// </param>
		/// <param name='mapTileClicked'>
		/// Map tile click delegate.
		/// </param>
		public static EventBox CreateMapTile (Gtk.Image image, Position position, MapTileClicked mapTileClicked)
		{
			EventBox box = new EventBox ();
			box.VisibleWindow = false;
			box.Add (image);
			box.ModifyCursor (WidgetFactory.COLOR_LABEL_FG, WidgetFactory.COLOR_BUTTON_LABEL_FG);

			box.ButtonPressEvent += delegate {
				mapTileClicked(position);
			};

			return box;
		}

		/// <summary>
		/// Creates a widget placeholder.
		/// </summary>
		/// <returns>
		/// GTK# widget.
		/// </returns>
		/// <param name='width'>
		/// Width.
		/// </param>
		/// <param name='height'>
		/// Height.
		/// </param>
		public static EventBox CreatePlaceholder (int width, int height)
		{
			EventBox placeholder = new EventBox ();
			placeholder.VisibleWindow = false;
			placeholder.SetSizeRequest (width, height);
			return placeholder;
		}

		/// <summary>
		/// Creates a spacer.
		/// </summary>
		/// <returns>
		/// GTK# widget.
		/// </returns>
		/// <param name='size'>
		/// Size.
		/// </param>
		public static Widget CreateSpacer (uint size)
		{
			EventBox spacer = new EventBox ();
			spacer.VisibleWindow = false;
			spacer.BorderWidth = size;
			return spacer;
		}

		/// <summary>
		/// No instantiation allowed.
		/// </summary>
		private WidgetFactory ()
		{
		}
	}
}

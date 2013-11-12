using System;
using Common;

namespace Client.View
{
	using Gtk;
	using Gdk;

	public sealed class PageMain : Page
	{
		private ActionEvent actionEvent;
		private Common.Character character;

		private string lastRecvPm;

		// Widgets
		private VBox containerVbox; // Background Container
		
		private VBox leftVbox; // Containing exit button and actions bar
		private VBox centerVbox; // Containing character bar
		private VBox rightVbox; // Containing minimap
		private HBox upperHbox; // Containing left, center and right Vbox
		
		private HBox characterBox; // Holds character image and name
		private VBox actionsBox; // Holds actions label and buttons
		private VBox chatBox; // Holds chat widgets

		private Label message;
		private EventBox placeholderCharacter;
		private EventBox placeholderMinimap;
		private Gtk.Image imageCharacter;
		private Label labelCharacter;

		private Gtk.Image[] terrainImages;
		private Table mapTable;

		// Chat widgets
		private Entry entryChat;
		private TextView incomingChatMessagesTextView;
		private TextBuffer incomingChatMessagesTextBuffer;
		private EventBox sendButton;

		private bool isMapLoaded = false;

		public PageMain(ActionEvent actionEvent, Common.Character character)
		{
			this.actionEvent = actionEvent;
			this.character = character;

			terrainImages = new Gtk.Image[typeof(AreaTerrainType).GetEnumValues ().Length];

			initialize ();
		}

		public Widget GetContainer ()
		{
			return containerVbox;
		}

		/// <summary>
		/// Sets the status message.
		/// </summary>
		/// <param name='message'>
		/// Message.
		/// </param>
		public void SetStatusMessage (string message)
		{
			this.message.Text = message;
		}

		/// <summary>
		/// Gets the entry chat message.
		/// </summary>
		/// <returns>
		/// The entry chat message.
		/// </returns>
		public string GetEntryChatMessage ()
		{
			string msg = entryChat.Text;
			entryChat.Text = "";
			
			return msg;
		}

        /// <summary>
        /// Write a chat message (with time stamp and sender)
        /// </summary>
        /// <param name="name"></param>
        /// <param name="text"></param>
        public void AddChatMessage (string name, string text, Common.ChatMsg.ChatType chatType)
		{
			TextIter insertIter;
			switch (chatType) 
			{
			case ChatMsg.ChatType.PRIVATE:
				insertIter = incomingChatMessagesTextBuffer.EndIter;
				incomingChatMessagesTextBuffer.InsertWithTagsByName(ref insertIter, "[" +
				                                      DateTime.Now.ToString("HH:mm:ss") + "] " +
				                                                    name + " (PM): " + text + "\n", "redtext");
	
				// Check if the name is a "To <name>" response from the server.
				if(name.Substring( 0, 3 ) != "To ")
				{
					if(lastRecvPm == null)
						AddChatInfoMessage("Tip: Use the Down Arrow-key to reply to last pm.");

					lastRecvPm = name;
				}

				break;

			case ChatMsg.ChatType.PUBLIC:
				insertIter = incomingChatMessagesTextBuffer.EndIter;
				incomingChatMessagesTextBuffer.Insert(ref insertIter, "[" +
				                                      DateTime.Now.ToString("HH:mm:ss") + "] " +
				                                      name + ": " + text + "\n");
				break;
			}

			TextIter ti = incomingChatMessagesTextBuffer.GetIterAtLine(incomingChatMessagesTextBuffer.LineCount-1);
			TextMark tm = incomingChatMessagesTextBuffer.CreateMark("eot", ti,false);
			incomingChatMessagesTextView.ScrollToMark(tm, 0, false, 0, 0);
        }

		/// <summary>
		/// Write info message in chat
		/// </summary>
		/// <param name='text'>
		/// Text.
		/// </param>
		public void AddChatInfoMessage (string text)
		{
			TextIter insertIter = incomingChatMessagesTextBuffer.EndIter;
			incomingChatMessagesTextBuffer.Insert(ref insertIter, text + "\n");
			
			TextIter ti = incomingChatMessagesTextBuffer.GetIterAtLine(incomingChatMessagesTextBuffer.LineCount-1);
			TextMark tm = incomingChatMessagesTextBuffer.CreateMark("eot", ti,false);
			incomingChatMessagesTextView.ScrollToMark(tm, 0, false, 0, 0);
		}

		/// <summary>
		///  Determines whether this screen desires the window to be resizable or not. 
		/// </summary>
		/// <returns>
		/// The window request resizable.
		/// </returns>
		public bool GetWindowRequestResizable ()
		{
			return true;
		}

		public void ShowDefault ()
		{
		}

		/// <summary>
		/// Loads the map images to minimap.
		/// </summary>
		/// <param name='terrainMatrix'>
		/// Terrain matrix.
		/// </param>
		public void LoadMap (TerrainMatrix terrainMatrix)
		{
			if (isMapLoaded == false) {
				if (terrainMatrix != null) {
					AreaTerrainType[,] terrains = terrainMatrix.GetMatrix();
					for (uint x = 0; x < 10; x++) {
						for (uint y = 0; y < 10; y++) {
							
							int imageIndex = (int)terrains[x,y];
							Gtk.Image image = new Gtk.Image(terrainImages[imageIndex].Pixbuf);
							
							mapTable.Attach (image, x, x+1, y, y+1);
						}
					}
					isMapLoaded = true;
				}
			}
		}

		/// <summary>
		/// Loads the terrain images.
		/// </summary>
		private void loadTerrainImages ()
		{
			Array terrainTypes = Enum.GetValues (typeof(AreaTerrainType));
			for (int i = 0; i < terrainTypes.Length; i++) {
				terrainImages[i] = WidgetFactory.CreateImage ("Graphics/terrain_" + terrainTypes.GetValue(i).ToString().ToLower() + ".png", 15, 15);
			}
		}

		/// <summary>
		/// Initialize the widgets.
		/// </summary>
		private void initialize ()
		{
			labelCharacter = WidgetFactory.CreateLabelTitle (character.Name, true, 250, 40);
			imageCharacter = WidgetFactory.CreateImage ("Graphics/avatar.png", 40, 40);

			message = WidgetFactory.CreateLabel ("", false);
			mapTable = new Table(10, 10, true);

			// Character box
			placeholderCharacter = new EventBox();
			placeholderCharacter.VisibleWindow = false;
			placeholderCharacter.Visible = true;
			placeholderCharacter.KeyReleaseEvent += keyHandler;

			characterBox = new HBox(false, 0);
			characterBox.PackStart(imageCharacter, false, false, 0);
			characterBox.PackStart(labelCharacter, false, false, 0);
			characterBox.ModifyBg(StateType.Normal, WidgetFactory.COLOR_ENTRY_BASE);
			characterBox.Visible = true;

			characterBox.EnterNotifyEvent += delegate {
				characterBox.ModifyBg(StateType.Normal, WidgetFactory.COLOR_ICON_BG_ACTIVE);
			};
			characterBox.LeaveNotifyEvent += delegate {
				characterBox.ModifyBg(StateType.Normal, WidgetFactory.COLOR_ENTRY_BASE);
			};
			placeholderCharacter.ButtonReleaseEvent += delegate {
				actionEvent (View.UserEvent.CHARACTER_SHOW);
			};
			placeholderCharacter.CanFocus = true;
			placeholderCharacter.Add (characterBox);

			Frame characterFrame = new Frame();
			characterFrame.Add(placeholderCharacter);

			// Minimap
			loadTerrainImages();

			placeholderMinimap = new EventBox();
			placeholderMinimap.VisibleWindow = false;
			placeholderMinimap.Visible = true;
			placeholderMinimap.KeyReleaseEvent += keyHandler;

			placeholderMinimap.ButtonReleaseEvent += delegate {
				actionEvent (View.UserEvent.MAP_SHOW_DEFAULT);
			};
			placeholderMinimap.CanFocus = true;
			placeholderMinimap.Add (mapTable);

			Frame minimapFrame = new Frame();
			minimapFrame.Add (placeholderMinimap);

			// Actions
			actionsBox = new VBox(false, 5);
			actionsBox.BorderWidth = 5;
			actionsBox.PackStart(WidgetFactory.CreateButtonText("Hunt", true, this.actionEvent, UserEvent.MAIN_HUNT), false, false, 0);

			Frame actionsFrame = new Frame("Actions");
			actionsFrame.LabelXalign = (float)0.5;
			actionsFrame.LabelWidget.ModifyFg(Gtk.StateType.Normal, new Gdk.Color(255,255,255));
			actionsFrame.Add (actionsBox);

			// Init layout boxes
			containerVbox = new VBox(false, 5);
			upperHbox = new HBox(false, 5);

			leftVbox = new VBox(false, 5); 
			centerVbox = new VBox(false, 5);
			rightVbox = new VBox(false, 5);

			// Add stuff to leftVbox
			leftVbox.PackStart(WidgetFactory.CreateButtonText ("Exit", true, this.actionEvent, UserEvent.MAIN_QUIT), false, false, 0);
			leftVbox.PackStart(actionsFrame, true, true, 0);

			// Add stuff to centerVbox
			centerVbox.PackStart(characterFrame, false, false, 0);

			// Add stuff to rightVbox
			rightVbox.PackStart(minimapFrame, false, false, 0);

			// Add stuff to upperHbox
			upperHbox.PackStart(leftVbox, false, false, 0);
			upperHbox.PackStart(centerVbox, false, false, 0);
			upperHbox.PackEnd(rightVbox, false, false, 0);
		

			// Chat frame
			Frame chatFrame = new Frame("Chat");
			chatFrame.LabelXalign = (float)0.5;
			chatFrame.LabelWidget.ModifyFg(Gtk.StateType.Normal, new Gdk.Color(255,255,255));

			chatBox = new VBox(false, 5);
			chatBox.BorderWidth = 5;

			HBox entryBox = new HBox(false, 5); // box for Send button and text field

			entryChat = WidgetFactory.CreateEntryField(30,30);
			entryBox.PackStart(entryChat, true, true, 0);
		
			sendButton = WidgetFactory.CreateButtonText("Send", false, this.actionEvent, UserEvent.MAIN_SEND_CHAT);
			entryBox.PackStart(sendButton, false, false, 0);

			chatBox.PackEnd(entryBox, false, false, 0); // add entrybox to the far bottom

			incomingChatMessagesTextView = new TextView();
			incomingChatMessagesTextView.Editable = false;

			incomingChatMessagesTextView.ModifyBase (StateType.Normal, WidgetFactory.COLOR_ENTRY_BASE);
			incomingChatMessagesTextView.ModifyBase (StateType.Selected, WidgetFactory.COLOR_ENTRY_BASE_SELECTED);
			incomingChatMessagesTextView.ModifyText (StateType.Normal,WidgetFactory.COLOR_ENTRY_TEXT);
			incomingChatMessagesTextView.FocusInEvent += delegate {
				incomingChatMessagesTextView.ModifyBase (StateType.Normal, WidgetFactory.COLOR_ENTRY_BASE_ACTIVE);
			};
			incomingChatMessagesTextView.FocusOutEvent += delegate {
				incomingChatMessagesTextView.ModifyBase (StateType.Normal, WidgetFactory.COLOR_ENTRY_BASE);
			};

			incomingChatMessagesTextBuffer = incomingChatMessagesTextView.Buffer;
			incomingChatMessagesTextBuffer.Text = "";

			// text tags
			TextTag tagRedText  = new TextTag ("redtext");
			tagRedText.ForegroundGdk = new Gdk.Color(255, 200, 255);
			incomingChatMessagesTextBuffer.TagTable .Add (tagRedText); 

			ScrolledWindow chatWindow = new ScrolledWindow(); // makes the textview scrollable
			chatWindow.Add(incomingChatMessagesTextView); 

			chatBox.PackStart(chatWindow, true, true, 0); // add the textview to the top

			chatFrame.Add(chatBox);
		
			// Add stuff to outerVbox
			containerVbox.PackStart(upperHbox, true, true, 0);
			containerVbox.PackEnd(message, false, false, 5);
			containerVbox.PackEnd(chatFrame, true, true, 0);

			containerVbox.KeyReleaseEvent += keyHandler;
		}


		private void keyHandler (object o, KeyReleaseEventArgs args)
		{
			switch (args.Event.Key) {
				case Gdk.Key.Escape:
					actionEvent (UserEvent.MAIN_QUIT);
					break;
				case Gdk.Key.Return:
					if (entryChat.HasFocus || sendButton.HasFocus)
						actionEvent (UserEvent.MAIN_SEND_CHAT);
					break;
				case Gdk.Key.Down:
					entryChat.GrabFocus();	
					
					if(entryChat.Text.Length == 0)
					{
						if(lastRecvPm != null)
							entryChat.Text += "/pm " + lastRecvPm + " ";
						else
							entryChat.Text += "/pm ";
					}

					entryChat.Position = entryChat.Text.Length;

					break;
			}
		}
	}
}


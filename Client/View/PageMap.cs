using System;
using Common;
using System.Collections.Generic;

namespace Client.View
{
	using Gtk;
	using Gdk;

	/// <summary>
	/// Map tile clicked.
	/// </summary>
	public delegate void MapTileClicked(Position Position);

	public sealed class PageMap : Page
	{
		private ActionEvent actionEvent;
		private MapTileClicked mapTileClicked;

		// Widgets
		private VBox outerVbox;
		private HBox innerHbox;

		private Frame mapFrame;
		private Frame mapViewFrame;
		private Frame descriptionFrame;
		private Frame tileFrame;

		private Fixed mapBackground;

		private Gtk.Image mapDescriptionImage;
		private Gtk.Image[] terrainImages;
		private Gtk.Image playerPositionIndicator;

		private EventBox travelButton;
		private Table mapTable;
		private Table descriptionTable;

		private Label message;
		private Label terrainLabel;
		private Label noCharactersLabel;
		private Label areaDescriptionLabel;
		private bool isMapLoaded = false;

		public PageMap(ActionEvent actionEvent, TerrainMatrix terrainMatrix, MapTileClicked mapTileClicked)
		{
			this.actionEvent = actionEvent;
			this.mapTileClicked = mapTileClicked;

			terrainImages = new Gtk.Image[typeof(AreaTerrainType).GetEnumValues ().Length];
			playerPositionIndicator = WidgetFactory.CreateImage("Graphics/player_position.png", 32, 32);

			initialize (terrainMatrix);
			ShowDefault();
		}

		// Need to copy the buffer before create new Image to store the image for mass usage
		private Gtk.Image copyImageBuffer (Pixbuf buffer)
		{
			return new Gtk.Image (buffer.Copy ());
		}

		public void LoadMap (TerrainMatrix terrainMatrix)
		{
			if (isMapLoaded == false) {
				if (terrainMatrix != null) {
					AreaTerrainType[,] terrains = terrainMatrix.GetMatrix();
					for (uint x = 0; x < 10; x++) {
						for (uint y = 0; y < 10; y++) {

							int imageIndex = (int)terrains[x,y];
							Gtk.Image image = new Gtk.Image(terrainImages[imageIndex].Pixbuf);

							Position position = new Position((int)x, (int)y);

							EventBox tile = WidgetFactory.CreateMapTile (image, position, mapTileClicked);
							mapTable.Attach (tile, x, x+1, y, y+1);
						}
					}
					isMapLoaded = true;
				}
			}
		}

		private void initialize (TerrainMatrix terrainMatrix)
		{
			outerVbox = new VBox(false, 5);
			innerHbox = new HBox(false, 5);

			mapBackground = new Fixed();

			// Frames
			mapFrame = new Frame("Map");
			mapViewFrame = new Frame("Map view");
			descriptionFrame = new Frame("Description");

			mapViewFrame.LabelXalign = (float)0.5;
			mapViewFrame.LabelWidget = WidgetFactory.CreateLabelTitle("Map view", true, 180, 30);
			mapViewFrame.LabelWidget.ModifyFg(Gtk.StateType.Normal, new Gdk.Color(255,255,255));
			mapViewFrame.BorderWidth = 20;

			mapFrame.LabelXalign = (float)0.5;
			mapFrame.LabelWidget.ModifyFg(Gtk.StateType.Normal, new Gdk.Color(255,255,255));
			mapFrame.BorderWidth = 2;

			descriptionFrame.LabelXalign = (float)0.5;
			descriptionFrame.LabelWidget.ModifyFg(Gtk.StateType.Normal, new Gdk.Color(255,255,255));
			descriptionFrame.BorderWidth = 2;

			// Tables
			descriptionTable = new Table(5, 1, false);
			message = WidgetFactory.CreateLabel ("", false);

			mapTable = new Table (10, 10, true);
			mapTable.KeyReleaseEvent += keyHandler;

			EventBox exitButton = WidgetFactory.CreateButtonText ("Main view", true, this.actionEvent, UserEvent.MAIN_SHOW);
			exitButton.SetSizeRequest (60, 30);

			loadTerrainTiles ();

			// Add stuff to inner frames
			mapBackground.Put (mapTable, 0, 0);
			mapBackground.Put (playerPositionIndicator, 0, 0);
			mapFrame.Add (mapBackground);
			descriptionFrame.Add (descriptionTable);

			// Add frames to innerhbox
			innerHbox.PackStart(mapFrame, false, false, 0);
			innerHbox.PackStart(descriptionFrame, true, true, 0);
			 
			// Add innerhbox to outer frame
			mapViewFrame.Add (innerHbox);

			// Add stuff to vbox
			outerVbox.PackStart(mapViewFrame, true, false, 0);
			outerVbox.PackStart(exitButton, false, true, 0);
			outerVbox.PackEnd(message, false, false, 5);

		}

		public Widget GetContainer ()
		{
			return outerVbox;
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
			message.Text = "";
		}

		public void ShowTileInfo (Area area)
		{
			if (travelButton == null) {
				travelButton = WidgetFactory.CreateButtonText ("Travel", false, actionEvent,
				                                               UserEvent.MAP_TRAVEL);
				descriptionTable.Attach (travelButton, 0, 1, 4, 5);
			}

			if (terrainLabel == null) {
				terrainLabel = WidgetFactory.CreateLabel ("Terrain: " + area.TerrainType.ToString (), false);
				descriptionTable.Attach (terrainLabel, 0, 1, 1, 2);
			} else {
				terrainLabel.Text = area.TerrainType.ToString ();
			}

			if (noCharactersLabel == null) {
				noCharactersLabel = WidgetFactory.CreateLabel ("No. players: " + area.Characters.Count, false);
				descriptionTable.Attach (noCharactersLabel, 0, 1, 2, 3);
			} else {
				noCharactersLabel.Text = "No. players: " + area.Characters.Count;
			}

			if (mapDescriptionImage == null) {
				tileFrame = new Frame();
				tileFrame.BorderWidth = 5;
				mapDescriptionImage = WidgetFactory.CreateImage ("Graphics/terrain_" + area.TerrainType.ToString ().ToLower () + ".png", 200, 200);
				tileFrame.Add(mapDescriptionImage);
				descriptionTable.Attach (tileFrame, 0, 1, 0, 1);
			} else {
				tileFrame.Remove(mapDescriptionImage);
				mapDescriptionImage = WidgetFactory.CreateImage ("Graphics/terrain_" + area.TerrainType.ToString ().ToLower () + ".png", 200, 200);
				tileFrame.Add(mapDescriptionImage);
				descriptionTable.Attach(tileFrame, 0, 1, 0, 1);
			}

			if (areaDescriptionLabel == null) {
				/* TODO:
				 * LATER IMPL: areaDescriptionLabel = WidgetFactory.CreateLabel ("Description: \n" + area.GetDescription(), false);
				 */
				areaDescriptionLabel = WidgetFactory.CreateLabel ("Description: \n" + "-", false);
				descriptionTable.Attach (areaDescriptionLabel, 0, 1, 3, 4);
			} else {
				areaDescriptionLabel.Text = "Description: \n -";
			}

			SetStatusMessage ("(" + area.Position.Y + ", " + area.Position.X + ")");
			descriptionTable.ShowAll();
		}

		public void SetPlayerPosition (Position position)
		{
			mapBackground.Move (playerPositionIndicator, position.X*32, position.Y*32);
		}

		public void SetStatusMessage (string message)
		{
			this.message.Text = message;
		}
	
		private void loadTerrainTiles ()
		{
			Array terrainTypes = Enum.GetValues (typeof(AreaTerrainType));
			for (int i = 0; i < terrainTypes.Length; i++) {
				terrainImages[i] = WidgetFactory.CreateImage ("Graphics/terrain_" + terrainTypes.GetValue(i).ToString().ToLower() + ".png", 32, 32);
			}
		}

		private void keyHandler (object o, KeyReleaseEventArgs args)
		{
			switch (args.Event.Key) {
				case Gdk.Key.Escape:
					actionEvent (UserEvent.MAIN_SHOW);
					break;
				case Gdk.Key.Return: break;
			}
		}
	}
}


using System;
using Client.View;
using Common;

namespace Client.Controller
{
	public class PageHandlerMap : PageHandler
	{
		private PageMap mapView;
		private TerrainMatrix terrainMatrix;

		public PageHandlerMap (View.ViewManager view, Model.ModelManager modelManager) :
		base(view, modelManager)
		{

		}

		override internal void SetView ()
		{
			terrainMatrix = modelManager.GetWorldManager ().GetTerrainMatrix ();
			mapView = viewManager.LoadMapPage (terrainMatrix, handleMapTileClicked);
			mapView.LoadMap(terrainMatrix);
		}

		override internal void HandleEvent (View.UserEvent userEvent)
		{
			try {
				switch (userEvent) {
				case View.UserEvent.MAP_SHOW_DEFAULT:
					mapView.ShowDefault ();
					mapView.SetPlayerPosition (modelManager.GetWorldManager ().GetCurrentArea ().Position);
					break;
				case View.UserEvent.MAP_CONTEXT:
					mapView.ShowTileInfo (modelManager.GetWorldManager ().GetFocusedArea ());
					break;
				case View.UserEvent.MAP_TRAVEL:
					try {
						modelManager.GetServerManager ().Send (
								new ClientMoveCharacterMsg (modelManager.GetWorldManager ().GetFocusedArea ().Position)
						);
					} catch {
						Console.WriteLine ("Could not send move character to server");
					}
					break;
				case View.UserEvent.MAP_MOVE_CHARACTER:
					UpdatePage ();
					mapView.SetStatusMessage ("Moved character to position" +
						modelManager.GetWorldManager ().GetCurrentArea ().Position.Y + ", " + modelManager.GetWorldManager ().GetCurrentArea ().Position.X
					);
					break;
				}
			} catch (Exception e) {
				Console.WriteLine(e.Message);
			}
		}

		override public void UpdatePage()
		{
			mapView.SetPlayerPosition (modelManager.GetWorldManager().GetCurrentArea().Position);
			mapView.LoadMap(modelManager.GetWorldManager().GetTerrainMatrix ());
		}
		/// <summary>
		/// Checks if the position exists.
		/// </summary>
		/// <returns>
		/// The exists.
		/// </returns>
		/// <param name='position'>
		/// If set to <c>true</c> position.
		/// </param>
		private bool PositionExists(Position position)
		{
			Area current = modelManager.GetWorldManager().GetCurrentArea();
			Area focused = modelManager.GetWorldManager().GetFocusedArea();

			if (current == null || focused == null) return false;

			return ((position.X == current.Position.X && position.Y == current.Position.Y) || 
			        (position.X == focused.Position.X && position.Y == focused.Position.Y));
		}
		/// <summary>
		/// Handles the map tile clicked.
		/// </summary>
		/// <param name='position'>
		/// Position.
		/// </param>
		private void handleMapTileClicked (Position position)
		{
			if (!PositionExists(position)) {
				try {
					modelManager.GetServerManager ().Send (new ClientAreaInfoRequestMsg (position));
				} catch {
					Console.WriteLine ("Could not send area info request to server");
				}
			}
		}
	}
}


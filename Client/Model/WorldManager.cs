using System;
using Common;

namespace Client.Model
{
	/// <summary>
	/// World manager.
	/// </summary>
	/// <description>
	/// Manages the area in which the player character currently is located, and the appearence of the world
	/// surrounding the area.
	/// </description>
	public class WorldManager
	{
		private Area focusedArea;
		private Area currentArea;
		private TerrainMatrix terrainMatrix;

		public Area GetFocusedArea ()
		{
			return focusedArea;
		}

		/// <summary>
		/// Gets the current character area.
		/// </summary>
		/// <returns>
		/// The current area.
		/// </returns>
		public Area GetCurrentArea ()
		{
			return currentArea;
		}

		/// <summary>
		/// Gets the terrain matrix, which is a representation of the appearence of the world map.
		/// </summary>
		/// <returns>
		/// The terrain matrix.
		/// </returns>
		public TerrainMatrix GetTerrainMatrix ()
		{
			return terrainMatrix;
		}

		public void SetFocusedArea (Area area)
		{
			this.focusedArea = area;
		}

		/// <summary>
		/// Sets the current character area.
		/// </summary>
		/// <param name='area'>
		/// Area.
		/// </param>
		public void SetCurrentArea (Area area)
		{
			this.currentArea = area;
		}

		/// <summary>
		/// Sets the terrain matrix.
		/// </summary>
		/// <param name='terrainMatrix'>
		/// Terrain matrix.
		/// </param>
		public void SetTerrainMatrix (TerrainMatrix terrainMatrix)
		{
			this.terrainMatrix = terrainMatrix;
		}
	}
}


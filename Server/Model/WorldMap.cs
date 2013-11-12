using System;
using System.Diagnostics;
using Common;
using MongoDB.Bson;

namespace Server.Model
{
	/// <summary>
	/// World map.
	/// </summary>
	public class WorldMap
	{
		public ObjectId _id { get; set; }
		public Area[,] Areas { get; set; }
		public int Width { get; set; }
		public int Height { get; set; }
		public Position DefaultStartLocation { get; set; }

		private static Random random = new Random ();

		/// <summary>
		/// Initializes a new instance of the <see cref="Server.WorldMap"/> class and generates a .
		/// </summary>
		/// <param name='width'>
		/// Width.
		/// </param>
		/// <param name='height'>
		/// Height.
		/// </param>
		public WorldMap (int width, int height)
		{
			Areas = new Area[width, height];
			Width = width;
			Height = height;

			for (int x = 0; x < width; x++)
				for (int y = 0; y < height; y++)
					Areas [x, y] = new Area (new Position (x, y), getRandomTerrainType ());

			this.Width = width;
			this.Height = height;

			DefaultStartLocation = new Position (5,5);
		}

		/// <summary>
		/// Gets the area at the given position.
		/// </summary>
		/// <returns>
		/// The area.
		/// </returns>
		/// <param name='position'>
		/// Position.
		/// </param>
		public Area GetArea (Position position)
		{
			return Areas [position.X, position.Y];
		}

		/// <summary>
		/// Generates a terrain matrix.
		/// </summary>
		/// <returns>
		/// A terrain matrix.
		/// </returns>
		public TerrainMatrix GenerateTerrainMatrix ()
		{
			AreaTerrainType[,] terrain = new AreaTerrainType[Width, Height];

			for (int x = 0; x < Width; x++)
				for (int y = 0; y < Height; y++)
					terrain [x, y] = Areas [x, y].TerrainType;

			return new TerrainMatrix (terrain, Width, Height);
		}

		/// <summary>
		/// Moves given character to target position in world map.
		/// </summary>
		/// <param name='character'>
		/// Character.
		/// </param>
		/// <param name='position'>
		/// Position.
		/// </param>
		/// <exception cref="NWOException">
		/// If the given position is'nt valid, or there was some other problem. In case a problem is detected,
		/// the method will try to rollback any changes made to the character current area.
		/// </exception>
		public void MoveCharacterToPosition (ServerCharacter character, Position position)
		{
			if (position.X == character.Position.X && position.Y == character.Position.Y)
				throw new NWOException ("Cannot move character to same position as it already occure in!");

			try {
				GetArea (character.Position).Characters.Remove (character.GetCharacter());
			
			} catch (Exception e) {
				Debug.WriteLine (e);
				throw new NWOException ("Unable to move character from area.");
			}

			try {
				GetArea (position).Characters.Add (character.GetCharacter());

			} catch (Exception e) {
				Debug.WriteLine (e);
				GetArea (character.Position).Characters.Add (character.GetCharacter());
				throw new NWOException ("Unable to move character to area.");
			}

			character.Position = position;
		}

		private static AreaTerrainType getRandomTerrainType ()
		{
			Array values = Enum.GetValues (typeof(AreaTerrainType));
			return (AreaTerrainType)values.GetValue (random.Next (values.Length));
		}
	}
}


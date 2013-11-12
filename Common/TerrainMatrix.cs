using System;
using System.Runtime.Serialization;

namespace Common
{
	/// <summary>
	/// Terrain matrix.
	/// </summary>
	[Serializable]
	public class TerrainMatrix : ISerializable
	{
		private AreaTerrainType[,] terrain;
		private int width, height;

		/// <summary>
		/// Initializes a new instance of the <see cref="Common.TerrainMatrix"/> class.
		/// </summary>
		/// <param name='terrain'>
		/// Matrix of terrain types.
		/// </param>
		public TerrainMatrix (AreaTerrainType[,] terrain, int width, int height)
		{
			this.terrain = terrain;
			this.width = width;
			this.height = height;
		}

		/// <summary>
		/// Serialization constructor.
		/// </summary>
		public TerrainMatrix (SerializationInfo info, StreamingContext ctxt)
		{
			terrain = (AreaTerrainType[,]) info.GetValue("terrain", typeof(AreaTerrainType[,]));
			width = info.GetInt32("width");
			height = info.GetInt32("height");
		}

		/// <summary>
		/// Gets the matrix.
		/// </summary>
		/// <returns>
		/// The matrix.
		/// </returns>
		public AreaTerrainType[,] GetMatrix()
		{
			return terrain;
		}

		/// <summary>
		/// Gets the width.
		/// </summary>
		/// <returns>
		/// The width.
		/// </returns>
		public int GetWidth ()
		{
			return width;
		}

		/// <summary>
		/// Gets the height.
		/// </summary>
		/// <returns>
		/// The height.
		/// </returns>
		public int GetHeight()
		{
			return height;
		}

		/// <summary>
		/// Deserialization method.
		/// </summary>
		public void GetObjectData(SerializationInfo info, StreamingContext ctxt)
		{
			info.AddValue("terrain", terrain);
			info.AddValue("width", width);
			info.AddValue("height", height);
		}
	}
}


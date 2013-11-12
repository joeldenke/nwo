using System;

namespace Common
{
	/// <summary>
	/// Server terrain matrix message.
	/// </summary>
	public class ServerTerrainMatrixMsg : Message
	{
		private TerrainMatrix terrainMatrix;

		/// <summary>
		/// Initializes a new instance of the <see cref="Common.ServerTerrainMatrixMsg"/> class.
		/// </summary>
		/// <param name='terrainMatrix'>
		/// Terrain matrix.
		/// </param>
		public ServerTerrainMatrixMsg (TerrainMatrix terrainMatrix)
		{
            this.terrainMatrix = terrainMatrix;
		}

		/// <summary>
		/// Gets the terrain matrix.
		/// </summary>
		/// <returns>
		/// The terrain matrix.
		/// </returns>
		public TerrainMatrix GetTerrainMatrix ()
		{
			return terrainMatrix;
		}

		/// <summary>
		/// Gets the type of the message.
		/// </summary>
		/// <returns>
		/// The message type.
		/// </returns>
        public override Type GetMsgType()
        {
            return Message.Type.STM;
        }
	}
}


using System;
using System.Collections.Generic;
using Common;

namespace Server.Model
{
	public class GameWorld
	{
        public CharacterManager characterManager;
		private WorldMap worldMap;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="characters"></param>
		public GameWorld (List<ServerCharacter> characters, WorldMap worldMap)
		{
            this.characterManager = new CharacterManager(characters);
			this.worldMap = worldMap;
        }

		/// <summary>
		/// Gets the world map.
		/// </summary>
		/// <returns>
		/// The world map.
		/// </returns>
		public WorldMap GetWorldMap ()
		{
			return worldMap;
		}
	}
}


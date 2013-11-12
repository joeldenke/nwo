using System;
using Common;

namespace Client.Model
{
	public class CharacterManager
	{
		private Character myCharacter;

		public CharacterManager ()
		{
		}
		/// <summary>
		/// Sets my character.
		/// </summary>
		/// <param name='character'>
		/// Character.
		/// </param>
		public void SetMyCharacter(Character character)
		{
			myCharacter = character;
		}
		/// <summary>
		/// Gets my character.
		/// </summary>
		/// <returns>
		/// The my character.
		/// </returns>
		public Character GetMyCharacter ()
		{
			return myCharacter;
		}
	}
}


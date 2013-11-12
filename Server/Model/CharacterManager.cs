using System;
using System.Collections.Generic;

namespace Server.Model
{
    public class CharacterManager
    {
        private List<ServerCharacter> characters;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="characters"></param>
        public CharacterManager(List<ServerCharacter> characters)
        {
            this.characters = characters;
        }

        /// <summary>
        /// Get the character list
        /// </summary>
        /// <returns></returns>
        public List<ServerCharacter> GetCharacters()
        {
            return characters;
        }

        /// <summary>
        /// Adds the character.
        /// </summary>
        /// <returns>
        /// <c>true</c>, if character was added, <c>false</c> otherwise.
        /// </returns>
        /// <param name='character'>
        /// Player char.
        /// </param>
        public bool AddCharacter(ServerCharacter character)
        {
            if (GetCharacterFromName(character.Name) == null)
            {
                characters.Add(character);
                return true;
            }
            return false;
        }

        /// <summary>
        /// Gets the character instance by name.
        /// </summary>
        /// <returns>
        /// The character.
        /// </returns>
        /// <param name='name'>
        /// Name.
        /// </param>
        public ServerCharacter GetCharacterFromName(string name)
        {
            foreach (ServerCharacter c in characters)
            {

                if (c.Name.ToLower() == name.ToLower())
                    return c;
            }
            return null;
        }

        /// <summary>
        /// Gets the character instance from account identifier.
        /// </summary>
        /// <returns>
        /// The character from account identifier.
        /// </returns>
        /// <param name='accountId'>
        /// Account identifier.
        /// </param>
        public ServerCharacter GetCharacterFromAccountId(string accountId)
        {
            foreach (ServerCharacter c in characters)
            {

                if (c.GetAccountId() == accountId)
                    return c;
            }
            return null;
        }

        /// <summary>
        /// Delete a character
        /// </summary>
        /// <param name="c">
        /// The character to delete
        /// </param>
        /// <returns>
        /// True if character is deleted,
        /// False if character does not exist
        /// </returns>
        public bool DeleteCharacter(string accountId)
        {
            return characters.Remove(GetCharacterFromAccountId(accountId));
        }
    }
}

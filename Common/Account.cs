using System;
using System.Runtime.Serialization;

namespace Common
{
	[Serializable]
	public class Account : ISerializable
	{
		public string Email;
		public bool HasCharacter;

		/// <summary>
		/// Initializes a new instance of the <see cref="Common.Account"/> class.
		/// </summary>
		/// <param name='email'>
		/// Email.
		/// </param>
		/// <param name='hasCharacter'>
		/// Has character.
		/// </param>
		public Account (string email, bool hasCharacter)
		{
			Email = email;
			HasCharacter = hasCharacter;
		}

		/// <summary>
		/// Serialization constructor.
		/// </summary>
		public Account (SerializationInfo info, StreamingContext ctxt)
		{
			Email = (String) info.GetValue("Email", typeof(string));
			HasCharacter = (bool) info.GetValue("HasCharacter", typeof(bool));
		}
		
		/// <summary>
		/// Deserialization method.
		/// </summary>
		public void GetObjectData(SerializationInfo info, StreamingContext ctxt)
		{
			info.AddValue("Email", Email);
			info.AddValue("HasCharacter", HasCharacter);
		}
	}
}

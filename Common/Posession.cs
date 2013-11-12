using System;
using System.Runtime.Serialization;

namespace Common
{
	[Serializable]
	public class Posession
	{
		public Posession ()
		{
		}

		/// <summary>
		/// Serialization constructor.
		/// </summary>
		public Posession (SerializationInfo info, StreamingContext ctxt)
		{
		}
		
		/// <summary>
		/// Deserialization method.
		/// </summary>
		public void GetObjectData(SerializationInfo info, StreamingContext ctxt)
		{
		}
	}
}


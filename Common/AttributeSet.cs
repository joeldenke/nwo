using System;
using System.Runtime.Serialization;

namespace Common
{

	public enum AttributeType {
		STRENGTH = 0, 
		ENDURANCE = 1, 
		AGILITY = 2, 
		INTELLIGENCE = 3, 
		PERCEPTION = 4, 
		WILLPOWER = 5
	};

	[Serializable]
	public class AttributeSet : ISerializable
	{
		public int[] Attributes;

		public AttributeSet ()
		{
			Attributes = new int[ Enum.GetNames(typeof(AttributeType)).Length ];

			for(int i = 0; i<Attributes.Length; i++)
				Attributes[i] = 1;
		}

		/// <summary>
		/// Serialization constructor.
		/// </summary>
		public AttributeSet (SerializationInfo info, StreamingContext ctxt)
		{
			Attributes = (int[]) info.GetValue("Attributes", typeof(int[]));
		}

		/// <summary>
		/// Deserialization method.
		/// </summary>
		public void GetObjectData(SerializationInfo info, StreamingContext ctxt)
		{
			info.AddValue("Attributes", Attributes);
		}
	}
}


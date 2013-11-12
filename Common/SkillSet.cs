using System;
using System.Runtime.Serialization;

namespace Common
{
	public enum SkillType 
	{
		HUNTING = 0,
		FARMING = 1, 
		WOODCUTTING = 2, 
		MINING = 3,
		MAGIC = 4, 
		MELEE = 5, 
		RANGED = 6, 
		STEALTH = 7, 
		DIPLOMACY = 8,
		CRAFTING = 9,
		BUILDING = 10, 
		PROPHECY = 11
	};


	[Serializable]
	public class SkillSet : ISerializable
	{
		public int[] Skills;

		public SkillSet ()
		{
			Skills = new int[Enum.GetNames(typeof(SkillType)).Length];

			for(int i =0; i<Skills.Length; i++)
				Skills[i] = 1;
		}

		/// <summary>
		/// Serialization constructor.
		/// </summary>
		public SkillSet (SerializationInfo info, StreamingContext ctxt)
		{
			Skills = (int[]) info.GetValue("Skills", typeof(int[]));
		}
		
		/// <summary>
		/// Deserialization method.
		/// </summary>
		public void GetObjectData(SerializationInfo info, StreamingContext ctxt)
		{
			info.AddValue("Skills", Skills);
		}
	}
}
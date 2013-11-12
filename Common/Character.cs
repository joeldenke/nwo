using System;
using System.Runtime.Serialization;
using System.Collections.Generic;

namespace Common
{
	[Serializable]
	public class Character : ISerializable
	{
		public string Name;
		public AttributeSet AttributeSet;
		public SkillSet SkillSet;

		private double attributePercentage = 0.05;
		private double skillPercentage = 0.2;

		/// <summary>
		/// Initializes a new instance of the <see cref="Common.Character"/> class.
		/// </summary>
		/// <param name='name'>
		/// Name.
		/// </param>
		public Character (string name)
		{
			Name = name;
			AttributeSet = new AttributeSet();
			SkillSet = new SkillSet();
		}

		public int GetAttributeBonusValue (AttributeType attrType)
		{
			switch (attrType) {

			case AttributeType.AGILITY:
				return (int)(
					(SkillSet.Skills[(int)SkillType.HUNTING] + 
				SkillSet.Skills[(int)SkillType.RANGED] +
				SkillSet.Skills[(int)SkillType.STEALTH] +
				SkillSet.Skills[(int)SkillType.CRAFTING]) * attributePercentage);

			case AttributeType.ENDURANCE:
				return (int)(
					(SkillSet.Skills[(int)SkillType.HUNTING] + 
				 SkillSet.Skills[(int)SkillType.FARMING] + 
				 SkillSet.Skills[(int)SkillType.WOODCUTTING] + 
				 SkillSet.Skills[(int)SkillType.MELEE] +
				 SkillSet.Skills[(int)SkillType.MINING] + 
				 SkillSet.Skills[(int)SkillType.RANGED] + 
				 SkillSet.Skills[(int)SkillType.BUILDING]) * attributePercentage);

			case AttributeType.INTELLIGENCE:
				return (int)(
					(SkillSet.Skills[(int)SkillType.FARMING] + 
				 SkillSet.Skills[(int)SkillType.MAGIC] + 
				 SkillSet.Skills[(int)SkillType.DIPLOMACY] + 
				 SkillSet.Skills[(int)SkillType.CRAFTING] + 
				 SkillSet.Skills[(int)SkillType.BUILDING]) * attributePercentage);
			
			case AttributeType.PERCEPTION:
				return (int)(
					(SkillSet.Skills[(int)SkillType.MAGIC] + 
				 SkillSet.Skills[(int)SkillType.RANGED] + 
				 SkillSet.Skills[(int)SkillType.STEALTH] + 
				 SkillSet.Skills[(int)SkillType.DIPLOMACY] + 
				 SkillSet.Skills[(int)SkillType.PROPHECY]) * attributePercentage);

			case AttributeType.STRENGTH:
				return (int)(
					(SkillSet.Skills [(int)SkillType.MINING] +
				SkillSet.Skills [(int)SkillType.MELEE] +
				SkillSet.Skills [(int)SkillType.BUILDING]) * attributePercentage);

			case AttributeType.WILLPOWER:
				return (int)(
					(SkillSet.Skills[(int)SkillType.MAGIC] +
				 SkillSet.Skills[(int)SkillType.DIPLOMACY] + 
				 SkillSet.Skills[(int)SkillType.PROPHECY]) * attributePercentage);

			default:
				return 0;
			}
		}

		public int GetAttributeTotalValue (AttributeType attrType)
		{
			return AttributeSet.Attributes[(int)attrType] + GetAttributeBonusValue (attrType);
		}


		public int GetSkillBonusValue (SkillType skillType)
		{
			switch (skillType) {

			case SkillType.BUILDING:
				return (int)(
					(GetAttributeTotalValue(AttributeType.STRENGTH) + 
				 GetAttributeTotalValue(AttributeType.ENDURANCE)) * skillPercentage);

			case SkillType.CRAFTING:
				return (int)(
					(GetAttributeTotalValue(AttributeType.AGILITY) +
				 GetAttributeTotalValue(AttributeType.INTELLIGENCE)) * skillPercentage);

			case SkillType.DIPLOMACY:
				return (int)(
					(GetAttributeTotalValue(AttributeType.INTELLIGENCE) + 
				 GetAttributeTotalValue(AttributeType.PERCEPTION) + 
				 GetAttributeTotalValue(AttributeType.WILLPOWER)) * skillPercentage);

			case SkillType.FARMING:
				return (int)(
					(GetAttributeTotalValue(AttributeType.ENDURANCE) + 
				 GetAttributeTotalValue(AttributeType.INTELLIGENCE)) * skillPercentage);

			case SkillType.HUNTING:
				return (int)(
					(GetAttributeTotalValue (AttributeType.ENDURANCE) + 
				 GetAttributeTotalValue (AttributeType.AGILITY)) * skillPercentage);

			case SkillType.MAGIC:
				return (int)(
					(GetAttributeTotalValue(AttributeType.INTELLIGENCE) +
				 GetAttributeTotalValue(AttributeType.PERCEPTION) +
				 GetAttributeTotalValue(AttributeType.WILLPOWER)) * skillPercentage);

			case SkillType.MELEE:
				return (int)(
					(GetAttributeTotalValue(AttributeType.STRENGTH) + 
				 GetAttributeTotalValue(AttributeType.ENDURANCE)) * skillPercentage);

			case SkillType.MINING:
				return (int)(
					(GetAttributeTotalValue(AttributeType.STRENGTH) + 
				 GetAttributeTotalValue(AttributeType.ENDURANCE)) * skillPercentage);

			case SkillType.PROPHECY:
				return (int)((
					GetAttributeTotalValue(AttributeType.PERCEPTION) +
				    GetAttributeTotalValue(AttributeType.WILLPOWER)) * skillPercentage);

			case SkillType.RANGED:
				return (int)(
					(GetAttributeTotalValue(AttributeType.AGILITY) + 
				 GetAttributeTotalValue(AttributeType.PERCEPTION)) * skillPercentage);

			case SkillType.STEALTH:
				return (int)(
					(GetAttributeTotalValue(AttributeType.AGILITY) + 
				 GetAttributeTotalValue(AttributeType.PERCEPTION)) * skillPercentage);

			case SkillType.WOODCUTTING:
				return (int)(
					(GetAttributeTotalValue(AttributeType.ENDURANCE) +
				 GetAttributeTotalValue(AttributeType.AGILITY)) * skillPercentage);

			default:
				return 0;
			}
		}

		public int GetSkillTotalValue(SkillType skillType)
		{
			return SkillSet.Skills[(int)skillType] + GetSkillBonusValue(skillType);
		}


		/// <summary>
		/// Serialization constructor.
		/// </summary>
		public Character (SerializationInfo info, StreamingContext ctxt)
		{
			Name = (String)info.GetValue("Name", typeof(string));
			AttributeSet = (AttributeSet)info.GetValue("AttributeSet", typeof(AttributeSet));
			SkillSet = (SkillSet)info.GetValue("SkillSet", typeof(SkillSet));
		}
		
		/// <summary>
		/// Deserialization method.
		/// </summary>
		public void GetObjectData(SerializationInfo info, StreamingContext ctxt)
		{
			info.AddValue("Name", Name);
			info.AddValue("AttributeSet", AttributeSet);
			info.AddValue("SkillSet", SkillSet);
		}
	}
}


using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Common
{
	/// <summary>
	/// Area estate type enumerator.
	/// </summary>
	public enum AreaEstateType
	{
		EMPTY,
		RUINS,
		CITY
	}

	/// <summary>
	/// Area terrain type enumerator.
	/// </summary>
	public enum AreaTerrainType
	{
		DESERT,
		FOREST,
		PLAINS,
		SWAMP,
		JUNGLE
	}

	/// <summary>
	/// Represents an in-map area.
	/// </summary>
	[Serializable]
	public class Area : ISerializable
	{
		public AreaEstateType EstateType { get; set; }
		public AreaTerrainType TerrainType { get; set; }
		public Position Position { get; set; }
		public List<Character> Characters { get; set; }
		public List<Posession> Posessions  { get; set; }

		/// <summary>
		/// Initializes a new instance of the <see cref="Common.Area"/> class.
		/// </summary>
		/// <param name='position'>
		/// Position.
		/// </param>
		/// <param name='terrainType'>
		/// Terrain type.
		/// </param>
		public Area (Position position, AreaTerrainType terrainType)
		{
			this.Position = position;
			this.TerrainType = terrainType;
			this.EstateType = AreaEstateType.EMPTY;

			Characters = new List<Character>();
			Posessions = new List<Posession>();
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Common.Area"/> class.
		/// </summary>
		/// <param name='position'>
		/// Position.
		/// </param>
		/// <param name='terrainType'>
		/// Terrain type.
		/// </param>
		/// <param name='estateType'>
		/// Estate type.
		/// </param>
		public Area (Position position, AreaTerrainType terrainType, AreaEstateType estateType)
		{
			this.Position = position;
			this.TerrainType = terrainType;
			this.EstateType = estateType;

			Characters = new List<Character>();
			Posessions = new List<Posession>();
		}

		/// <summary>
		/// Serialization constructor.
		/// </summary>
		public Area (SerializationInfo info, StreamingContext ctxt)
		{
			EstateType = (AreaEstateType)info.GetValue("EstateType", typeof(AreaEstateType));
			TerrainType = (AreaTerrainType)info.GetValue("TerrainType", typeof(AreaTerrainType));
			Position = (Position)info.GetValue("Position", typeof(Position));
			Characters = (List<Character>)info.GetValue("Characters", typeof(List<Character>));
			Posessions = (List<Posession>)info.GetValue("Posessions", typeof(List<Posession>));
		}

		/// <summary>
		/// Gets an area skill modifier.
		/// </summary>
		/// <returns>
		/// The modifier value.
		/// </returns>
		/// <param name='skill'>
		/// Skill.
		/// </param>
		public int GetModifier(SkillType skill)
		{
			return 0; // TODO.
		}

		/// <summary>
		/// Deserialization method.
		/// </summary>
		public void GetObjectData(SerializationInfo info, StreamingContext ctxt)
		{
			info.AddValue("EstateType", EstateType);
			info.AddValue("TerrainType", TerrainType);
			info.AddValue("Position", Position);
			info.AddValue("Characters", Characters);
			info.AddValue("Posessions", Posessions);
		}

		/// <summary>
		/// Sets area skill modifier.
		/// </summary>
		/// <param name='skill'>
		/// Skill.
		/// </param>
		/// <param name='value'>
		/// Modifier value.
		/// </param>
		public void SetModifier(SkillType skill, int value)
		{
			// TODO
		}
	}
}


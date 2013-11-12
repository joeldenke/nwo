using System;
using System.Runtime.Serialization;

namespace Common
{
	/// <summary>
	/// Represents a 2-dimensional discreete position.
	/// </summary>
	[Serializable]
	public class Position : ISerializable
	{
		public int X { get; set; }
		public int Y { get; set; }

		public Position (int x, int y)
		{
			this.X = x;
			this.Y = y;
		}

		/// <summary>
		/// Serialization constructor.
		/// </summary>
		public Position (SerializationInfo info, StreamingContext ctxt)
		{
			X = (int) info.GetValue("X", typeof(int));
			Y = (int) info.GetValue("Y", typeof(int));
		}
		
		/// <summary>
		/// Deserialization method.
		/// </summary>
		public void GetObjectData(SerializationInfo info, StreamingContext ctxt)
		{
			info.AddValue("X", X);
			info.AddValue("Y", Y);
		}

		/// <summary>
		/// Returns a <see cref="System.String"/> that represents the current <see cref="Common.Position"/>.
		/// </summary>
		/// <returns>
		/// A <see cref="System.String"/> that represents the current <see cref="Common.Position"/>.
		/// </returns>
		public override string ToString() {
			return "(" + X + "," + Y + ")";
		}

		/// <summary>
		/// Determines whether the specified <see cref="Common.Position"/> is equal to the current <see cref="Common.Position"/>.
		/// </summary>
		/// <param name='compared'>
		/// The <see cref="Common.Position"/> to compare with the current <see cref="Common.Position"/>.
		/// </param>
		/// <returns>
		/// <c>true</c> if the specified <see cref="Common.Position"/> is equal to the current <see cref="Common.Position"/>;
		/// otherwise, <c>false</c>.
		/// </returns>
		public override bool Equals (Object obj)
		{
			if (obj == null)
				return false;

			Position compared = (Position)obj;

			return compared.X == X && compared.Y == Y;
		}

		/// <summary>
		/// Serves as a hash function for a <see cref="Common.Position"/> object.
		/// </summary>
		/// <returns>
		/// A hash code for this instance that is suitable for use in hashing algorithms and data structures such as a hash table.
		/// </returns>
		public override int GetHashCode ()
		{
			return X ^ Y;
		}
	}
}


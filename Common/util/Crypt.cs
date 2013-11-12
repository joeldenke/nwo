using System;
using System.Security.Cryptography;
using System.Text;

namespace Common
{
	/// <summary>
	/// Internal cryptographic tools
	/// </summary>
	public class Crypt
	{
		public Crypt ()
		{
		}

		/// <summary>
		/// Generate SHA256 hash from data
		/// </summary>
		/// <param name="data">Data input</param>
		/// <returns>Hash string</returns>
		public static string GenHash(string data)
		{
		   Byte[] inputBytes = Encoding.UTF8.GetBytes(data);
		   Byte[] hashedBytes = new SHA256Managed().ComputeHash(inputBytes);

		   return BitConverter.ToString(hashedBytes);
		}
	}
}


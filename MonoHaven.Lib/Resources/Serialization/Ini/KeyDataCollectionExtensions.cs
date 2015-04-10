using System;
using IniParser.Model;

namespace SharpHaven.Resources.Serialization.Ini
{
	public static class KeyDataCollectionExtensions
	{
		public static void AddKey<T>(this KeyDataCollection keyData, string key, T value)
		{
			keyData.AddKey(key, (value != null) ? value.ToString() : "");
		}

		public static bool GetBool(this KeyDataCollection keyData, string key)
		{
			var value = keyData[key] ?? "";
			return bool.Parse(value);
		}

		public static bool GetBool(this KeyDataCollection keyData, string key, bool defaultValue)
		{
			bool value;
			var strValue = keyData[key] ?? "";
			return bool.TryParse(strValue, out value) ? value : defaultValue;
		}

		public static byte GetByte(this KeyDataCollection keyData, string key)
		{
			var value = keyData[key] ?? "";
			return byte.Parse(value);
		}

		public static byte GetByte(this KeyDataCollection keyData, string key, byte defaultValue)
		{
			byte value;
			var strValue = keyData[key] ?? "";
			return byte.TryParse(strValue, out value) ? value : defaultValue;
		}

		public static char GetChar(this KeyDataCollection keyData, string key)
		{
			var value = keyData[key] ?? "";
			if (value.Length == 1)
				return value[0];
			throw new FormatException("Expected single character");
		}

		public static char GetChar(this KeyDataCollection keyData, string key, char defaultValue)
		{
			var value = keyData[key] ?? "";
			return value.Length > 0 ? value[0] : defaultValue;
		}

		public static int GetInt(this KeyDataCollection keyData, string key)
		{
			var value = keyData[key] ?? "";
			return int.Parse(value);
		}

		public static int GetInt(this KeyDataCollection keyData, string key, int defaultValue)
		{
			int value;
			var strValue = keyData[key] ?? "";
			return int.TryParse(strValue, out value) ? value : defaultValue;
		}

		public static ushort GetUInt16(this KeyDataCollection keyData, string key)
		{
			var value = keyData[key] ?? "";
			return ushort.Parse(value);
		}

		public static ushort GetUInt16(this KeyDataCollection keyData, string key, ushort defaultValue)
		{
			ushort value;
			var strValue = keyData[key] ?? "";
			return ushort.TryParse(strValue, out value) ? value : defaultValue;
		}
	}
}

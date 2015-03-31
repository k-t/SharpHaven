using IniParser.Model;

namespace MonoHaven.Resources.Serialization.Ini
{
	public static class KeyDataCollectionExtensions
	{
		public static void AddKey<T>(this KeyDataCollection keyData, string key, T value)
		{
			keyData.AddKey(key, (value != null) ? value.ToString() : "");
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
	}
}

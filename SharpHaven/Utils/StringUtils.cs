﻿using System.Globalization;

namespace SharpHaven.Utils
{
	public static class StringUtils
	{
		public static string ToTitleCase(this string str)
		{
			return CultureInfo.CurrentCulture.TextInfo.ToTitleCase(str.ToLower());
		}
	}
}

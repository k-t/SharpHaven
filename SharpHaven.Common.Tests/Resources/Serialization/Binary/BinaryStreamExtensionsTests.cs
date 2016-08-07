﻿using System;
using System.IO;
using NUnit.Framework;
using SharpHaven.Utils;

namespace SharpHaven.Resources.Serialization.Binary
{
	[TestFixture]
	public class BinaryStreamExtensionsTests
	{
		[TestCase(0.1720519997179508)]
		[TestCase(-0.035707999995793216)]
		[TestCase(0)]
		public void ReadWriteFloat40Works(double input)
		{
			using (var ms = new MemoryStream())
			using (var buffer = new ByteBuffer(ms))
			{
				buffer.WriteFloat40(input);
				ms.Position = 0;

				var output = buffer.ReadFloat40();

				var tolerance = 1.0 / Math.Pow(10, 6);
				Assert.That(output, Is.EqualTo(input).Within(tolerance));
			}
		}
	}
}
using System;
using System.Collections.Generic;
using System.IO;

namespace MonoHaven.Resources.Serialization.Binary
{
	public class CodeEntryDataSerializer : IBinaryDataLayerSerializer
	{
		public string LayerName
		{
			get { return "codeentry"; }
		}

		public Type LayerType
		{
			get { return typeof(CodeEntry); }
		}

		public object Deserialize(BinaryReader reader, int size)
		{
			var entries = new List<CodeEntry>();
			using (var ms = new MemoryStream(reader.ReadBytes(size)))
			using (var br = new BinaryReader(ms))
			{
				while (br.BaseStream.Position < size)
				{
					var name = br.ReadCString();
					var className = br.ReadCString();
					entries.Add(new CodeEntry(name, className));
				}
			}
			return new CodeEntryData { Entries = entries.ToArray() };
		}

		public void Serialize(BinaryWriter writer, object data)
		{
			var entryData = (CodeEntryData)data;
			foreach (var entry in entryData.Entries)
			{
				writer.WriteCString(entry.Name);
				writer.WriteCString(entry.ClassName);
			}
		}
	}
}

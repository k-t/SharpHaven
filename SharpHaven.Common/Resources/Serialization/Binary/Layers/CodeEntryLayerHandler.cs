using System.Collections.Generic;
using SharpHaven.Utils;

namespace SharpHaven.Resources.Serialization.Binary.Layers
{
	public class CodeEntryLayerHandler : GenericLayerHandler<CodeEntryLayer>
	{
		private const byte EntriesPart = 1;
		private const byte ClasspathPart = 2;

		public CodeEntryLayerHandler() : base("codeentry")
		{
		}

		protected override CodeEntryLayer Deserialize(ByteBuffer buffer)
		{
			var entries = new List<CodeEntry>();
			var classpath = new List<ResourceRef>();
			while (buffer.HasRemaining)
			{
				var type = buffer.ReadByte();
				switch (type)
				{
					case EntriesPart:
						while (true)
						{
							var name = buffer.ReadCString();
							var className = buffer.ReadCString();
							if (name.Length == 0)
								break;
							entries.Add(new CodeEntry(name, className));
						}
						break;
					case ClasspathPart:
						while (true)
						{
							var ln = buffer.ReadCString();
							if (ln.Length == 0)
								break;
							var ver = buffer.ReadUInt16();
							classpath.Add(new ResourceRef(ln, ver));
						}
						break;
				}
			}
			return new CodeEntryLayer {
				Entries = entries.ToArray(),
				Classpath = classpath.ToArray()
			};
		}

		protected override void Serialize(ByteBuffer writer, CodeEntryLayer data)
		{
			// entries
			if (data.Entries != null && data.Entries.Length > 0)
			{
				writer.Write(EntriesPart);
				foreach (var entry in data.Entries)
				{
					writer.WriteCString(entry.Name);
					writer.WriteCString(entry.ClassName);
				}
				writer.WriteCString("");
				writer.WriteCString("");
			}
			// classpath
			if (data.Classpath != null && data.Classpath.Length > 0)
			{
				writer.Write(ClasspathPart);
				foreach (var res in data.Classpath)
				{
					writer.WriteCString(res.Name);
					writer.Write(res.Version);
				}
				writer.WriteCString("");
			}
		}
	}
}

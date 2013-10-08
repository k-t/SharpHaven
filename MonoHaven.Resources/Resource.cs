using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using MonoHaven.Core;

namespace MonoHaven.Resources
{
	internal class Resource : IResource, IPrioritized
	{
		private const string Signature = "Haven Resource 1";

		private readonly string _name;
		private int _version;
		private int _priority;
		private List<Layer> _layers = new List<Layer>();

		public Resource(string name, int version)
		{
			_name = name;
			_version = version;
			IsLoading = true;
		}
		
		public string Name
		{
			get { return _name; }
		}
		
		public int Version
		{
			get { return _version; }
		}

		public bool HasError
		{
			get { return Error != null; }
		}

		public Exception Error
		{
			get;
			set;
		}
		
		public int Priority
		{
			get { return _priority; }
			set
			{
				if (_priority < value)
					_priority = value;
			}
		}

		public string Source
		{
			get;
			set;
		}

		public bool IsLoading
		{
			get;
			set;
		}

		public T GetLayer<T>() where T : Layer
		{
			Type t = typeof(T);
			return (T)_layers.FirstOrDefault(l => t.IsInstanceOfType(l));
		}

		public void Load(ILayerFactory factory, Stream stream)
		{
			var reader = new BinaryReader(stream);

			var sig = new String(reader.ReadChars(Signature.Length));
			if (sig != Signature)
				throw new ResourceLoadException("Invalid signature");

			short version = reader.ReadInt16();
			if (_version == -1)
				_version = version;
			else if (_version != version)
				throw new ResourceLoadException(string.Format("Wrong version ({0} != {1})", version, _version));

			var layers = new List<Layer>();
			while (true)
			{
				var layer = LoadLayer(factory, reader);
				if (layer == null)
					break;
				layers.Add(layer);
			}
			foreach (var layer in layers)
				layer.Init();
			_layers = layers;
		}

		private static Layer LoadLayer(ILayerFactory factory, BinaryReader reader)
		{
			var type = ReadString(reader);
			if (string.IsNullOrEmpty(type))
				return null;
			var size = reader.ReadInt32();
			var data = new byte[size];
			reader.Read(data, 0, size);
			return factory.Make(type, data);
		}

		private static string ReadString(BinaryReader reader)
		{
			var sb = new StringBuilder();
			while (true)
			{
				int next = reader.Read();
				if (next == -1)
					if (sb.Length != 0)
						throw new ResourceLoadException("Incomplete resource at " + sb.ToString());
					else
						return string.Empty;
				else if (next == 0)
					break;
				sb.Append(Convert.ToChar(next));
			}
			return sb.ToString();
		}
	}
}


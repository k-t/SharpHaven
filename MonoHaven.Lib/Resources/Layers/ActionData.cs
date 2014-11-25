using System.IO;

namespace MonoHaven.Resources
{
	public class ActionData
	{
		public string Name { get; set; }
		public ResourceRef Parent { get; set; }
		public char Hotkey { get; set; }
		public string[] Verbs { get; set; }
	}

	public class ActionDataSerializer : IDataLayerSerializer
	{
		public string LayerName
		{
			get { return "action"; }
		}

		public object Deserialize(int size, BinaryReader reader)
		{
			var action = new ActionData();
			action.Parent = new ResourceRef(reader.ReadCString(), reader.ReadUInt16());
			action.Name = reader.ReadCString();
			reader.ReadCString(); /* prerequisite skill */
			action.Hotkey = (char)reader.ReadUInt16();
			action.Verbs = new string[reader.ReadUInt16()];
			for (int i = 0; i < action.Verbs.Length; i++)
				action.Verbs[i] = reader.ReadCString();
			return action;
		}
	}
}

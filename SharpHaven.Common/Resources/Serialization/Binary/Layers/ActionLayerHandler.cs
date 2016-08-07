using SharpHaven.Utils;

namespace SharpHaven.Resources.Serialization.Binary.Layers
{
	internal class ActionLayerHandler : GenericLayerHandler<ActionLayer>
	{
		public ActionLayerHandler() : base("action")
		{
		}

		protected override ActionLayer Deserialize(ByteBuffer buffer)
		{
			var action = new ActionLayer();
			action.Parent = new ResourceRef(buffer.ReadCString(), buffer.ReadUInt16());
			action.Name = buffer.ReadCString();
			action.Prerequisite = buffer.ReadCString();
			action.Hotkey = (char)buffer.ReadUInt16();
			action.Verbs = new string[buffer.ReadUInt16()];
			for (int i = 0; i < action.Verbs.Length; i++)
				action.Verbs[i] = buffer.ReadCString();
			return action;
		}

		protected override void Serialize(ByteBuffer writer, ActionLayer action)
		{
			writer.WriteCString(action.Parent.Name ?? "");
			writer.Write(action.Parent.Version);
			writer.WriteCString(action.Name);
			writer.WriteCString(action.Prerequisite);
			writer.Write((ushort)action.Hotkey);
			writer.Write((ushort)action.Verbs.Length);
			foreach (var verb in action.Verbs)
				writer.WriteCString(verb);
		}
	}
}
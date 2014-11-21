using System;
using System.Collections.Generic;

namespace MonoHaven.UI.Remote
{
	public class ServerWidgetFactory
	{
		private delegate ServerWidget FactoryMethod(ushort id, ServerWidget parent, object[] args);

		private readonly Dictionary<string, FactoryMethod> methods;

		public ServerWidgetFactory()
		{
			methods = new Dictionary<string, FactoryMethod>();
			methods["cnt"] = ServerContainer.Create;
			methods["img"] = ServerImage.Create;
			methods["charlist"] = ServerCharlist.Create;
			methods["ibtn"] = ServerImageButton.Create;
			methods["wnd"] = ServerWindow.Create;
			methods["av"] = ServerAvatarView.Create;
			methods["av2"] = ServerAvatarView.CreateFromLayers;
			methods["lbl"] = ServerLabel.Create;
			methods["btn"] = ServerButton.Create;
			methods["mapview"] = ServerMapView.Create;
			methods["slen"] = ServerHud.Create;
			methods["cal"] = ServerCalendar.Create;
			methods["slenlog"] = ServerLogWindow.Create;
			methods["slenchat"] = ServerChatWindow.Create;
			methods["im"] = ServerMeter.Create;
			methods["speedget"] = ServerSpeedget.Create;
			methods["buffs"] = ServerBufflist.Create;
			methods["scm"] = ServerMenuGrid.Create;
			methods["pv"] = ServerPartyView.Create;
			methods["text"] = ServerTextBox.Create;
			methods["inv"] = ServerInventoryWidget.Create;
			methods["item"] = ServerItemWidget.Create;
		}

		public ServerWidget Create(
			string widgetType,
			ushort id,
			ServerWidget parent,
			object[] args)
		{
			FactoryMethod method;
			if (methods.TryGetValue(widgetType, out method))
				return method(id, parent, args);
			throw new ArgumentException("Unknown widget type " + widgetType);
		}
	}
}

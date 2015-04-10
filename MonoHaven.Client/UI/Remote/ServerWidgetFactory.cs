using System;
using System.Collections.Generic;

namespace SharpHaven.UI.Remote
{
	public class ServerWidgetFactory
	{
		private delegate ServerWidget FactoryMethod(ushort id, ServerWidget parent);

		private readonly Dictionary<string, FactoryMethod> methods;

		public ServerWidgetFactory()
		{
			methods = new Dictionary<string, FactoryMethod>();
			methods["cnt"] = ServerContainer.Create;
			methods["img"] = ServerImage.Create;
			methods["charlist"] = ServerCharlist.Create;
			methods["ibtn"] = ServerImageButton.Create;
			methods["wnd"] = ServerWindow.Create;
			methods["av"] = ServerGobAvatarView.Create;
			methods["av2"] = ServerLayeredAvatarView.Create;
			methods["lbl"] = ServerLabel.Create;
			methods["btn"] = ServerButton.Create;
			methods["mapview"] = ServerMapView.Create;
			methods["slen"] = ServerHud.Create;
			methods["cal"] = ServerCalendar.Create;
			methods["slenlog"] = ServerChat.Create;
			methods["slenchat"] = ServerChat.Create;
			methods["im"] = ServerMeter.Create;
			methods["speedget"] = ServerSpeedget.Create;
			methods["buffs"] = ServerBufflist.Create;
			methods["scm"] = ServerMenuGrid.Create;
			methods["pv"] = ServerPartyWidget.Create;
			methods["text"] = ServerTextBox.Create;
			methods["inv"] = ServerInventoryWidget.Create;
			methods["item"] = ServerItemWidget.Create;
			methods["sm"] = ServerFlowerMenu.Create;
			methods["epry"] = ServerEquipory.Create;
			methods["buddy"] = ServerBuddyList.Create;
			methods["chr"] = ServerCharWindow.Create;
			methods["vm"] = ServerVMeter.Create;
			methods["prog"] = ServerProgress.Create;
			methods["isbox"] = ServerISBox.Create;
			methods["make"] = ServerCraftWindow.Create;
			methods["frv"] = ServerCombatView.Create;
			methods["give"] = ServerGiveButton.Create;
			methods["ui/land2"] = ServerClaimWindow.Create;
		}

		public ServerWidget Create(
			string widgetType,
			ushort id,
			ServerWidget parent)
		{
			// if widget is supposed to be in a resource
			if (widgetType.Contains("/"))
				// remove version, e.g. ui/land2:6 => ui/land2
				widgetType = widgetType.Split(':')[0];

			FactoryMethod method;
			if (methods.TryGetValue(widgetType, out method))
				return method(id, parent);

			throw new ArgumentException("Unknown widget type " + widgetType);
		}
	}
}

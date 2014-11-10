using System;
using System.Collections.Generic;

namespace MonoHaven.UI.Remote
{
	public class RemoteWidgetFactory
	{
		private delegate RemoteWidget FactoryMethod(int id, RemoteWidget parent, object[] args);

		private readonly Dictionary<string, FactoryMethod> factoryMethods;

		public RemoteWidgetFactory()
		{
			factoryMethods = new Dictionary<string, FactoryMethod>();
			factoryMethods["cnt"] = (id, parent, args) => new RemoteContainer(id, parent);
			factoryMethods["img"] = (id, parent, args) => new RemoteImage(id, parent, args);
			factoryMethods["charlist"] = (id, parent, args) => new RemoteContainer(id, parent);
			factoryMethods["ibtn"] = (id, parent, args) => new RemoteImageButton(id, parent, args);
		}

		public RemoteWidget Create(int id, string type, RemoteWidget parent, object[] args)
		{
			FactoryMethod method;
			if (!factoryMethods.TryGetValue(type, out method))
				throw new Exception("Unknown widget type " + type);
			return method(id, parent, args);
		}
	}
}

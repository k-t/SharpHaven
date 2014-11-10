using System;
using System.Collections.Generic;

namespace MonoHaven.UI.Remote
{
	public class ControllerFactory
	{
		private delegate Controller FactoryMethod(int id, Controller parent, object[] args);

		private readonly Dictionary<string, FactoryMethod> constructors;

		public ControllerFactory()
		{
			constructors = new Dictionary<string, FactoryMethod>();
			constructors["cnt"] = (id, parent, args) => new ContainerController(id, parent);
			constructors["img"] = (id, parent, args) => new ImageController(id, parent, args);
			constructors["charlist"] = (id, parent, args) => new ContainerController(id, parent);
			constructors["ibtn"] = (id, parent, args) => new ImageButtonController(id, parent, args);
		}

		public Controller Create(int id, string type, Controller parent, object[] args)
		{
			FactoryMethod constructor;
			if (!constructors.TryGetValue(type, out constructor))
				throw new Exception("Unknown widget type " + type);
			return constructor(id, parent, args);
		}
	}
}

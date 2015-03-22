using System;
using System.Collections.Generic;
using System.Drawing;
using MonoHaven.Game;
using MonoHaven.Graphics;
using MonoHaven.Graphics.Sprites;
using OpenTK;
using SharpFont;

namespace MonoHaven.Resources
{
	public class ResourceManager
	{
		private readonly IResourceSource defaultSource = new FolderSource("haven-res/res");
		
		private readonly Dictionary<Type, ResourceObjectCache> objectCaches;
		private readonly Dictionary<Type, IObjectFactory<object>> objectFactories;

		public ResourceManager()
		{
			objectCaches = new Dictionary<Type, ResourceObjectCache>();
			objectFactories = new Dictionary<Type, IObjectFactory<object>>();

			var drawableFactory = new DrawableFactory();

			RegisterType(typeof(Drawable), drawableFactory);
			RegisterType(typeof(SkillInfo), new SkillInfoFactory(drawableFactory));
			RegisterType(typeof(Bitmap), new BitmapFactory());
			RegisterType(typeof(MouseCursor), new MouseCursorFactory());
			RegisterType(typeof(Face), new FontFaceFactory());
			RegisterType(typeof(Tileset), new TilesetFactory());
			RegisterType(typeof(SpritePrototype), new SpritePrototypeFactory());
			RegisterType(typeof(GameActionInfo), new GameActionInfoFactory());
		}

		public T Get<T>(string resName) where T : class
		{
			var cache = objectCaches[typeof(T)];
			var obj = cache.Get(resName) as T;
			if (obj == null)
			{
				var res = Load(resName);
				var factory = objectFactories[typeof(T)];
				obj = (T)factory.Create(resName, res);
				cache.Add(resName, obj);
			}
			return obj;
		}

		public ISprite GetSprite(string resName, byte[] state = null)
		{
			var prototype = Get<SpritePrototype>(resName);
			return prototype.CreateInstance(state);
		}

		public Resource Load(string resName)
		{
			return defaultSource.Get(resName);
		}

		private void RegisterType(Type type, IObjectFactory<object> factory)
		{
			objectFactories[type] = factory;
			objectCaches[type] = new ResourceObjectCache();
		}
	}
}

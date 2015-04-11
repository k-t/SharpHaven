using System;
using System.Collections.Generic;
using System.Drawing;
using NLog;
using OpenTK;
using SharpFont;
using SharpHaven.Game;
using SharpHaven.Graphics;
using SharpHaven.Graphics.Sprites;

namespace SharpHaven.Resources
{
	public class ResourceManager
	{
		private static readonly Logger Log = LogManager.GetCurrentClassLogger();

		private readonly List<IResourceSource> sources;
		private readonly Dictionary<Type, ResourceObjectCache> objectCaches;
		private readonly Dictionary<Type, IObjectFactory<object>> objectFactories;

		public ResourceManager()
		{
			sources = new List<IResourceSource>();
			objectCaches = new Dictionary<Type, ResourceObjectCache>();
			objectFactories = new Dictionary<Type, IObjectFactory<object>>();

			var drawableFactory = new DrawableFactory();
			RegisterType(typeof(Drawable), drawableFactory);
			RegisterType(typeof(Skill), new SkillFactory(drawableFactory));
			RegisterType(typeof(Bitmap), new BitmapFactory());
			RegisterType(typeof(MouseCursor), new MouseCursorFactory());
			RegisterType(typeof(Face), new FontFaceFactory());
			RegisterType(typeof(Tileset), new TilesetFactory());
			RegisterType(typeof(SpriteMaker), new SpriteMakerFactory());
			RegisterType(typeof(GameAction), new GameActionFactory());
			RegisterType(typeof(ItemMold), new ItemMoldFactory(drawableFactory));
			RegisterType(typeof(BuffMold), new BuffMoldFactory(drawableFactory));

			AddSource(new FolderSource("Data"));
			AddSource(new JarSource("haven-res.jar"));
			AddSource(new HttpSource(App.Config.ResUrl));
			
		}

		public void AddSource(IResourceSource source)
		{
			sources.Add(source);
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
			var maker = Get<SpriteMaker>(resName);
			return maker.MakeInstance(state);
		}

		public Resource Load(string resName)
		{
			foreach (var source in sources)
			{
				try
				{
					var res = source.Get(resName);
					if (res != null)
						return res;
				}
				catch (Exception ex)
				{
					Log.Debug(ex);
				}
			}
			throw new ResourceException("Couldn't load resource " + resName);
		}

		private void RegisterType(Type type, IObjectFactory<object> factory)
		{
			objectFactories[type] = factory;
			objectCaches[type] = new ResourceObjectCache();
		}
	}
}

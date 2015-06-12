using System.Collections.Generic;
using SharpHaven.Game.Events;

namespace SharpHaven.Game
{
	public class CompositeGameEventListener : IGameEventListener
	{
		private readonly List<IGameEventListener> listeners;

		public CompositeGameEventListener()
		{
			listeners = new List<IGameEventListener>();
		}

		public void Add(IGameEventListener listener)
		{
			listeners.Add(listener);
		}

		public void Remove(IGameEventListener listener)
		{
			listeners.Remove(listener);
		}

		public void Handle(WidgetCreateEvent args)
		{
			foreach (var listener in listeners)
				listener.Handle(args);
		}

		public void Handle(WidgetMessageEvent args)
		{
			foreach (var listener in listeners)
				listener.Handle(args);
		}

		public void Handle(WidgetDestroyEvent args)
		{
			foreach (var listener in listeners)
				listener.Handle(args);
		}

		public void Handle(ResourceLoadEvent args)
		{
			foreach (var listener in listeners)
				listener.Handle(args);
		}

		public void Handle(TilesetsLoadEvent args)
		{
			foreach (var listener in listeners)
				listener.Handle(args);
		}

		public void Handle(MapInvalidateEvent args)
		{
			foreach (var listener in listeners)
				listener.Handle(args);
		}

		public void Handle(MapInvalidateGridEvent args)
		{
			foreach (var listener in listeners)
				listener.Handle(args);
		}

		public void Handle(MapInvalidateRegionEvent args)
		{
			foreach (var listener in listeners)
				listener.Handle(args);
		}

		public void Handle(CharAttributesUpdateEvent args)
		{
			foreach (var listener in listeners)
				listener.Handle(args);
		}

		public void Handle(GameTimeUpdateEvent args)
		{
			foreach (var listener in listeners)
				listener.Handle(args);
		}

		public void Handle(AmbientLightUpdateEvent args)
		{
			foreach (var listener in listeners)
				listener.Handle(args);
		}

		public void Handle(AstronomyUpdateEvent args)
		{
			foreach (var listener in listeners)
				listener.Handle(args);
		}

		public void Handle(GameActionsUpdateEvent args)
		{
			foreach (var listener in listeners)
				listener.Handle(args);
		}

		public void Handle(GobUpdateEvent args)
		{
			foreach (var listener in listeners)
				listener.Handle(args);
		}

		public void Handle(MapUpdateEvent args)
		{
			foreach (var listener in listeners)
				listener.Handle(args);
		}

		public void Handle(BuffUpdateEvent args)
		{
			foreach (var listener in listeners)
				listener.Handle(args);
		}

		public void Handle(BuffRemoveEvent args)
		{
			foreach (var listener in listeners)
				listener.Handle(args);
		}

		public void Handle(BuffClearEvent args)
		{
			foreach (var listener in listeners)
				listener.Handle(args);
		}

		public void Handle(PartyLeaderChangeEvent args)
		{
			foreach (var listener in listeners)
				listener.Handle(args);
		}

		public void Handle(PartyUpdateEvent args)
		{
			foreach (var listener in listeners)
				listener.Handle(args);
		}

		public void Handle(PartyMemberUpdateEvent args)
		{
			foreach (var listener in listeners)
				listener.Handle(args);
		}

		public void Handle(PlaySoundEvent args)
		{
			foreach (var listener in listeners)
				listener.Handle(args);
		}

		public void Handle(PlayMusicEvent args)
		{
			foreach (var listener in listeners)
				listener.Handle(args);
		}

		public void Exit()
		{
			foreach (var listener in listeners)
				listener.Exit();
		}
	}
}

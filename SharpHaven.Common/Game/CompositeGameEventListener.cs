using System.Collections.Generic;
using System.Drawing;
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

		public void CreateWidget(WidgetCreateEvent args)
		{
			foreach (var listener in listeners)
				listener.CreateWidget(args);
		}

		public void UpdateWidget(WidgetMessage args)
		{
			foreach (var listener in listeners)
				listener.UpdateWidget(args);
		}

		public void DestroyWidget(ushort widgetId)
		{
			foreach (var listener in listeners)
				listener.DestroyWidget(widgetId);
		}

		public void LoadResource(ResourceLoadEvent args)
		{
			foreach (var listener in listeners)
				listener.LoadResource(args);
		}

		public void LoadTilesets(IEnumerable<TilesetLoadEvent> args)
		{
			foreach (var listener in listeners)
				listener.LoadTilesets(args);
		}

		public void InvalidateMap()
		{
			foreach (var listener in listeners)
				listener.InvalidateMap();
		}

		public void InvalidateMap(Point gc)
		{
			foreach (var listener in listeners)
				listener.InvalidateMap(gc);
		}

		public void InvalidateMap(Point ul, Point br)
		{
			foreach (var listener in listeners)
				listener.InvalidateMap(ul, br);
		}

		public void UpdateCharAttributes(IEnumerable<CharAttrUpdateEvent> attributes)
		{
			foreach (var listener in listeners)
				listener.UpdateCharAttributes(attributes);
		}

		public void UpdateTime(int time)
		{
			foreach (var listener in listeners)
				listener.UpdateTime(time);
		}

		public void UpdateAmbientLight(Color color)
		{
			foreach (var listener in listeners)
				listener.UpdateAmbientLight(color);
		}

		public void UpdateAstronomy(AstronomyEvent args)
		{
			foreach (var listener in listeners)
				listener.UpdateAstronomy(args);
		}

		public void UpdateActions(IEnumerable<ActionUpdateEvent> actions)
		{
			foreach (var listener in listeners)
				listener.UpdateActions(actions);
		}

		public void UpdateGob(GobUpdateEvent args)
		{
			foreach (var listener in listeners)
				listener.UpdateGob(args);
		}

		public void UpdateMap(MapUpdateEvent args)
		{
			foreach (var listener in listeners)
				listener.UpdateMap(args);
		}

		public void UpdateBuff(BuffUpdateEvent args)
		{
			foreach (var listener in listeners)
				listener.UpdateBuff(args);
		}

		public void RemoveBuff(int buffId)
		{
			foreach (var listener in listeners)
				listener.RemoveBuff(buffId);
		}

		public void ClearBuffs()
		{
			foreach (var listener in listeners)
				listener.ClearBuffs();
		}

		public void SetPartyLeader(int leaderId)
		{
			foreach (var listener in listeners)
				listener.SetPartyLeader(leaderId);
		}

		public void UpdatePartyList(List<int> memberIds)
		{
			foreach (var listener in listeners)
				listener.UpdatePartyList(memberIds);
		}

		public void UpdatePartyMember(int memberId, Color color, Point? location)
		{
			foreach (var listener in listeners)
				listener.UpdatePartyMember(memberId, color, location);
		}

		public void PlaySound(SoundEvent args)
		{
			foreach (var listener in listeners)
				listener.PlaySound(args);
		}

		public void PlayMusic()
		{
			foreach (var listener in listeners)
				listener.PlayMusic();
		}

		public void Exit()
		{
			foreach (var listener in listeners)
				listener.Exit();
		}
	}
}

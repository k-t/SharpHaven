using System.Collections.Generic;
using System.Drawing;
using SharpHaven.Game.Events;

namespace SharpHaven.Game
{
	public interface IGameEventListener
	{
		void CreateWidget(WidgetCreateEvent args);
		void UpdateWidget(WidgetMessage args);
		void DestroyWidget(ushort widgetId);

		void LoadResource(ResourceLoadEvent args);
		void LoadTilesets(IEnumerable<TilesetLoadEvent> args);

		void InvalidateMap();
		void InvalidateMap(Point gc);
		void InvalidateMap(Point ul, Point br);

		void UpdateCharAttributes(IEnumerable<CharAttrUpdateEvent> attributes);
		void UpdateTime(int time);
		void UpdateAmbientLight(Color color);
		void UpdateAstronomy(AstronomyEvent astronomy);
		void UpdateActions(IEnumerable<ActionUpdateEvent> actions);
		void UpdateGob(GobUpdateEvent args);
		void UpdateMap(MapUpdateEvent args);

		void UpdateBuff(BuffUpdateEvent args);
		void RemoveBuff(int buffId);
		void ClearBuffs();

		void SetPartyLeader(int leaderId);
		void UpdatePartyList(List<int> memberIds);
		void UpdatePartyMember(int memberId, Color color, Point? location);

		void PlaySound(SoundEvent args);
		void PlayMusic();

		void Exit();
	}
}

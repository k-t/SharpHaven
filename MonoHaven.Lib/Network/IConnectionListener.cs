using System.Collections.Generic;
using System.Drawing;
using MonoHaven.Network.Messages;

namespace MonoHaven.Network
{
	public interface IConnectionListener
	{
		void CreateWidget(CreateWidgetArgs args);
		void UpdateWidget(UpdateWidgetArgs args);
		void DestroyWidget(ushort widgetId);

		void BindResource(ResourceBinding binding);
		void BindTilesets(IEnumerable<TilesetBinding> bindings);

		void InvalidateMap();

		void UpdateCharAttributes(IEnumerable<CharAttribute> attributes);
		void UpdateTime(int time);
		void UpdateAmbientLight(Color color);
		void UpdateAstronomy(Astonomy astronomy);
		void UpdateActions(IEnumerable<ActionDelta> actions);
		void UpdateParty();
		void UpdateGob(GobChangeset changeset);
		void UpdateMap(MapData mapData);

		void AddBuff(BuffData buffData);
		void RemoveBuff(int buffId);
		void ClearBuffs();

		void PlaySound();
		void PlayMusic();

		void Exit();
	}
}

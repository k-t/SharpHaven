using System.Collections.Generic;
using System.Drawing;
using MonoHaven.Messages;

namespace MonoHaven.Network
{
	public interface IConnectionListener
	{
		void CreateWidget(WidgetCreateMessage message);
		void UpdateWidget(WidgetUpdateMessage message);
		void DestroyWidget(ushort widgetId);

		void BindResource(BindResourceMessage message);
		void BindTilesets(IEnumerable<BindTilesetMessage> bindings);

		void InvalidateMap();

		void UpdateCharAttributes(IEnumerable<CharAttributeMessage> attributes);
		void UpdateTime(int time);
		void UpdateAmbientLight(Color color);
		void UpdateAstronomy(AstronomyMessage astronomy);
		void UpdateActions(IEnumerable<ActionMessage> actions);
		void UpdateParty();
		void UpdateGob(UpdateGobMessage message);
		void UpdateMap(UpdateMapMessage updateMapMessage);

		void AddBuff(BuffAddMessage message);
		void RemoveBuff(int buffId);
		void ClearBuffs();

		void PlaySound();
		void PlayMusic();

		void Exit();
	}
}

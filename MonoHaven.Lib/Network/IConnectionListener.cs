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
		void InvalidateMap(Point gc);
		void InvalidateMap(Point ul, Point br);

		void UpdateCharAttributes(IEnumerable<CharAttributeMessage> attributes);
		void UpdateTime(int time);
		void UpdateAmbientLight(Color color);
		void UpdateAstronomy(AstronomyMessage astronomy);
		void UpdateActions(IEnumerable<ActionMessage> actions);
		void UpdateGob(UpdateGobMessage message);
		void UpdateMap(UpdateMapMessage updateMapMessage);

		void AddBuff(BuffAddMessage message);
		void RemoveBuff(int buffId);
		void ClearBuffs();

		void SetPartyLeader(int leaderId);
		void UpdatePartyList(List<int> memberIds);
		void UpdatePartyMember(int memberId, Color color, Point? location);

		void PlaySound(PlaySoundMessage message);
		void PlayMusic();

		void Exit();
	}
}

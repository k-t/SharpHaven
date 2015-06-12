using SharpHaven.Game.Events;

namespace SharpHaven.Game
{
	public interface IGameEventListener
	{
		void Handle(AmbientLightUpdateEvent args);
		void Handle(AstronomyUpdateEvent args);
		void Handle(CharAttributesUpdateEvent args);
		void Handle(GameActionsUpdateEvent args);
		void Handle(GameTimeUpdateEvent args);
		void Handle(GobUpdateEvent args);

		void Handle(BuffUpdateEvent args);
		void Handle(BuffRemoveEvent args);
		void Handle(BuffClearEvent args);

		void Handle(MapInvalidateEvent args);
		void Handle(MapInvalidateGridEvent args);
		void Handle(MapInvalidateRegionEvent args);
		void Handle(MapUpdateEvent args);

		void Handle(PartyLeaderChangeEvent args);
		void Handle(PartyUpdateEvent args);
		void Handle(PartyMemberUpdateEvent args);

		void Handle(PlaySoundEvent args);
		void Handle(PlayMusicEvent args);

		void Handle(WidgetCreateEvent args);
		void Handle(WidgetMessageEvent args);
		void Handle(WidgetDestroyEvent args);

		void Handle(ResourceLoadEvent args);
		void Handle(TilesetsLoadEvent args);

		void Exit();
	}
}

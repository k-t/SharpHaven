using System.Collections;
using System.Collections.Generic;

namespace SharpHaven.Client
{
	public class GobOverlayCollection : IEnumerable<GobOverlay>
	{
		private readonly List<GobOverlay> overlays;

		public GobOverlayCollection()
		{
			overlays = new List<GobOverlay>();
		}

		public void Add(GobOverlay overlay)
		{
			Remove(overlay.Id);
			overlays.Add(overlay);
		}

		public void Remove(GobOverlay overlay)
		{
			overlays.Remove(overlay);
		}

		public void Remove(int overlayId)
		{
			overlays.RemoveAll(x => x.Id == overlayId);
		}

		#region IEnumerable

		public IEnumerator<GobOverlay> GetEnumerator()
		{
			return overlays.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}

		#endregion
	}
}

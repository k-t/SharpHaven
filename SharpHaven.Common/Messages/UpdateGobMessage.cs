using System.Collections.Generic;

namespace SharpHaven.Messages
{
	public class UpdateGobMessage
	{
		private readonly List<GobDelta> deltas = new List<GobDelta>();

		public bool ReplaceFlag
		{
			get;
			set;
		}

		public int GobId
		{
			get;
			set;
		}

		public int Frame
		{
			get;
			set;
		}

		public IList<GobDelta> Deltas
		{
			get { return deltas; }
		}
	}
}

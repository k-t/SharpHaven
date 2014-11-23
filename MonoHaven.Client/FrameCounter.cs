using System;

namespace MonoHaven
{
	public class FrameCounter
	{
		private int framesPerSecond;
		private int frameCount;
		private DateTime lastUpdate;

		public int FramesPerSecond
		{
			get { return framesPerSecond; }
		}

		public void Update()
		{
			frameCount++;

			var now = DateTime.Now;
			if ((now - lastUpdate).TotalMilliseconds > 1000)
			{
				framesPerSecond = frameCount;
				frameCount = 0;
				lastUpdate = now;
			}
		}
	}
}

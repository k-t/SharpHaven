#region License
// Copyright (c) 2014 Marat Vildanov <marat.vildanov@gmail.com>
// Distributed under the MIT License.
// (See accompanying LICENSE file or copy at http://opensource.org/licenses/MIT)
#endregion

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

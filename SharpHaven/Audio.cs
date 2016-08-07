using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using NVorbis.OpenTKSupport;
using OpenTK.Audio;
using SharpHaven.Resources;
using SharpHaven.Utils;

namespace SharpHaven
{
	public class Audio : IDisposable
	{
		private readonly AudioContext context;
		private readonly OggStreamer streamer;
		private readonly AudioPlayer player;

		public Audio()
		{
			context = new AudioContext();
			streamer = new OggStreamer();
			player = new AudioPlayer();
			player.Run();
		}

		public void Play(Delayed<Resource> res)
		{
			player.Queue(res);
		}

		public void Dispose()
		{
			player.Stop();

			if (streamer != null)
				streamer.Dispose();
			if (context != null)
				context.Dispose();
		}

		private class AudioPlayer : BackgroundTask
		{
			private readonly Queue<Delayed<Resource>> queue;

			public AudioPlayer() : base("Audio Player")
			{
				queue = new Queue<Delayed<Resource>>();
			}

			protected override void OnStart()
			{
				while (!IsCancelled)
				{
					lock (queue)
					{
						var item = queue.Count > 0 ? queue.Peek() : null;
						if (item != null && item.Value != null)
						{
							queue.Dequeue();

							var audio = item.Value.GetLayer<AudioLayer>();
							if (audio != null)
							{
								var ms = new MemoryStream(audio.Bytes);
								var oggStream = new OggStream(ms);
								oggStream.Play();
							}
						}
						Monitor.Wait(queue, TimeSpan.FromMilliseconds(100));
					}
				}
			}

			public void Queue(Delayed<Resource> res)
			{
				lock (queue)
				{
					queue.Enqueue(res);
					Monitor.PulseAll(queue);
				}
			}
		}
	}
}

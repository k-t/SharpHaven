using System;
using System.IO;
using MonoHaven.Resources;
using MonoHaven.Resources.Layers;
using MonoHaven.Utils;
using NVorbis.OpenTKSupport;
using OpenTK.Audio;

namespace MonoHaven
{
	public class Audio : IDisposable
	{
		private readonly AudioContext context;
		private readonly OggStreamer streamer;

		public Audio()
		{
			context = new AudioContext();
			streamer = new OggStreamer();
		}

		public void Play(Delayed<Resource> res)
		{
			if (res.Value == null)
				return;

			var audio = res.Value.GetLayer<AudioData>();
			var ms = new MemoryStream(audio.Bytes);
			var oggStream = new OggStream(ms);
			oggStream.Play();
		}

		public void Dispose()
		{
			if (streamer != null)
				streamer.Dispose();
			if (context != null)
				context.Dispose();
		}
	}
}

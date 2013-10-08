using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using MonoHaven.Core;

namespace MonoHaven.Resources
{
	public class ChainedResourceLoader : IResourceLoader, IDisposable
	{
		private readonly Dictionary<string, Resource> cache;
		private readonly Worker firstWorker;

		public ChainedResourceLoader(IEnumerable<IResourceSource> sources)
			: this(new LayerFactory(), sources)
		{}

		public ChainedResourceLoader(
			ILayerFactory factory,
			IEnumerable<IResourceSource> sources)
		{
			if (sources == null)
				throw new ArgumentNullException("sources");

			cache = new Dictionary<string, Resource>();

			Worker next = null;
			foreach (var source in sources.Reverse())
			{
				var worker = new Worker(factory, next, source);
				next = worker;
			}
			this.firstWorker = next;
		}

		public IResource Load(string name, int version, int priority)
		{
			Resource res = null;
			lock (cache)
			{
				if (cache.TryGetValue(name, out res))
				{
					if (res.Version != -1 && version != -1)
					{
						if (res.Version < version)
						{
							res = null;
							cache.Remove(name);
						}
						else if (res.Version > version)
						{
							throw new Exception(string.Format(
								"Weird version number on {0} ({1} > {2}), loaded from {3}",
								res.Name, res.Version, version, res.Source));
						}
					}
					else if (version == -1 && res.HasError)
					{
						res = null;
						cache.Remove(name);
					}
				}
				if (res != null)
				{
					res.Priority = priority;
					return res;
				}
				res = new Resource(name, version);
				res.Priority = priority;
				cache.Add(name, res);
			}
			if (firstWorker != null)
				firstWorker.Load(res);
			return res;
		}

		public void Dispose()
		{
			if (firstWorker != null)
				firstWorker.Dispose();
		}

		private class Worker : IDisposable
		{
			private bool disposed = false;
			private readonly ILayerFactory factory;
			private readonly Worker next;
			private readonly IResourceSource src;
			private readonly PrioQueue<Resource> queue = new PrioQueue<Resource>();
			private readonly Thread thread;
			private readonly CancellationTokenSource cts;

			public Worker(ILayerFactory factory, Worker next, IResourceSource src)
			{
				this.factory = factory;
				this.next = next;
				this.src = src;
				this.cts = new CancellationTokenSource();
				this.thread = new Thread(Loop);
				this.thread.IsBackground = true;
				this.thread.Name = "Resource Loader";
				this.thread.Start(cts.Token);
			}

			public void Load(Resource resource)
			{
				lock (queue)
				{
					queue.Add(resource);
					Monitor.PulseAll(queue);
				}
			}

			private void Loop(object o)
			{
				var token = (CancellationToken)o;
				try
				{
					while (!token.IsCancellationRequested)
					{
						Resource res;
						lock (queue)
						{
							while ((res = queue.Poll()) == null)
								Monitor.Wait(queue);
						}
						Handle(res);
					}
				}
				catch (Exception)
				{
					// TODO: logging
				}
			}

			private void Handle(Resource res)
			{
				lock (res)
				{
					res.Error = null;
					res.Source = src.Name;
					try
					{
						using (var stream = src.Get(res.Name))
						{
							res.Load(factory, stream);
							res.IsLoading = false;
							Monitor.PulseAll(res);
						}
					}
					catch (Exception e)
					{
						if (next == null)
						{
							res.Error = e;
							res.IsLoading = false;
							Monitor.PulseAll(res);
						}
						else
						{
							next.Load(res);
						}
					}
				}
			}

			public void Dispose()
			{
				if (disposed)
					return;

				if (cts != null)
				{
					cts.Cancel();
					cts.Dispose();
				}
				if (next != null)
					next.Dispose();
				if (src != null)
					src.Dispose();

				disposed = true;
			}
		}
	}
}


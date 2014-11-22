#region License
// Copyright (c) 2014 Marat Vildanov <marat.vildanov@gmail.com>
// Distributed under the MIT License.
// (See accompanying LICENSE file or copy at http://opensource.org/licenses/MIT)
#endregion

using System;
using System.Collections.Generic;
using System.Linq;

namespace MonoHaven.Resources
{
	public class Resource
	{
		private int _version;
		private List<IDataLayer> _layers;

		public Resource(int version, IEnumerable<IDataLayer> layers)
		{
			_version = version;
			_layers = new List<IDataLayer>(layers);
		}

		public int Version
		{
			get { return _version; }
		}
		
		public T GetLayer<T>() where T : IDataLayer
		{
			return GetLayers<T>().FirstOrDefault();
		}

		public IEnumerable<T> GetLayers<T>() where T : IDataLayer
		{
			return _layers.OfType<T>();
		}
	}
}


#region License
// Copyright (c) 2014 Marat Vildanov <marat.vildanov@gmail.com>
// Distributed under the MIT License.
// (See accompanying LICENSE file or copy at http://opensource.org/licenses/MIT)
#endregion

namespace MonoHaven.Network
{
	public class ConnectionSettings
	{
		public string Host { get; set; }
		public int Port { get; set; }
		public string UserName { get; set; }
		public byte[] Cookie { get; set; }
	}
}

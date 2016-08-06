using System;
using System.Net.Security;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace SharpHaven.Net
{
	public class AuthClient : IDisposable
	{
		private const int CMD_USR = 1;
		private const int CMD_PASSWD = 2;
		private const int CMD_GETTOKEN = 3;
		private const int CMD_USETOKEN = 4;

		private readonly string host;
		private readonly int port;
		private SslStream ctx;

		public AuthClient(NetworkAddress address)
			: this(address.Host, address.Port)
		{
		}

		public AuthClient(string host, int port)
		{
			this.host = host;
			this.port = port;
		}

		private static bool ValidateServerCertificate(
			object sender,
			X509Certificate certificate,
			X509Chain chain,
			SslPolicyErrors sslPolicyErrors)
		{
			// TODO: proper certificate validation
			return true;
		}

		public void Connect()
		{
			var tc = new TcpClient(host, port);
			ctx = new SslStream(tc.GetStream(), false, ValidateServerCertificate, null);
			ctx.AuthenticateAsClient(host);
		}

		public void BindUser(string userName)
		{
			var msg = new Message(CMD_USR).Chars(userName);
			Send(msg);
			var reply = GetReply();
			if (reply.MessageType != 0)
				throw new AuthException("Unhandled reply " + reply.MessageType + " when binding username");
		}

		public bool TryToken(byte[] token, out byte[] cookie)
		{
			Send(new Message(CMD_USETOKEN).Bytes(token));
			var reply = GetReply();
			if (reply.MessageType != 0)
			{
				cookie = null;
				return false;
			}
			cookie = reply.GetBytes();
			return true;
		}

		public bool TryPassword(string password, out byte[] cookie)
		{
			byte[] phash = Digest(password);
			Send(new Message(CMD_PASSWD).Bytes(phash));
			var reply = GetReply();
			if (reply.MessageType != 0)
			{
				cookie = null;
				return false;
			}
			cookie = reply.GetBytes();
			return true;
		}

		public byte[] GetToken()
		{
			Send(new Message(CMD_GETTOKEN));
			var reply = GetReply();
			return reply.MessageType == 0 ? reply.Buffer : null;
		}

		private byte[] Digest(string password)
		{
			byte[] buf = Encoding.UTF8.GetBytes(password);
			SHA256 shaM = new SHA256Managed();
			return shaM.ComputeHash(buf);
		}

		private void ReadAll(byte[] buf)
		{
			int rv;
			for (int i = 0; i < buf.Length; i += rv)
			{
				rv = ctx.Read(buf, i, buf.Length - i);
				if (rv < 0)
					throw new AuthException("Premature end of input");
			}
		}

		private MessageReader GetReply()
		{
			byte[] header = new byte[2];
			ReadAll(header);
			byte[] buf = new byte[header[1]];
			ReadAll(buf);
			return new MessageReader(header[0], buf);
		}

		private void Send(Message msg)
		{
			if (msg.Length > 255)
				throw new AuthException("Message is too long (" + msg.Length + " bytes)");

			var bytes = new byte[msg.Length + 2];
			bytes[0] = msg.Type;
			bytes[1] = (byte)msg.Length;
			msg.CopyBytes(bytes, 2, msg.Length);

			ctx.Write(bytes);
		}

		#region IDisposable Members

		public void Dispose()
		{
			if (ctx != null)
				ctx.Dispose();
		}

		#endregion
	}
}


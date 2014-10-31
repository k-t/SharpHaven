using System;
using System.Net.Security;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace MonoHaven.Network
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
		private byte[] token;

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
			TcpClient tc = new TcpClient(host, port);
			ctx = new SslStream(tc.GetStream(), false, ValidateServerCertificate, null);
			ctx.AuthenticateAsClient(string.Empty);
		}

		public void BindUser(string userName)
		{
			using(var writer = new MessageWriter(CMD_USR))
			{
				writer.AddString2(userName);
				SendMessage(writer.GetMessage());
				Message reply = ReceiveMessage();
				if (reply.Type != 0)
				{
					throw new AuthException("Unhandled reply " + reply.Type + " when binding username");
				}
			}
		}

		public bool TryPassword(string password, out byte[] cookie)
		{
			byte[] phash = Digest(password);

			SendMessage(new Message(CMD_PASSWD, phash));
			Message reply = ReceiveMessage();

			if (reply.Type == 0)
			{
				cookie = reply.Data;
				return true;
			}
			else
			{
				cookie = null;
				return false;
			}
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

		private Message ReceiveMessage()
		{
			byte[] header = new byte[2];
			ReadAll(header);
			byte[] buf = new byte[header[1]];
			ReadAll(buf);
			return new Message(header[0], buf);
		}

		private void SendMessage(Message msg)
		{
			if (msg.Length > 255)
				throw new AuthException("Message is too long (" + msg.Length + " bytes)");

			byte[] buf = new byte[msg.Length + 2];
			buf[0] = (byte)msg.Type;
			buf[1] = (byte)msg.Length;
			Array.Copy(msg.Data, 0, buf, 2, msg.Length);

			ctx.Write(buf);
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


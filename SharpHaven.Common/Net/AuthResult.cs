namespace SharpHaven.Net
{
	public class AuthResult
	{
		private readonly bool isSuccessful;
		private readonly byte[] token;

		public AuthResult(bool isSuccessful, byte[] token)
		{
			this.isSuccessful = isSuccessful;
			this.token = token;
		}

		public bool IsSuccessful
		{
			get { return isSuccessful; }
		}

		public byte[] Token
		{
			get { return token; }
		}

		public static AuthResult Success(byte[] token = null)
		{
			return new AuthResult(true, token);
		}

		public static AuthResult Fail()
		{
			return new AuthResult(false, null);
		}
	}
}

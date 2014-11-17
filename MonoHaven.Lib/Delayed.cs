namespace MonoHaven
{
	public delegate bool Promise<T>(out T value);

	public class Delayed<T>
	{
		private T value;
		private bool bound;
		private readonly Promise<T> promise;

		public Delayed(T value)
		{
			this.bound = true;
			this.value = value;
		}

		public Delayed(Promise<T> promise)
		{
			this.promise = promise;
			this.value = default(T);
		}

		public T Value
		{
			get
			{
				if (!bound && promise(out value))
					bound = true;
				return value;
			}
		}
	}
}

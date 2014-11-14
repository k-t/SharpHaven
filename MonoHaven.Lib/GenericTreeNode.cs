using System;
using System.Collections.Generic;
using System.Linq;

namespace MonoHaven
{
	public class GenericTreeNode<T> : TreeNode<GenericTreeNode<T>>
	{
		private static readonly Func<T, T, bool> comparer = EqualityComparer<T>.Default.Equals;

		public GenericTreeNode() : this(default(T))
		{
		}

		public GenericTreeNode(T value)
		{
			Value = value;
		}

		public T Value { get; set; }

		public GenericTreeNode<T> Find(T value)
		{
			return Children.FirstOrDefault(x => comparer(x.Value, value));
		}
	}
}

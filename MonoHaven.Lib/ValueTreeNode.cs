#region License
// Copyright (c) 2014 Marat Vildanov <marat.vildanov@gmail.com>
// Distributed under the MIT License.
// (See accompanying LICENSE file or copy at http://opensource.org/licenses/MIT)
#endregion

using System;
using System.Collections.Generic;
using System.Linq;

namespace MonoHaven
{
	public class ValueTreeNode<T> : TreeNode<ValueTreeNode<T>>
	{
		private static readonly Func<T, T, bool> comparer = EqualityComparer<T>.Default.Equals;

		public ValueTreeNode() : this(default(T))
		{
		}

		public ValueTreeNode(T value)
		{
			Value = value;
		}

		public T Value
		{
			get;
			set;
		}

		public ValueTreeNode<T> Find(T value)
		{
			return Children.FirstOrDefault(x => comparer(x.Value, value));
		}
	}
}

using System;
using System.Collections.Generic;
using System.Linq;

namespace MonoHaven
{
	public class TreeNode<T>
	{
		private static readonly Func<T, T, bool> comparer = EqualityComparer<T>.Default.Equals;

		private readonly T value;

		private TreeNode<T> parent;
		private TreeNode<T> prev;
		private TreeNode<T> next;
		private TreeNode<T> firstChild;
		private TreeNode<T> lastChild;

		public TreeNode()
		{
		}

		public TreeNode(T value)
		{
			this.value = value;
		}

		public TreeNode<T> Parent
		{
			get { return parent; }
		}

		public IEnumerable<TreeNode<T>> Children
		{
			get
			{
				for (var child = firstChild; child != null; child = child.next)
					yield return child;
			}
		}

		public IEnumerable<TreeNode<T>> Descendants
		{
			get
			{
				var stack = new Stack<TreeNode<T>>(Children);
				while (stack.Any())
				{
					var node = stack.Pop();
					yield return node;
					foreach (var child in node.Children)
						stack.Push(child);
				}
			}
		}

		public T Value
		{
			get { return value; }
		}

		/// <summary>
		/// Adds specified node as a child to this node.
		/// </summary>
		public TreeNode<T> AddChild(TreeNode<T> node)
		{
			node.Remove();

			node.parent = this;

			if (this.firstChild == null)
				this.firstChild = node;

			if (this.lastChild != null)
			{
				node.prev = this.lastChild;
				this.lastChild.next = node;
				this.lastChild = node;
			}
			else
				this.lastChild = node;

			return this;
		}

		/// <summary>
		/// Adds range of children nodes to this node.
		/// </summary>
		public TreeNode<T> AddChildren(IEnumerable<TreeNode<T>> children)
		{
			foreach (var child in children)
				AddChild(child);
			return this;
		}

		/// <summary>
		/// Adds range of children nodes to this node.
		/// </summary>
		public TreeNode<T> AddChildren(params TreeNode<T>[] children)
		{
			return AddChildren(children.AsEnumerable());
		}

		/// <summary>
		/// Removes this node from the parent node.
		/// </summary>
		public void Remove()
		{
			if (parent != null)
			{
				if (parent.firstChild == this)
					parent.firstChild = next;
				if (parent.lastChild == this)
					parent.lastChild = prev;
			}
			if (next != null) next.prev = prev;
			if (prev != null) prev.next = next;
			parent = null;
		}

		public TreeNode<T> FindChild(T value)
		{
			return Children.FirstOrDefault(x => comparer(x.Value, value));
		}
	}
}

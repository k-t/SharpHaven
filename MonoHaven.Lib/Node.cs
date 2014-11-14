using System;
using System.Collections.Generic;
using System.Linq;

namespace MonoHaven
{
	public class Node<T>
	{
		private static readonly Func<T, T, bool> comparer = EqualityComparer<T>.Default.Equals;

		private readonly T value;

		private Node<T> parent;
		private Node<T> prev;
		private Node<T> next;
		private Node<T> firstChild;
		private Node<T> lastChild;

		public Node()
		{
		}

		public Node(T value)
		{
			this.value = value;
		}

		public Node<T> Parent
		{
			get { return parent; }
		}

		public IEnumerable<Node<T>> Children
		{
			get
			{
				for (var child = firstChild; child != null; child = child.next)
					yield return child;
			}
		}

		public IEnumerable<Node<T>> Descendants
		{
			get
			{
				var stack = new Stack<Node<T>>(Children);
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
		public Node<T> AddChild(Node<T> node)
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
		public Node<T> AddChildren(IEnumerable<Node<T>> children)
		{
			foreach (var child in children)
				AddChild(child);
			return this;
		}

		/// <summary>
		/// Adds range of children nodes to this node.
		/// </summary>
		public Node<T> AddChildren(params Node<T>[] children)
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

		public Node<T> FindChild(T value)
		{
			return Children.FirstOrDefault(x => comparer(x.Value, value));
		}
	}
}

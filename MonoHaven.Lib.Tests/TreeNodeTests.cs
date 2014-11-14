﻿using System.Linq;
using NUnit.Framework;

namespace MonoHaven.Tests
{
	[TestFixture]
	public class TreeNodeTests
	{
		[Test]
		public void AddChildWorks()
		{
			GenericTreeNode<int> n, n1, n2, n3;

			n = new GenericTreeNode<int>(0);
			n.AddChild(n1 = new GenericTreeNode<int>(1));
			n.AddChild(n2 = new GenericTreeNode<int>(2));
			n.AddChild(n3 = new GenericTreeNode<int>(3));

			Assert.That(n.Children, Is.EquivalentTo(new [] { n2, n1, n3 }),
				"Invalid children collection");

			Assert.IsTrue(
				n.Children.All(x => x.Parent == n),
				"Parent node is not assigned");
		}

		[Test]
		public void AddChildReturnsThis()
		{
			GenericTreeNode<int> n1, n2, n3;

			n1 = new GenericTreeNode<int>(0);
			n2 = new GenericTreeNode<int>(1);
			n3 = n1.AddChild(n2);

			Assert.AreSame(n1, n3);
		}

		[Test]
		public void AddChildrenWorks()
		{
			var node = new GenericTreeNode<int>(0);
			var children = GenerateNodes(3);

			node.AddChildren(children);

			Assert.That(node.Children, Is.EquivalentTo(children),
				"Invalid children collection");

			Assert.IsTrue(
				node.Children.All(x => x.Parent == node),
				"Parent node is not assigned");
		}

		[Test]
		public void AddChildrenReturnsThis()
		{
			var n1 = new GenericTreeNode<int>(0);
			var children = GenerateNodes(3);
			var n2 = n1.AddChildren(children);

			Assert.AreSame(n1, n2);
		}

		[Test]
		public void RemoveChildWorks()
		{
			GenericTreeNode<int> n, n1, n2, n3, n4;

			n = new GenericTreeNode<int>(0);
			n.AddChild(n1 = new GenericTreeNode<int>(1));
			n.AddChild(n2 = new GenericTreeNode<int>(2));
			n.AddChild(n3 = new GenericTreeNode<int>(3));
			n.AddChild(n4 = new GenericTreeNode<int>(4));
			n2.Remove();

			Assert.IsNull(n2.Parent, "Parent node is not set to null");

			Assert.That(n.Children, Is.EquivalentTo(new [] { n1, n3, n4 }),
				"Invalid children collection after removal");
		}

		[Test]
		public void DescendantsWorks()
		{
			var node = new GenericTreeNode<int>(0);
			var nodes = GenerateNodes(15);

			node.AddChildren(
				nodes[0].AddChild(
					nodes[1].AddChild(
						nodes[2])),
				nodes[3].AddChildren(
					nodes[4],
					nodes[5].AddChild(
						nodes[6]).AddChildren(
							nodes[7],
							nodes[8].AddChild(
								nodes[9])),
					nodes[10].AddChild(
						nodes[11]),
					nodes[12]),
				nodes[13].AddChild(
					nodes[14]));

			var a = node.Descendants.ToArray();

			Assert.That(node.Descendants, Is.EquivalentTo(nodes),
				"Invalid descendant collection");
		}

		#region Helpers

		private static GenericTreeNode<int>[] GenerateNodes(int count)
		{
			var children = new GenericTreeNode<int>[count];
			for (int i = 0; i < count; i++)
				children[i] = new GenericTreeNode<int>(i + 1);
			return children;
		}

		#endregion
	}
}

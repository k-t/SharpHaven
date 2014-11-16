using System.Collections.Generic;
using System.Drawing;
using C5;
using MonoHaven.Resources;

namespace MonoHaven.Game
{
	public class GameObjectCache : IEnumerable<GameObject>
	{
		private readonly TreeDictionary<int, GameObject> objects;

		public GameObjectCache()
		{
			objects = new TreeDictionary<int, GameObject>();
		}

		private GameObject Get(int id, int frame)
		{
			GameObject gob;
			if (!objects.Find(ref id, out gob))
			{
				gob = new GameObject();
				objects[id] = gob;
			}
			return gob;
		}

		public void Remove(int id, int frame)
		{
			objects.Remove(id);
		}

		public void Move(int id, int frame, Point position)
		{
			var gob = Get(id, frame);
			if (gob == null)
				return;
			gob.Position = position;
		}

		public void ChangeResource(int id, int frame, Resource res, byte[] spriteData)
		{
		}

		public void StartMove(int id, int frame, Point orig, Point dest, int time)
		{
		}

		public void AdjustMove(int id, int frame, int time)
		{
		}

		public void StopMove(int id, int frame)
		{
		}

		public void Speak(int id, int frame, Point offset, string text)
		{
		}

		public void SetLayers(
			int id,
			int frame,
			Resource baseRes,
			IEnumerable<Resource> layers)
		{
		}

		public void SetAvatar(int id, int frame, IEnumerable<Resource> layers)
		{
		}

		public void SetDrawOffset(int id, int frame, Point offset)
		{
			var gob = Get(id, frame);
			if (gob == null)
				return;
			gob.DrawOffset = offset;
		}

		public void Light(int id, int frame, Point offset, int size, byte intensity)
		{
		}

		public void Follow(int id, int frame, int oid, int szo, Point offset)
		{
		}

		public void Unfollow(int id, int frame)
		{
		}

		public void ShootAt(int id, int frame, int oid, Point target, int velocity)
		{
		}

		public void ShootAt(int id, int frame, Point target, int velocity)
		{
		}

		public void StopShot(int id, int frame)
		{
		}

		public void SetHealth(int id, int frame, byte hp)
		{
		}

		public void SetBuddy(int id, int frame, string name, byte group, byte type)
		{
		}

		public void SetOverlay(
			int id,
			int frame,
			int overlayId,
			bool prs,
			Resource res,
			byte[] spriteData)
		{
		}

		public IEnumerator<GameObject> GetEnumerator()
		{
			return objects.Values.GetEnumerator();
		}

		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}
	}
}

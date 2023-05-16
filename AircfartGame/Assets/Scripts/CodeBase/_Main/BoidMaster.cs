using UnityEngine;

namespace CodeBase._Main
{
	public class BoidMaster : MonoBehaviour
	{
		private void Start()
		{
			for (int i = 0; i < spawnCount; i++)
			{
				Spawn();
			}
		}

		public GameObject Spawn()
		{
			return Spawn(transform.position + Random.insideUnitSphere * spawnRadius);
		}

		public GameObject Spawn(Vector3 position)
		{
			Quaternion rotation = Quaternion.Slerp(transform.rotation, Random.rotation, 0.25f);
			GameObject gameObject = Instantiate(boidPrefab, position, rotation) as GameObject;
			gameObject.GetComponent<BoidUnit>().master = this;
			if (transform.parent != null)
			{
				gameObject.transform.parent = transform.parent;
			}
			return gameObject;
		}

		public GameObject boidPrefab;

		public int spawnCount = 10;

		public float spawnRadius = 100f;

		public float neighborDistance = 10f;

		public float speed = 10f;

		public float speedVariation = 1f;

		public float rotationCoefficient = 5f;

		public LayerMask searchLayer;
	}
}

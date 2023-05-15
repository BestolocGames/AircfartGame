// dnSpy decompiler from Assembly-CSharp.dll class: FlightKit.BoidMaster
using System;
using UnityEngine;

namespace FlightKit
{
	public class BoidMaster : MonoBehaviour
	{
		private void Start()
		{
			for (int i = 0; i < this.spawnCount; i++)
			{
				this.Spawn();
			}
		}

		public GameObject Spawn()
		{
			return this.Spawn(base.transform.position + UnityEngine.Random.insideUnitSphere * this.spawnRadius);
		}

		public GameObject Spawn(Vector3 position)
		{
			Quaternion rotation = Quaternion.Slerp(base.transform.rotation, UnityEngine.Random.rotation, 0.25f);
			GameObject gameObject = UnityEngine.Object.Instantiate(this.boidPrefab, position, rotation) as GameObject;
			gameObject.GetComponent<BoidUnit>().master = this;
			if (base.transform.parent != null)
			{
				gameObject.transform.parent = base.transform.parent;
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

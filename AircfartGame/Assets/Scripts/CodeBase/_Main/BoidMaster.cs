using UnityEngine;
using UnityEngine.Serialization;

namespace CodeBase._Main
{
	public class BoidMaster : MonoBehaviour
	{
		private void Start()
		{
			for (int i = 0; i < _spawnCount; i++)
			{
				Spawn();
			}
		}

		public GameObject Spawn()
		{
			return Spawn(transform.position + Random.insideUnitSphere * _spawnRadius);
		}

		public GameObject Spawn(Vector3 position)
		{
			Quaternion rotation = Quaternion.Slerp(transform.rotation, Random.rotation, 0.25f);
			GameObject gameObject = Instantiate(_boidPrefab, position, rotation) as GameObject;
			gameObject.GetComponent<BoidUnit>()._master = this;
			if (transform.parent != null)
			{
				gameObject.transform.parent = transform.parent;
			}
			return gameObject;
		}

		[FormerlySerializedAs("boidPrefab")] public GameObject _boidPrefab;

		[FormerlySerializedAs("spawnCount")] public int _spawnCount = 10;

		[FormerlySerializedAs("spawnRadius")] public float _spawnRadius = 100f;

		[FormerlySerializedAs("neighborDistance")] public float _neighborDistance = 10f;

		[FormerlySerializedAs("speed")] public float _speed = 10f;

		[FormerlySerializedAs("speedVariation")] public float _speedVariation = 1f;

		[FormerlySerializedAs("rotationCoefficient")] public float _rotationCoefficient = 5f;

		[FormerlySerializedAs("searchLayer")] public LayerMask _searchLayer;
	}
}

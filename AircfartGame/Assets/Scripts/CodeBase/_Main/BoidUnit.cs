using UnityEngine;
using UnityEngine.Serialization;

namespace CodeBase._Main
{
	public class BoidUnit : MonoBehaviour
	{
		private void Update()
		{
			Vector3 position = this.transform.position;
			Quaternion rotation = this.transform.rotation;
			Vector3 a = Vector3.zero;
			Vector3 vector = _master.transform.forward;
			Vector3 vector2 = _master.transform.position;
			Collider[] array = Physics.OverlapSphere(position, _master._neighborDistance, _master._searchLayer);
			foreach (Collider collider in array)
			{
				if (!(collider.gameObject == gameObject))
				{
					Transform transform = collider.transform;
					a += GetSeparationVector(transform);
					vector += transform.forward;
					vector2 += transform.position;
				}
			}
			float d = 1f / (float)array.Length;
			vector *= d;
			vector2 *= d;
			vector2 = (vector2 - position).normalized;
			Vector3 vector3 = a + vector + vector2;
			Quaternion quaternion = Quaternion.FromToRotation(Vector3.forward, vector3.normalized);
			if (quaternion != rotation)
			{
				float t = Mathf.Exp(-_master._rotationCoefficient * Time.deltaTime);
				transform.rotation = Quaternion.Slerp(quaternion, rotation, t);
			}
			float num = Mathf.PerlinNoise(Time.time, Random.value * 10f) * 2f - 1f;
			float d2 = _master._speed * (1f + num * _master._speedVariation);
			this.transform.position = position + this.transform.forward * d2 * Time.deltaTime;
		}

		private Vector3 GetSeparationVector(Transform target)
		{
			Vector3 a = transform.position - target.transform.position;
			float magnitude = a.magnitude;
			float num = Mathf.Clamp01(1f - magnitude / _master._neighborDistance);
			return a * (num / magnitude);
		}

		[FormerlySerializedAs("master")] public BoidMaster _master;
	}
}

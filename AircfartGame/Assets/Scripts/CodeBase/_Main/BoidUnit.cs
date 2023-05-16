using UnityEngine;

namespace CodeBase._Main
{
	public class BoidUnit : MonoBehaviour
	{
		private void Update()
		{
			Vector3 position = this.transform.position;
			Quaternion rotation = this.transform.rotation;
			Vector3 a = Vector3.zero;
			Vector3 vector = master.transform.forward;
			Vector3 vector2 = master.transform.position;
			Collider[] array = Physics.OverlapSphere(position, master.neighborDistance, master.searchLayer);
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
				float t = Mathf.Exp(-master.rotationCoefficient * Time.deltaTime);
				transform.rotation = Quaternion.Slerp(quaternion, rotation, t);
			}
			float num = Mathf.PerlinNoise(Time.time, Random.value * 10f) * 2f - 1f;
			float d2 = master.speed * (1f + num * master.speedVariation);
			this.transform.position = position + this.transform.forward * d2 * Time.deltaTime;
		}

		private Vector3 GetSeparationVector(Transform target)
		{
			Vector3 a = transform.position - target.transform.position;
			float magnitude = a.magnitude;
			float num = Mathf.Clamp01(1f - magnitude / master.neighborDistance);
			return a * (num / magnitude);
		}

		public BoidMaster master;
	}
}

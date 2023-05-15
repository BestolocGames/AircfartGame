// dnSpy decompiler from Assembly-CSharp.dll class: FlightKit.BoidUnit
using System;
using UnityEngine;

namespace FlightKit
{
	public class BoidUnit : MonoBehaviour
	{
		private void Update()
		{
			Vector3 position = base.transform.position;
			Quaternion rotation = base.transform.rotation;
			Vector3 a = Vector3.zero;
			Vector3 vector = this.master.transform.forward;
			Vector3 vector2 = this.master.transform.position;
			Collider[] array = Physics.OverlapSphere(position, this.master.neighborDistance, this.master.searchLayer);
			foreach (Collider collider in array)
			{
				if (!(collider.gameObject == base.gameObject))
				{
					Transform transform = collider.transform;
					a += this.GetSeparationVector(transform);
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
				float t = Mathf.Exp(-this.master.rotationCoefficient * Time.deltaTime);
				base.transform.rotation = Quaternion.Slerp(quaternion, rotation, t);
			}
			float num = Mathf.PerlinNoise(Time.time, UnityEngine.Random.value * 10f) * 2f - 1f;
			float d2 = this.master.speed * (1f + num * this.master.speedVariation);
			base.transform.position = position + base.transform.forward * d2 * Time.deltaTime;
		}

		private Vector3 GetSeparationVector(Transform target)
		{
			Vector3 a = base.transform.position - target.transform.position;
			float magnitude = a.magnitude;
			float num = Mathf.Clamp01(1f - magnitude / this.master.neighborDistance);
			return a * (num / magnitude);
		}

		public BoidMaster master;
	}
}

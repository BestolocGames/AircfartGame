using System.Collections;
using UnityEngine;

namespace Utility
{
	public class ObjectResetter : MonoBehaviour
	{
		private void Start()
		{
			originalPosition = transform.position;
			originalRotation = transform.rotation;
			Rigidbody = GetComponent<Rigidbody>();
		}

		public void DelayedReset(float delay)
		{
			StartCoroutine(ResetCoroutine(delay));
		}

		public IEnumerator ResetCoroutine(float delay)
		{
			yield return new WaitForSeconds(delay);
			transform.position = originalPosition;
			transform.rotation = originalRotation;
			if (Rigidbody)
			{
				Rigidbody.velocity = Vector3.zero;
				Rigidbody.angularVelocity = Vector3.zero;
			}
			SendMessage("Reset");
			yield break;
		}

		private Vector3 originalPosition;

		private Quaternion originalRotation;

		private Rigidbody Rigidbody;
	}
}

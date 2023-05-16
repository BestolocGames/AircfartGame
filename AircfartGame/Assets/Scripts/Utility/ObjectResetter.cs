using System.Collections;
using UnityEngine;

namespace Utility
{
	public class ObjectResetter : MonoBehaviour
	{
		private Vector3 _originalPosition;

		private Quaternion _originalRotation;

		private Rigidbody _rigidbody;
		
		private void Start()
		{
			_originalPosition = transform.position;
			_originalRotation = transform.rotation;
			_rigidbody = GetComponent<Rigidbody>();
		}

		public void DelayedReset(float delay) => 
			StartCoroutine(ResetCoroutine(delay));

		private IEnumerator ResetCoroutine(float delay)
		{
			yield return new WaitForSeconds(delay);
			transform.position = _originalPosition;
			transform.rotation = _originalRotation;
			if (_rigidbody)
			{
				_rigidbody.velocity = Vector3.zero;
				_rigidbody.angularVelocity = Vector3.zero;
			}
			SendMessage("Reset");
			yield break;
		}
	}
}

// dnSpy decompiler from Assembly-CSharp.dll class: UnityStandardAssets.Utility.ObjectResetter
using System;
using System.Collections;
using UnityEngine;

namespace UnityStandardAssets.Utility
{
	public class ObjectResetter : MonoBehaviour
	{
		private void Start()
		{
			this.originalPosition = base.transform.position;
			this.originalRotation = base.transform.rotation;
			this.Rigidbody = base.GetComponent<Rigidbody>();
		}

		public void DelayedReset(float delay)
		{
			base.StartCoroutine(this.ResetCoroutine(delay));
		}

		public IEnumerator ResetCoroutine(float delay)
		{
			yield return new WaitForSeconds(delay);
			base.transform.position = this.originalPosition;
			base.transform.rotation = this.originalRotation;
			if (this.Rigidbody)
			{
				this.Rigidbody.velocity = Vector3.zero;
				this.Rigidbody.angularVelocity = Vector3.zero;
			}
			base.SendMessage("Reset");
			yield break;
		}

		private Vector3 originalPosition;

		private Quaternion originalRotation;

		private Rigidbody Rigidbody;
	}
}

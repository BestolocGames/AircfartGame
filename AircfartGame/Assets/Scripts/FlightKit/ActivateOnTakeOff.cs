// dnSpy decompiler from Assembly-CSharp.dll class: FlightKit.ActivateOnTakeOff
using System;
using UnityEngine;

namespace FlightKit
{
	public class ActivateOnTakeOff : MonoBehaviour
	{
		private void OnEnable()
		{
			TakeOffPublisher.OnTakeOffEvent += this.OnTakeOff;
		}

		private void OnDisable()
		{
			TakeOffPublisher.OnTakeOffEvent -= this.OnTakeOff;
		}

		private void OnTakeOff()
		{
			base.Invoke("OnTakeOffCore", this.delay);
		}

		private void OnTakeOffCore()
		{
			foreach (GameObject gameObject in this.objectsToActivate)
			{
				if (gameObject != null)
				{
					gameObject.SetActive(true);
				}
			}
		}

		public float delay;

		public GameObject[] objectsToActivate;
	}
}

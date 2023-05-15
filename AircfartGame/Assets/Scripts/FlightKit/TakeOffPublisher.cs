// dnSpy decompiler from Assembly-CSharp.dll class: FlightKit.TakeOffPublisher
using System;
using UnityEngine;

namespace FlightKit
{
	public class TakeOffPublisher : MonoBehaviour
	{
		public static event GameActions.SimpleAction OnTakeOffEvent;

		private void OnCollisionEnter(Collision collision)
		{
			float num = Time.time - this._collisionEnterTime;
			bool flag = num > 10f || this._collisionEnterTime < 0f;
			if (flag && collision.gameObject.CompareTag(Tags.TakeOffPlatform))
			{
				this._collisionEnterTime = Time.time;
			}
		}

		private void OnCollisionExit(Collision collision)
		{
			float num = Time.time - this._collisionEnterTime;
			if (num > 1f && collision.gameObject.CompareTag(Tags.TakeOffPlatform) && TakeOffPublisher.OnTakeOffEvent != null)
			{
				TakeOffPublisher.OnTakeOffEvent();
			}
		}

		private const float MIN_LANDING_DURATION = 1f;

		private const float MIN_TIME_BETWEEN_LANDINGS = 10f;

		private float _collisionEnterTime = -1f;
	}
}

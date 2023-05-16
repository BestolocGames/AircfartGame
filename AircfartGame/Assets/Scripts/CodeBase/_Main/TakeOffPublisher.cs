using UnityEngine;

namespace CodeBase._Main
{
	public class TakeOffPublisher : MonoBehaviour
	{
		public static event GameActions.SimpleAction OnTakeOffEvent;

		private void OnCollisionEnter(Collision collision)
		{
			float num = Time.time - _collisionEnterTime;
			bool flag = num > 10f || _collisionEnterTime < 0f;
			if (flag && collision.gameObject.CompareTag(Tags.TakeOffPlatform))
			{
				_collisionEnterTime = Time.time;
			}
		}

		private void OnCollisionExit(Collision collision)
		{
			float num = Time.time - _collisionEnterTime;
			if (num > 1f && collision.gameObject.CompareTag(Tags.TakeOffPlatform) && OnTakeOffEvent != null)
			{
				OnTakeOffEvent();
			}
		}

		private const float MIN_LANDING_DURATION = 1f;

		private const float MIN_TIME_BETWEEN_LANDINGS = 10f;

		private float _collisionEnterTime = -1f;
	}
}

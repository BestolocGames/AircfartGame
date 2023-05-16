using System.Collections;
using CodeBase._ImageEffects;
using UnityEngine;
using Utility;

namespace CodeBase._Main.Player
{
	public class CrashController : MonoBehaviour
	{
		public float crashImpulse = 10f;

		public float soundImpulse = 5f;

		[Space]
		public AudioClip crash;

		public AudioClip hit;

		private AudioSource _soundSource;
		
		private void Start() => 
			_soundSource = GetComponent<AudioSource>();

		public void OnCollisionEnter(Collision collision)
		{
			float magnitude = collision.impulse.magnitude;
			if (!collision.gameObject.CompareTag(Tags.TakeOffPlatform) && magnitude > soundImpulse) 
				RegisterHit();
			if (magnitude > crashImpulse) 
				RegisterCrash();
		}

		private void RegisterHit()
		{
			if (_soundSource != null && _soundSource.isActiveAndEnabled)
			{
				_soundSource.PlayOneShot(crash);
			}
			StartCoroutine(CollisionCameraAnimation());
		}

		private void RegisterCrash()
		{
			if (_soundSource != null)
			{
				_soundSource.PlayOneShot(hit);
			}
			AirplaneTrails component = GetComponent<AirplaneTrails>();
			component.DeactivateTrails();
			component.ClearTrails();
			ObjectResetter component2 = GetComponent<ObjectResetter>();
			if (component2 != null)
			{
				component2.DelayedReset(0.4f);
			}
		}

		private IEnumerator CollisionCameraAnimation()
		{
			NoiseAndScratches cameraFx = FindObjectOfType<NoiseAndScratches>();
			cameraFx.enabled = true;
			yield return new WaitForSeconds(1f);
			cameraFx.enabled = false;
			yield break;
		}
	}
}

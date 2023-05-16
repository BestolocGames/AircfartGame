using System.Collections;
using CodeBase._ImageEffects;
using UnityEngine;
using UnityEngine.Serialization;
using Utility;

namespace CodeBase._Main.Player
{
	public class CrashController : MonoBehaviour
	{
		[FormerlySerializedAs("crashImpulse")] public float _crashImpulse = 10f;

		[FormerlySerializedAs("soundImpulse")] public float _soundImpulse = 5f;

		[FormerlySerializedAs("crash")] [Space]
		public AudioClip _crash;

		[FormerlySerializedAs("hit")] public AudioClip _hit;

		private AudioSource _soundSource;
		
		private void Start() => 
			_soundSource = GetComponent<AudioSource>();

		public void OnCollisionEnter(Collision collision)
		{
			float magnitude = collision.impulse.magnitude;
			if (!collision.gameObject.CompareTag(Tags.TakeOffPlatform) && magnitude > _soundImpulse) 
				RegisterHit();
			if (magnitude > _crashImpulse) 
				RegisterCrash();
		}

		private void RegisterHit()
		{
			if (_soundSource != null && _soundSource.isActiveAndEnabled)
			{
				_soundSource.PlayOneShot(_crash);
			}
			StartCoroutine(CollisionCameraAnimation());
		}

		private void RegisterCrash()
		{
			if (_soundSource != null)
			{
				_soundSource.PlayOneShot(_hit);
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

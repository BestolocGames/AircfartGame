// dnSpy decompiler from Assembly-CSharp.dll class: FlightKit.CrashController
using System;
using System.Collections;
using UnityEngine;
using UnityStandardAssets.ImageEffects;
using UnityStandardAssets.Utility;

namespace FlightKit
{
	public class CrashController : MonoBehaviour
	{
		private void Start()
		{
			this._soundSource = base.GetComponent<AudioSource>();
		}

		public void OnCollisionEnter(Collision collision)
		{
			float magnitude = collision.impulse.magnitude;
			if (!collision.gameObject.CompareTag(Tags.TakeOffPlatform) && magnitude > this.soundImpulse)
			{
				this.RegisterHit();
			}
			if (magnitude > this.crashImpulse)
			{
				this.RegisterCrash();
			}
		}

		private void RegisterHit()
		{
			if (this._soundSource != null && this._soundSource.isActiveAndEnabled)
			{
				this._soundSource.PlayOneShot(this.crash);
			}
			base.StartCoroutine(this.CollisionCameraAnimation());
		}

		private void RegisterCrash()
		{
			if (this._soundSource != null)
			{
				this._soundSource.PlayOneShot(this.hit);
			}
			AirplaneTrails component = base.GetComponent<AirplaneTrails>();
			component.DeactivateTrails();
			component.ClearTrails();
			ObjectResetter component2 = base.GetComponent<ObjectResetter>();
			if (component2 != null)
			{
				component2.DelayedReset(0.4f);
			}
		}

		private IEnumerator CollisionCameraAnimation()
		{
			NoiseAndScratches cameraFx = UnityEngine.Object.FindObjectOfType<NoiseAndScratches>();
			cameraFx.enabled = true;
			yield return new WaitForSeconds(1f);
			cameraFx.enabled = false;
			yield break;
		}

		public float crashImpulse = 10f;

		public float soundImpulse = 5f;

		[Space]
		public AudioClip crash;

		public AudioClip hit;

		private AudioSource _soundSource;
	}
}

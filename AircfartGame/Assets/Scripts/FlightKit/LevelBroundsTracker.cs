// dnSpy decompiler from Assembly-CSharp.dll class: FlightKit.LevelBroundsTracker
using System;
using System.Collections;
using UnityEngine;
using UnityStandardAssets.ImageEffects;
using UnityStandardAssets.Utility;

namespace FlightKit
{
	public class LevelBroundsTracker : MonoBehaviour
	{
		private void Start()
		{
			this._soundSource = base.GetComponent<AudioSource>();
		}

		private void OnTriggerEnter(Collider collider)
		{
			if (collider.gameObject.CompareTag(this.levelBoundsTag))
			{
				this._currentSensorsCount++;
			}
		}

		private void OnTriggerExit(Collider collider)
		{
			if (collider.gameObject.CompareTag(this.levelBoundsTag))
			{
				this._currentSensorsCount--;
				if (this._currentSensorsCount <= 0)
				{
					this.RegisterAbandonedLevel();
				}
			}
		}

		private void RegisterAbandonedLevel()
		{
			if (this._soundSource != null && this.resetSound != null)
			{
				this._soundSource.PlayOneShot(this.resetSound);
			}
			AirplaneTrails component = base.GetComponent<AirplaneTrails>();
			component.DeactivateTrails();
			component.ClearTrails();
			base.StartCoroutine(this.FadeOutCoroutine());
		}

		private IEnumerator FadeOutCoroutine()
		{
			BloomOptimized bloom = UnityEngine.Object.FindObjectOfType<BloomOptimized>();
			if (bloom == null)
			{
				this.ResetAirplane();
				yield break;
			}
			float targetIntensity = 2.5f;
			WaitForEndOfFrame wait = new WaitForEndOfFrame();
			float tween = 1f;
			float initIntensity = bloom.intensity;
			float initThreshold = bloom.threshold;
			while ((double)tween > 0.1)
			{
				bloom.intensity = Mathf.Lerp(bloom.intensity, targetIntensity, 1.5f * Time.deltaTime);
				bloom.threshold = Mathf.Lerp(bloom.threshold, 0f, 1.5f * Time.deltaTime);
				tween = Mathf.Lerp(tween, 0f, 1.5f * Time.deltaTime);
				yield return wait;
			}
			this.ResetAirplane();
			targetIntensity = initIntensity;
			tween = 1f;
			while ((double)tween > 0.1)
			{
				bloom.intensity = Mathf.Lerp(bloom.intensity, targetIntensity, 2f * Time.deltaTime);
				bloom.threshold = Mathf.Lerp(bloom.threshold, 0f, 2f * Time.deltaTime);
				tween = Mathf.Lerp(tween, 0f, 3f * Time.deltaTime);
				yield return wait;
			}
			bloom.intensity = initIntensity;
			bloom.threshold = initThreshold;
			yield break;
		}

		private void ResetAirplane()
		{
			ObjectResetter component = base.GetComponent<ObjectResetter>();
			if (component != null)
			{
				component.DelayedReset(0f);
			}
		}

		public string levelBoundsTag;

		private int _currentSensorsCount;

		private AudioSource _soundSource;

		public AudioClip resetSound;
	}
}

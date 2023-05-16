using System.Collections;
using CodeBase._ImageEffects;
using CodeBase._Main.Player;
using UnityEngine;
using Utility;

namespace CodeBase._Main
{
	public class LevelBroundsTracker : MonoBehaviour
	{
		public string levelBoundsTag;

		private int _currentSensorsCount;

		private AudioSource _soundSource;

		public AudioClip resetSound;
		
		private void Start() => 
			_soundSource = GetComponent<AudioSource>();

		private void OnTriggerEnter(Collider collider)
		{
			if (collider.gameObject.CompareTag(levelBoundsTag)) 
				_currentSensorsCount++;
		}

		private void OnTriggerExit(Collider collider)
		{
			if (collider.gameObject.CompareTag(levelBoundsTag))
			{
				_currentSensorsCount--;
				if (_currentSensorsCount <= 0) 
					RegisterAbandonedLevel();
			}
		}

		private void RegisterAbandonedLevel()
		{
			if (_soundSource != null && resetSound != null) 
				_soundSource.PlayOneShot(resetSound);
			AirplaneTrails component = GetComponent<AirplaneTrails>();
			component.DeactivateTrails();
			component.ClearTrails();
			StartCoroutine(FadeOutCoroutine());
		}

		private IEnumerator FadeOutCoroutine()
		{
			BloomOptimized bloom = FindObjectOfType<BloomOptimized>();
			if (bloom == null)
			{
				ResetAirplane();
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
			ResetAirplane();
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
			ObjectResetter component = GetComponent<ObjectResetter>();
			if (component != null) 
				component.DelayedReset(0f);
		}

	}
}

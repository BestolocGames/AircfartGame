using System.Collections;
using CodeBase._ImageEffects;
using CodeBase._Main.Player;
using UnityEngine;
using UnityEngine.Serialization;
using Utility;

namespace CodeBase._Main
{
	public class LevelBroundsTracker : MonoBehaviour
	{
		[FormerlySerializedAs("levelBoundsTag")] public string _levelBoundsTag;

		private int _currentSensorsCount;

		private AudioSource _soundSource;

		[FormerlySerializedAs("resetSound")] public AudioClip _resetSound;
		
		private void Start() => 
			_soundSource = GetComponent<AudioSource>();

		private void OnTriggerEnter(Collider collider)
		{
			if (collider.gameObject.CompareTag(_levelBoundsTag)) 
				_currentSensorsCount++;
		}

		private void OnTriggerExit(Collider collider)
		{
			if (collider.gameObject.CompareTag(_levelBoundsTag))
			{
				_currentSensorsCount--;
				if (_currentSensorsCount <= 0) 
					RegisterAbandonedLevel();
			}
		}

		private void RegisterAbandonedLevel()
		{
			if (_soundSource != null && _resetSound != null) 
				_soundSource.PlayOneShot(_resetSound);
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
			float initIntensity = bloom._intensity;
			float initThreshold = bloom._threshold;
			while ((double)tween > 0.1)
			{
				bloom._intensity = Mathf.Lerp(bloom._intensity, targetIntensity, 1.5f * Time.deltaTime);
				bloom._threshold = Mathf.Lerp(bloom._threshold, 0f, 1.5f * Time.deltaTime);
				tween = Mathf.Lerp(tween, 0f, 1.5f * Time.deltaTime);
				yield return wait;
			}
			ResetAirplane();
			targetIntensity = initIntensity;
			tween = 1f;
			while ((double)tween > 0.1)
			{
				bloom._intensity = Mathf.Lerp(bloom._intensity, targetIntensity, 2f * Time.deltaTime);
				bloom._threshold = Mathf.Lerp(bloom._threshold, 0f, 2f * Time.deltaTime);
				tween = Mathf.Lerp(tween, 0f, 3f * Time.deltaTime);
				yield return wait;
			}
			bloom._intensity = initIntensity;
			bloom._threshold = initThreshold;
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

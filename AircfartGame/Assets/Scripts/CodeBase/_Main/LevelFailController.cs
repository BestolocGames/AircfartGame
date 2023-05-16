using System.Collections;
using CodeBase._ImageEffects;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace CodeBase._Main
{
	public class LevelFailController : MonoBehaviour
	{
		[Tooltip("If true, level fail menu will be shown. Otherwise, scene will be restarted.")]
		public bool showLevelFailMenu = true;

		[Tooltip("Game over screen.")]
		public CanvasGroup levelFailMenu;

		private float _defaultBloomIntensity;

		private float _defaultBloomThreshold;
		
		private void OnEnable()
		{
			FuelController.OnFuelEmptyEvent += HandleLevelFailed;
			RevivePermissionProvider.OnReviveGranted += HandleReviveGranted;
		}

		private void OnDisable()
		{
			FuelController.OnFuelEmptyEvent -= HandleLevelFailed;
			RevivePermissionProvider.OnReviveGranted -= HandleReviveGranted;
		}

		public virtual void HandleLevelFailed()
		{
			if (showLevelFailMenu)
			{
				StartCoroutine(FadeOutCoroutine());
			}
			else
			{
				Time.timeScale = 1f;
				SceneManager.LoadScene(SceneManager.GetActiveScene().name);
			}
		}

		private IEnumerator FadeOutCoroutine()
		{
			yield return new WaitForSeconds(1f);
			BloomOptimized bloom = FindObjectOfType<BloomOptimized>();
			if (bloom == null)
			{
				Time.timeScale = 0f;
				if (levelFailMenu != null)
				{
					levelFailMenu.gameObject.SetActive(true);
				}
				yield break;
			}
			float targetIntensity = 2.5f;
			WaitForEndOfFrame wait = new WaitForEndOfFrame();
			float tween = 1f;
			_defaultBloomIntensity = bloom.intensity;
			_defaultBloomThreshold = bloom.threshold;
			if (levelFailMenu != null)
			{
				levelFailMenu.alpha = 0f;
				levelFailMenu.gameObject.SetActive(true);
			}
			float prevTime = Time.realtimeSinceStartup;
			float deltaTime = 0f;
			while ((double)tween > 0.1)
			{
				deltaTime = Time.realtimeSinceStartup - prevTime;
				prevTime = Time.realtimeSinceStartup;
				bloom.intensity = Mathf.Lerp(bloom.intensity, targetIntensity, 1.5f * deltaTime);
				bloom.threshold = Mathf.Lerp(bloom.threshold, 0f, 1.5f * deltaTime);
				tween = Mathf.Lerp(tween, 0f, 1.5f * deltaTime);
				Time.timeScale = tween;
				if (levelFailMenu != null)
				{
					levelFailMenu.alpha = 1f - tween;
				}
				yield return wait;
			}
			if (levelFailMenu != null)
			{
				levelFailMenu.alpha = 1f;
			}
			Time.timeScale = 0f;
			yield break;
		}

		private void HandleReviveGranted()
		{
			Time.timeScale = 1f;
			levelFailMenu.gameObject.SetActive(false);
			StartCoroutine(TweenIn());
		}

		private IEnumerator TweenIn()
		{
			BloomOptimized bloom = FindObjectOfType<BloomOptimized>();
			if (bloom == null)
			{
				yield break;
			}
			WaitForEndOfFrame wait = new WaitForEndOfFrame();
			float targetIntensity = _defaultBloomIntensity;
			float tween = 1f;
			while ((double)tween > 0.1)
			{
				bloom.intensity = Mathf.Lerp(bloom.intensity, targetIntensity, 2f * Time.deltaTime);
				bloom.threshold = Mathf.Lerp(bloom.threshold, 0f, 2f * Time.deltaTime);
				tween = Mathf.Lerp(tween, 0f, 3f * Time.deltaTime);
				yield return wait;
			}
			bloom.intensity = _defaultBloomIntensity;
			bloom.threshold = _defaultBloomThreshold;
			yield break;
		}
	}
}

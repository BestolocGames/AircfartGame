using System.Collections;
using CodeBase._ImageEffects;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

namespace CodeBase._Main
{
	public class LevelFailController : MonoBehaviour
	{
		[FormerlySerializedAs("showLevelFailMenu")] [Tooltip("If true, level fail menu will be shown. Otherwise, scene will be restarted.")]
		public bool _showLevelFailMenu = true;

		[FormerlySerializedAs("levelFailMenu")] [Tooltip("Game over screen.")]
		public CanvasGroup _levelFailMenu;

		private float _defaultBloomIntensity;

		private float _defaultBloomThreshold;
		
		private void OnEnable() => 
			RevivePermissionProvider.OnReviveGranted += HandleReviveGranted;

		private void OnDisable() => 
			RevivePermissionProvider.OnReviveGranted -= HandleReviveGranted;

		public virtual void HandleLevelFailed()
		{
			if (_showLevelFailMenu)
				StartCoroutine(FadeOutCoroutine());
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
				if (_levelFailMenu != null) 
					_levelFailMenu.gameObject.SetActive(true);
				yield break;
			}
			float targetIntensity = 2.5f;
			WaitForEndOfFrame wait = new WaitForEndOfFrame();
			float tween = 1f;
			_defaultBloomIntensity = bloom._intensity;
			_defaultBloomThreshold = bloom._threshold;
			if (_levelFailMenu != null)
			{
				_levelFailMenu.alpha = 0f;
				_levelFailMenu.gameObject.SetActive(true);
			}
			float prevTime = Time.realtimeSinceStartup;
			float deltaTime = 0f;
			while ((double)tween > 0.1)
			{
				deltaTime = Time.realtimeSinceStartup - prevTime;
				prevTime = Time.realtimeSinceStartup;
				bloom._intensity = Mathf.Lerp(bloom._intensity, targetIntensity, 1.5f * deltaTime);
				bloom._threshold = Mathf.Lerp(bloom._threshold, 0f, 1.5f * deltaTime);
				tween = Mathf.Lerp(tween, 0f, 1.5f * deltaTime);
				Time.timeScale = tween;
				if (_levelFailMenu != null)
				{
					_levelFailMenu.alpha = 1f - tween;
				}
				yield return wait;
			}
			if (_levelFailMenu != null) 
				_levelFailMenu.alpha = 1f;
			Time.timeScale = 0f;
			yield break;
		}

		private void HandleReviveGranted()
		{
			Time.timeScale = 1f;
			_levelFailMenu.gameObject.SetActive(false);
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
				bloom._intensity = Mathf.Lerp(bloom._intensity, targetIntensity, 2f * Time.deltaTime);
				bloom._threshold = Mathf.Lerp(bloom._threshold, 0f, 2f * Time.deltaTime);
				tween = Mathf.Lerp(tween, 0f, 3f * Time.deltaTime);
				yield return wait;
			}
			bloom._intensity = _defaultBloomIntensity;
			bloom._threshold = _defaultBloomThreshold;
			yield break;
		}
	}
}

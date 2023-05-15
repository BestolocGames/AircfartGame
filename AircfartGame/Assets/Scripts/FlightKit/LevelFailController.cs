// dnSpy decompiler from Assembly-CSharp.dll class: FlightKit.LevelFailController
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityStandardAssets.ImageEffects;

namespace FlightKit
{
	public class LevelFailController : MonoBehaviour
	{
		private void OnEnable()
		{
			FuelController.OnFuelEmptyEvent += this.HandleLevelFailed;
			RevivePermissionProvider.OnReviveGranted += this.HandleReviveGranted;
		}

		private void OnDisable()
		{
			FuelController.OnFuelEmptyEvent -= this.HandleLevelFailed;
			RevivePermissionProvider.OnReviveGranted -= this.HandleReviveGranted;
		}

		public virtual void HandleLevelFailed()
		{
			if (this.showLevelFailMenu)
			{
				base.StartCoroutine(this.FadeOutCoroutine());
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
			BloomOptimized bloom = UnityEngine.Object.FindObjectOfType<BloomOptimized>();
			if (bloom == null)
			{
				Time.timeScale = 0f;
				if (this.levelFailMenu != null)
				{
					this.levelFailMenu.gameObject.SetActive(true);
				}
				yield break;
			}
			float targetIntensity = 2.5f;
			WaitForEndOfFrame wait = new WaitForEndOfFrame();
			float tween = 1f;
			this._defaultBloomIntensity = bloom.intensity;
			this._defaultBloomThreshold = bloom.threshold;
			if (this.levelFailMenu != null)
			{
				this.levelFailMenu.alpha = 0f;
				this.levelFailMenu.gameObject.SetActive(true);
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
				if (this.levelFailMenu != null)
				{
					this.levelFailMenu.alpha = 1f - tween;
				}
				yield return wait;
			}
			if (this.levelFailMenu != null)
			{
				this.levelFailMenu.alpha = 1f;
			}
			Time.timeScale = 0f;
			yield break;
		}

		private void HandleReviveGranted()
		{
			Time.timeScale = 1f;
			this.levelFailMenu.gameObject.SetActive(false);
			base.StartCoroutine(this.TweenIn());
		}

		private IEnumerator TweenIn()
		{
			BloomOptimized bloom = UnityEngine.Object.FindObjectOfType<BloomOptimized>();
			if (bloom == null)
			{
				yield break;
			}
			WaitForEndOfFrame wait = new WaitForEndOfFrame();
			float targetIntensity = this._defaultBloomIntensity;
			float tween = 1f;
			while ((double)tween > 0.1)
			{
				bloom.intensity = Mathf.Lerp(bloom.intensity, targetIntensity, 2f * Time.deltaTime);
				bloom.threshold = Mathf.Lerp(bloom.threshold, 0f, 2f * Time.deltaTime);
				tween = Mathf.Lerp(tween, 0f, 3f * Time.deltaTime);
				yield return wait;
			}
			bloom.intensity = this._defaultBloomIntensity;
			bloom.threshold = this._defaultBloomThreshold;
			yield break;
		}

		[Tooltip("If true, level fail menu will be shown. Otherwise, scene will be restarted.")]
		public bool showLevelFailMenu = true;

		[Tooltip("Game over screen.")]
		public CanvasGroup levelFailMenu;

		private float _defaultBloomIntensity;

		private float _defaultBloomThreshold;
	}
}

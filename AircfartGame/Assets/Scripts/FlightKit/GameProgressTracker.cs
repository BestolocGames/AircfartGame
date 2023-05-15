// dnSpy decompiler from Assembly-CSharp.dll class: FlightKit.GameProgressTracker
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets.ImageEffects;

namespace FlightKit
{
	public class GameProgressTracker : MonoBehaviour
	{
		private void Start()
		{
			if (this.pickupsCurrentText == null)
			{
				GameObject gameObject = GameObject.Find("PickupsCurrent");
				if (gameObject != null)
				{
					this.pickupsCurrentText = gameObject.GetComponent<Text>();
				}
			}
			if (this.pickupsTotalText == null)
			{
				GameObject gameObject2 = GameObject.Find("PickupsTotal");
				if (gameObject2 != null)
				{
					this.pickupsTotalText = gameObject2.GetComponent<Text>();
				}
			}
			if (this.pickupIconImage == null)
			{
				GameObject gameObject3 = GameObject.Find("PickupIcon");
				if (gameObject3 != null)
				{
					this.pickupIconImage = gameObject3.GetComponent<Image>();
				}
			}
			PickupSphere[] array = UnityEngine.Object.FindObjectsOfType<PickupSphere>();
			this._numPickupsTotal = array.Length;
			if (this.pickupsTotalText != null)
			{
				this.pickupsTotalText.text = this._numPickupsTotal.ToString();
			}
			PickupSphere.OnCollectEvent += this.RegisterPickup;
		}

		private void OnDestroy()
		{
			PickupSphere.OnCollectEvent -= this.RegisterPickup;
		}

		private void RegisterPickup()
		{
			if (this._numPickupsCollected == 0)
			{
				this.ShowPickupCounter();
			}
			this._numPickupsCollected++;
			if (this.pickupsCurrentText != null)
			{
				this.pickupsCurrentText.text = this._numPickupsCollected.ToString();
			}
			if (this._numPickupsCollected >= this._numPickupsTotal)
			{
				this.RegisterLevelComplete();
			}
		}

		public virtual void RegisterLevelComplete()
		{
			base.StartCoroutine(this.FadeOutCoroutine());
		}

		private IEnumerator FadeOutCoroutine()
		{
			BloomOptimized bloom = UnityEngine.Object.FindObjectOfType<BloomOptimized>();
			float targetIntensity = 2.5f;
			MusicController musicController = UnityEngine.Object.FindObjectOfType<MusicController>();
			bool tweenMusic = musicController != null && musicController.gameplay != null;
			WaitForEndOfFrame wait = new WaitForEndOfFrame();
			float tween = 1f;
			float tweenSpeed = 0.5f;
			float startTime = Time.realtimeSinceStartup;
			float lastTime = startTime;
			float deltaTime = 0f;
			float fixedDeltaTime = Time.fixedDeltaTime;
			while ((double)tween > 0.1)
			{
				deltaTime = Time.realtimeSinceStartup - lastTime;
				lastTime = Time.realtimeSinceStartup;
				if (bloom != null)
				{
					bloom.intensity = Mathf.Lerp(bloom.intensity, targetIntensity, tweenSpeed * deltaTime);
					bloom.threshold = Mathf.Lerp(bloom.threshold, 0f, tweenSpeed * deltaTime);
				}
				if (tweenMusic)
				{
					musicController.gameplay.volume = Mathf.Lerp(musicController.gameplay.volume, 0f, tweenSpeed * deltaTime);
				}
				Time.timeScale = Mathf.Lerp(Time.timeScale, 0f, tweenSpeed * deltaTime);
				Time.fixedDeltaTime = fixedDeltaTime * Time.timeScale;
				tween = Mathf.Lerp(tween, 0f, tweenSpeed * deltaTime);
				yield return wait;
			}
			LevelCompleteController lcc = UnityEngine.Object.FindObjectOfType<LevelCompleteController>();
			if (lcc != null)
			{
				lcc.HandleLevelComplete();
			}
			Time.timeScale = 1f;
			Time.fixedDeltaTime = fixedDeltaTime;
			yield break;
		}

		private void ShowPickupCounter()
		{
			if (this.pickupIconImage != null)
			{
				this.pickupIconImage.enabled = true;
				this.pickupIconImage.canvasRenderer.SetAlpha(0f);
				this.pickupIconImage.CrossFadeAlpha(1f, 5f, false);
			}
			if (this.pickupsCurrentText != null)
			{
				this.pickupsCurrentText.enabled = true;
				this.pickupsCurrentText.canvasRenderer.SetAlpha(0f);
				this.pickupsCurrentText.CrossFadeAlpha(1f, 5f, false);
			}
			if (this.pickupsTotalText != null)
			{
				this.pickupsTotalText.enabled = true;
				this.pickupsTotalText.canvasRenderer.SetAlpha(0f);
				this.pickupsTotalText.CrossFadeAlpha(1f, 5f, false);
			}
		}

		public Text pickupsCurrentText;

		public Text pickupsTotalText;

		public Image pickupIconImage;

		private int _numPickupsCollected;

		private int _numPickupsTotal;
	}
}

using System.Collections;
using CodeBase._ImageEffects;
using UnityEngine;
using UnityEngine.UI;

namespace CodeBase._Main
{
	public class GameProgressTracker : MonoBehaviour
	{
		public Text pickupsCurrentText;

		public Text pickupsTotalText;

		public Image pickupIconImage;

		private int _numPickupsCollected;

		private int _numPickupsTotal;
		
		private void Start()
		{
			if (pickupsCurrentText == null)
			{
				GameObject gameObject = GameObject.Find("PickupsCurrent");
				if (gameObject != null)
				{
					pickupsCurrentText = gameObject.GetComponent<Text>();
				}
			}
			if (pickupsTotalText == null)
			{
				GameObject gameObject2 = GameObject.Find("PickupsTotal");
				if (gameObject2 != null)
				{
					pickupsTotalText = gameObject2.GetComponent<Text>();
				}
			}
			if (pickupIconImage == null)
			{
				GameObject gameObject3 = GameObject.Find("PickupIcon");
				if (gameObject3 != null)
				{
					pickupIconImage = gameObject3.GetComponent<Image>();
				}
			}
			PickupSphere[] array = FindObjectsOfType<PickupSphere>();
			_numPickupsTotal = array.Length;
			if (pickupsTotalText != null)
			{
				pickupsTotalText.text = _numPickupsTotal.ToString();
			}
			PickupSphere.OnCollectEvent += RegisterPickup;
		}

		private void OnDestroy()
		{
			PickupSphere.OnCollectEvent -= RegisterPickup;
		}

		private void RegisterPickup()
		{
			if (_numPickupsCollected == 0) 
				ShowPickupCounter();
			_numPickupsCollected++;
			if (pickupsCurrentText != null) 
				pickupsCurrentText.text = _numPickupsCollected.ToString();
			if (_numPickupsCollected >= _numPickupsTotal) 
				RegisterLevelComplete();
		}

		public virtual void RegisterLevelComplete()
		{
			StartCoroutine(FadeOutCoroutine());
		}

		private IEnumerator FadeOutCoroutine()
		{
			BloomOptimized bloom = FindObjectOfType<BloomOptimized>();
			float targetIntensity = 2.5f;
			MusicController musicController = FindObjectOfType<MusicController>();
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
			LevelCompleteController lcc = FindObjectOfType<LevelCompleteController>();
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
			if (pickupIconImage != null)
			{
				pickupIconImage.enabled = true;
				pickupIconImage.canvasRenderer.SetAlpha(0f);
				pickupIconImage.CrossFadeAlpha(1f, 5f, false);
			}
			if (pickupsCurrentText != null)
			{
				pickupsCurrentText.enabled = true;
				pickupsCurrentText.canvasRenderer.SetAlpha(0f);
				pickupsCurrentText.CrossFadeAlpha(1f, 5f, false);
			}
			if (pickupsTotalText != null)
			{
				pickupsTotalText.enabled = true;
				pickupsTotalText.canvasRenderer.SetAlpha(0f);
				pickupsTotalText.CrossFadeAlpha(1f, 5f, false);
			}
		}
	}
}

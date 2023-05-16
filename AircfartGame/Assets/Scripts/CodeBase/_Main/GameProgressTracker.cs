using System.Collections;
using CodeBase._ImageEffects;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace CodeBase._Main
{
	public class GameProgressTracker : MonoBehaviour
	{
		[FormerlySerializedAs("pickupsCurrentText")] public Text _pickupsCurrentText;

		[FormerlySerializedAs("pickupsTotalText")] public Text _pickupsTotalText;

		[FormerlySerializedAs("pickupIconImage")] public Image _pickupIconImage;

		private int _numPickupsCollected;

		private int _numPickupsTotal;
		
		private void Start()
		{
			if (_pickupsCurrentText == null)
			{
				GameObject gameObject = GameObject.Find("PickupsCurrent");
				if (gameObject != null)
				{
					_pickupsCurrentText = gameObject.GetComponent<Text>();
				}
			}
			if (_pickupsTotalText == null)
			{
				GameObject gameObject2 = GameObject.Find("PickupsTotal");
				if (gameObject2 != null)
				{
					_pickupsTotalText = gameObject2.GetComponent<Text>();
				}
			}
			if (_pickupIconImage == null)
			{
				GameObject gameObject3 = GameObject.Find("PickupIcon");
				if (gameObject3 != null)
				{
					_pickupIconImage = gameObject3.GetComponent<Image>();
				}
			}
			PickupSphere[] array = FindObjectsOfType<PickupSphere>();
			_numPickupsTotal = array.Length;
			if (_pickupsTotalText != null)
			{
				_pickupsTotalText.text = _numPickupsTotal.ToString();
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
			if (_pickupsCurrentText != null) 
				_pickupsCurrentText.text = _numPickupsCollected.ToString();
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
			bool tweenMusic = musicController != null && musicController._gameplay != null;
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
					bloom._intensity = Mathf.Lerp(bloom._intensity, targetIntensity, tweenSpeed * deltaTime);
					bloom._threshold = Mathf.Lerp(bloom._threshold, 0f, tweenSpeed * deltaTime);
				}
				if (tweenMusic)
				{
					musicController._gameplay.volume = Mathf.Lerp(musicController._gameplay.volume, 0f, tweenSpeed * deltaTime);
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
			if (_pickupIconImage != null)
			{
				_pickupIconImage.enabled = true;
				_pickupIconImage.canvasRenderer.SetAlpha(0f);
				_pickupIconImage.CrossFadeAlpha(1f, 5f, false);
			}
			if (_pickupsCurrentText != null)
			{
				_pickupsCurrentText.enabled = true;
				_pickupsCurrentText.canvasRenderer.SetAlpha(0f);
				_pickupsCurrentText.CrossFadeAlpha(1f, 5f, false);
			}
			if (_pickupsTotalText != null)
			{
				_pickupsTotalText.enabled = true;
				_pickupsTotalText.canvasRenderer.SetAlpha(0f);
				_pickupsTotalText.CrossFadeAlpha(1f, 5f, false);
			}
		}
	}
}

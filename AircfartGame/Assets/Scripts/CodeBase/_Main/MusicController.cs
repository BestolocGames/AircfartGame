using System.Collections;
using UI;
using UnityEngine;

namespace CodeBase._Main
{
	public class MusicController : MonoBehaviour
	{
		private void Awake()
		{
			if (menu != null && enabled)
			{
				_initMenuVolume = menu.volume;
				if (playOnStart)
				{
					menu.Play();
				}
			}
		}

		private void OnEnable()
		{
			if (changeMusicOnTakeOff)
			{
				UIEventsPublisher.OnPlayEvent += OnPlayClicked;
				TakeOffPublisher.OnTakeOffEvent += StartGameplay;
			}
			RevivePermissionProvider.OnReviveRequested += HandleReviveRequest;
			RevivePermissionProvider.OnReviveGranted += HandleRevive;
		}

		private void OnDisable()
		{
			UIEventsPublisher.OnPlayEvent -= OnPlayClicked;
			TakeOffPublisher.OnTakeOffEvent -= StartGameplay;
			RevivePermissionProvider.OnReviveRequested -= HandleReviveRequest;
			RevivePermissionProvider.OnReviveGranted -= HandleRevive;
		}

		public virtual void Pause()
		{
			if (menu && menu.isPlaying)
			{
				menu.Pause();
			}
			if (gameplay && gameplay.isPlaying)
			{
				gameplay.Pause();
			}
		}

		public virtual void UnPause()
		{
			if (menu && !menu.isPlaying)
			{
				menu.UnPause();
			}
			if (gameplay && !gameplay.isPlaying)
			{
				gameplay.UnPause();
			}
		}

		private void OnPlayClicked()
		{
			StartCoroutine(FadeOutMenu());
		}

		private IEnumerator FadeOutMenu()
		{
			float tweenStartTime = Time.realtimeSinceStartup;
			WaitForEndOfFrame wait = new WaitForEndOfFrame();
			float tweenOutProgress = 1f;
			while (tweenOutProgress > 0.01f)
			{
				tweenOutProgress = Mathf.SmoothStep(1f, 0f, (Time.realtimeSinceStartup - tweenStartTime) * 0.1f * menuMusicFadeOutSpeed);
				menu.volume = _initMenuVolume * tweenOutProgress;
				yield return wait;
			}
			menu.Stop();
			menu.volume = _initMenuVolume;
			yield break;
		}

		private IEnumerator FadeOutGameplay()
		{
			float initVolume = gameplay.volume;
			float tweenStartTime = Time.realtimeSinceStartup;
			WaitForEndOfFrame wait = new WaitForEndOfFrame();
			float tweenOutProgress = 1f;
			while (tweenOutProgress > 0.01f)
			{
				tweenOutProgress = Mathf.SmoothStep(1f, 0f, (Time.realtimeSinceStartup - tweenStartTime) * 0.5f);
				gameplay.volume = initVolume * tweenOutProgress;
				yield return wait;
			}
			gameplay.Pause();
			gameplay.volume = initVolume;
			yield break;
		}

		private void StartGameplay()
		{
			Invoke("StartGameplayCore", 0.5f);
		}

		private void StartGameplayCore()
		{
			if (gameplay && !gameplay.isPlaying)
			{
				gameplay.Play();
			}
		}

		private void HandleReviveRequest()
		{
			if (gameplay != null)
			{
				StartCoroutine(FadeOutGameplay());
			}
		}

		private void HandleRevive()
		{
			if (gameplay != null)
			{
				StopAllCoroutines();
				StartGameplayCore();
			}
		}

		public AudioSource menu;

		public AudioSource gameplay;

		public bool playOnStart = true;

		public bool changeMusicOnTakeOff = true;

		public float menuMusicFadeOutSpeed = 1f;

		private float _initMenuVolume;
	}
}

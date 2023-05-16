using System.Collections;
using UI;
using UnityEngine;
using UnityEngine.Serialization;

namespace CodeBase._Main
{
	public class MusicController : MonoBehaviour
	{
		private void Awake()
		{
			if (_menu != null && enabled)
			{
				_initMenuVolume = _menu.volume;
				if (_playOnStart)
				{
					_menu.Play();
				}
			}
		}

		private void OnEnable()
		{
			if (_changeMusicOnTakeOff)
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
			if (_menu && _menu.isPlaying)
			{
				_menu.Pause();
			}
			if (_gameplay && _gameplay.isPlaying)
			{
				_gameplay.Pause();
			}
		}

		public virtual void UnPause()
		{
			if (_menu && !_menu.isPlaying)
			{
				_menu.UnPause();
			}
			if (_gameplay && !_gameplay.isPlaying)
			{
				_gameplay.UnPause();
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
				tweenOutProgress = Mathf.SmoothStep(1f, 0f, (Time.realtimeSinceStartup - tweenStartTime) * 0.1f * _menuMusicFadeOutSpeed);
				_menu.volume = _initMenuVolume * tweenOutProgress;
				yield return wait;
			}
			_menu.Stop();
			_menu.volume = _initMenuVolume;
			yield break;
		}

		private IEnumerator FadeOutGameplay()
		{
			float initVolume = _gameplay.volume;
			float tweenStartTime = Time.realtimeSinceStartup;
			WaitForEndOfFrame wait = new WaitForEndOfFrame();
			float tweenOutProgress = 1f;
			while (tweenOutProgress > 0.01f)
			{
				tweenOutProgress = Mathf.SmoothStep(1f, 0f, (Time.realtimeSinceStartup - tweenStartTime) * 0.5f);
				_gameplay.volume = initVolume * tweenOutProgress;
				yield return wait;
			}
			_gameplay.Pause();
			_gameplay.volume = initVolume;
			yield break;
		}

		private void StartGameplay()
		{
			Invoke("StartGameplayCore", 0.5f);
		}

		private void StartGameplayCore()
		{
			if (_gameplay && !_gameplay.isPlaying)
			{
				_gameplay.Play();
			}
		}

		private void HandleReviveRequest()
		{
			if (_gameplay != null)
			{
				StartCoroutine(FadeOutGameplay());
			}
		}

		private void HandleRevive()
		{
			if (_gameplay != null)
			{
				StopAllCoroutines();
				StartGameplayCore();
			}
		}

		[FormerlySerializedAs("menu")] public AudioSource _menu;

		[FormerlySerializedAs("gameplay")] public AudioSource _gameplay;

		[FormerlySerializedAs("playOnStart")] public bool _playOnStart = true;

		[FormerlySerializedAs("changeMusicOnTakeOff")] public bool _changeMusicOnTakeOff = true;

		[FormerlySerializedAs("menuMusicFadeOutSpeed")] public float _menuMusicFadeOutSpeed = 1f;

		private float _initMenuVolume;
	}
}

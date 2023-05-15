// dnSpy decompiler from Assembly-CSharp.dll class: FlightKit.MusicController
using System;
using System.Collections;
using UnityEngine;

namespace FlightKit
{
	public class MusicController : MonoBehaviour
	{
		private void Awake()
		{
			if (this.menu != null && base.enabled)
			{
				this._initMenuVolume = this.menu.volume;
				if (this.playOnStart)
				{
					this.menu.Play();
				}
			}
		}

		private void OnEnable()
		{
			if (this.changeMusicOnTakeOff)
			{
				UIEventsPublisher.OnPlayEvent += this.OnPlayClicked;
				TakeOffPublisher.OnTakeOffEvent += this.StartGameplay;
			}
			RevivePermissionProvider.OnReviveRequested += this.HandleReviveRequest;
			RevivePermissionProvider.OnReviveGranted += this.HandleRevive;
		}

		private void OnDisable()
		{
			UIEventsPublisher.OnPlayEvent -= this.OnPlayClicked;
			TakeOffPublisher.OnTakeOffEvent -= this.StartGameplay;
			RevivePermissionProvider.OnReviveRequested -= this.HandleReviveRequest;
			RevivePermissionProvider.OnReviveGranted -= this.HandleRevive;
		}

		public virtual void Pause()
		{
			if (this.menu && this.menu.isPlaying)
			{
				this.menu.Pause();
			}
			if (this.gameplay && this.gameplay.isPlaying)
			{
				this.gameplay.Pause();
			}
		}

		public virtual void UnPause()
		{
			if (this.menu && !this.menu.isPlaying)
			{
				this.menu.UnPause();
			}
			if (this.gameplay && !this.gameplay.isPlaying)
			{
				this.gameplay.UnPause();
			}
		}

		private void OnPlayClicked()
		{
			base.StartCoroutine(this.FadeOutMenu());
		}

		private IEnumerator FadeOutMenu()
		{
			float tweenStartTime = Time.realtimeSinceStartup;
			WaitForEndOfFrame wait = new WaitForEndOfFrame();
			float tweenOutProgress = 1f;
			while (tweenOutProgress > 0.01f)
			{
				tweenOutProgress = Mathf.SmoothStep(1f, 0f, (Time.realtimeSinceStartup - tweenStartTime) * 0.1f * this.menuMusicFadeOutSpeed);
				this.menu.volume = this._initMenuVolume * tweenOutProgress;
				yield return wait;
			}
			this.menu.Stop();
			this.menu.volume = this._initMenuVolume;
			yield break;
		}

		private IEnumerator FadeOutGameplay()
		{
			float initVolume = this.gameplay.volume;
			float tweenStartTime = Time.realtimeSinceStartup;
			WaitForEndOfFrame wait = new WaitForEndOfFrame();
			float tweenOutProgress = 1f;
			while (tweenOutProgress > 0.01f)
			{
				tweenOutProgress = Mathf.SmoothStep(1f, 0f, (Time.realtimeSinceStartup - tweenStartTime) * 0.5f);
				this.gameplay.volume = initVolume * tweenOutProgress;
				yield return wait;
			}
			this.gameplay.Pause();
			this.gameplay.volume = initVolume;
			yield break;
		}

		private void StartGameplay()
		{
			base.Invoke("StartGameplayCore", 0.5f);
		}

		private void StartGameplayCore()
		{
			if (this.gameplay && !this.gameplay.isPlaying)
			{
				this.gameplay.Play();
			}
		}

		private void HandleReviveRequest()
		{
			if (this.gameplay != null)
			{
				base.StartCoroutine(this.FadeOutGameplay());
			}
		}

		private void HandleRevive()
		{
			if (this.gameplay != null)
			{
				base.StopAllCoroutines();
				this.StartGameplayCore();
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

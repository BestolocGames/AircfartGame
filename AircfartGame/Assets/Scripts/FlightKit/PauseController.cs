// dnSpy decompiler from Assembly-CSharp.dll class: FlightKit.PauseController
using System;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

namespace FlightKit
{
	public class PauseController : MonoBehaviour
	{
		public static event PauseController.OnPauseAction OnPauseEvent;

		public static event PauseController.OnUnPauseAction OnUnPauseEvent;

		private void Start()
		{
			this._musicController = UnityEngine.Object.FindObjectOfType<MusicController>();
		}

		private void Update()
		{
			if (CrossPlatformInputManager.GetButtonDown("Cancel"))
			{
				if (!this._paused)
				{
					if (!this.pausePanelObject.activeSelf)
					{
						this.Pause();
					}
				}
				else if (this.pausePanelObject.activeSelf)
				{
					this.Unpause();
				}
			}
		}

		public virtual void Pause()
		{
			this._paused = true;
			Time.timeScale = 0f;
			this.pausePanelObject.SetActive(true);
			if (this._musicController)
			{
				this._musicController.Pause();
			}
			this.PublishPause();
		}

		public virtual void Unpause()
		{
			this._paused = false;
			Time.timeScale = 1f;
			this.pausePanelObject.SetActive(false);
			if (this._musicController)
			{
				this._musicController.UnPause();
			}
			this.PublishUnPause();
		}

		public virtual void PublishPause()
		{
			if (PauseController.OnPauseEvent != null)
			{
				PauseController.OnPauseEvent();
			}
		}

		public virtual void PublishUnPause()
		{
			if (PauseController.OnUnPauseEvent != null)
			{
				PauseController.OnUnPauseEvent();
			}
		}

		public GameObject pausePanelObject;

		private bool _paused;

		private MusicController _musicController;

		public delegate void OnPauseAction();

		public delegate void OnUnPauseAction();
	}
}

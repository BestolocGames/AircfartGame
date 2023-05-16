using CodeBase._CrossPlatformInput;
using CodeBase._Main;
using UnityEngine;

namespace UI
{
	public class PauseController : MonoBehaviour
	{
		public static event OnPauseAction OnPauseEvent;

		public static event OnUnPauseAction OnUnPauseEvent;

		private void Start()
		{
			_musicController = FindObjectOfType<MusicController>();
		}

		private void Update()
		{
			if (CrossPlatformInputManager.GetButtonDown("Cancel"))
			{
				if (!_paused)
				{
					if (!pausePanelObject.activeSelf)
					{
						Pause();
					}
				}
				else if (pausePanelObject.activeSelf)
				{
					Unpause();
				}
			}
		}

		public virtual void Pause()
		{
			_paused = true;
			Time.timeScale = 0f;
			pausePanelObject.SetActive(true);
			if (_musicController)
			{
				_musicController.Pause();
			}
			PublishPause();
		}

		public virtual void Unpause()
		{
			_paused = false;
			Time.timeScale = 1f;
			pausePanelObject.SetActive(false);
			if (_musicController)
			{
				_musicController.UnPause();
			}
			PublishUnPause();
		}

		public virtual void PublishPause()
		{
			if (OnPauseEvent != null)
			{
				OnPauseEvent();
			}
		}

		public virtual void PublishUnPause()
		{
			if (OnUnPauseEvent != null)
			{
				OnUnPauseEvent();
			}
		}

		public GameObject pausePanelObject;

		private bool _paused;

		private MusicController _musicController;

		public delegate void OnPauseAction();

		public delegate void OnUnPauseAction();
	}
}

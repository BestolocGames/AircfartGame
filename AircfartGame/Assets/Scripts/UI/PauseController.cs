using CodeBase._CrossPlatformInput;
using CodeBase._Main;
using UnityEngine;
using UnityEngine.Serialization;

namespace UI
{
	public class PauseController : MonoBehaviour
	{
		[FormerlySerializedAs("pausePanelObject")] public GameObject _pausePanelObject;

		private bool _paused;

		private MusicController _musicController;

		public delegate void PauseAction();

		public delegate void UnPauseAction();
		
		public static event PauseAction OnPauseEvent;

		public static event UnPauseAction OnUnPauseEvent;

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
					if (!_pausePanelObject.activeSelf) 
						Pause();
				}
				else if (_pausePanelObject.activeSelf) 
					Unpause();
			}
		}

		public virtual void Pause()
		{
			_paused = true;
			Time.timeScale = 0f;
			_pausePanelObject.SetActive(true);
			if (_musicController) 
				_musicController.Pause();
			
			PublishPause();
		}

		public virtual void Unpause()
		{
			_paused = false;
			Time.timeScale = 1f;
			_pausePanelObject.SetActive(false);
			if (_musicController) 
				_musicController.UnPause();
			
			PublishUnPause();
		}

		protected virtual void PublishPause()
		{
			if (OnPauseEvent != null) 
				OnPauseEvent();
		}

		protected virtual void PublishUnPause()
		{
			if (OnUnPauseEvent != null) 
				OnUnPauseEvent();
		}
	}
}

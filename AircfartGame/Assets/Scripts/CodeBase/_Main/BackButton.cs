using UI;
using UnityEngine;
using UnityEngine.Serialization;

namespace CodeBase._Main
{
	[RequireComponent(typeof(CrossFadeCanvasGroups))]
	public class BackButton : MonoBehaviour
	{
		private void Start()
		{
			_crossFade = GetComponent<CrossFadeCanvasGroups>();
			_startLevelController = FindObjectOfType<StartLevelController>();
			_crossFade._toGroup = _mainMenu;
		}

		public virtual void Activate()
		{
			if (_startLevelController != null)
			{
				_crossFade._toGroup = ((!_startLevelController.LevelStarted) ? _mainMenu : _pauseMenu);
			}
			_crossFade.Activate();
		}

		[FormerlySerializedAs("mainMenu")] public CanvasGroup _mainMenu;

		[FormerlySerializedAs("pauseMenu")] public CanvasGroup _pauseMenu;

		private CrossFadeCanvasGroups _crossFade;

		private StartLevelController _startLevelController;
	}
}

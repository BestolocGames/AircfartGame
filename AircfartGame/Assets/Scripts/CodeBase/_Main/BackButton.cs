using UI;
using UnityEngine;

namespace CodeBase._Main
{
	[RequireComponent(typeof(CrossFadeCanvasGroups))]
	public class BackButton : MonoBehaviour
	{
		private void Start()
		{
			crossFade = GetComponent<CrossFadeCanvasGroups>();
			startLevelController = FindObjectOfType<StartLevelController>();
			crossFade.toGroup = mainMenu;
		}

		public virtual void Activate()
		{
			if (startLevelController != null)
			{
				crossFade.toGroup = ((!startLevelController.levelStarted) ? mainMenu : pauseMenu);
			}
			crossFade.Activate();
		}

		public CanvasGroup mainMenu;

		public CanvasGroup pauseMenu;

		private CrossFadeCanvasGroups crossFade;

		private StartLevelController startLevelController;
	}
}

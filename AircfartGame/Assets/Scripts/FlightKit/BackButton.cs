// dnSpy decompiler from Assembly-CSharp.dll class: FlightKit.BackButton
using System;
using UnityEngine;

namespace FlightKit
{
	[RequireComponent(typeof(CrossFadeCanvasGroups))]
	public class BackButton : MonoBehaviour
	{
		private void Start()
		{
			this.crossFade = base.GetComponent<CrossFadeCanvasGroups>();
			this.startLevelController = UnityEngine.Object.FindObjectOfType<StartLevelController>();
			this.crossFade.toGroup = this.mainMenu;
		}

		public virtual void Activate()
		{
			if (this.startLevelController != null)
			{
				this.crossFade.toGroup = ((!this.startLevelController.levelStarted) ? this.mainMenu : this.pauseMenu);
			}
			this.crossFade.Activate();
		}

		public CanvasGroup mainMenu;

		public CanvasGroup pauseMenu;

		private CrossFadeCanvasGroups crossFade;

		private StartLevelController startLevelController;
	}
}

// dnSpy decompiler from Assembly-CSharp.dll class: FlightKit.PauseButton
using System;
using UnityEngine;

namespace FlightKit
{
	public class PauseButton : MonoBehaviour
	{
		public virtual void OnClick()
		{
			if (this.active)
			{
				PauseController pauseController = UnityEngine.Object.FindObjectOfType<PauseController>();
				if (pauseController != null)
				{
					pauseController.Pause();
				}
			}
		}

		[Header("Finds PauseController and calls Pause on click/touch.")]
		public bool active = true;
	}
}

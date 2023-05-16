using UnityEngine;

namespace UI
{
	public class PauseButton : MonoBehaviour
	{
		public virtual void OnClick()
		{
			if (active)
			{
				PauseController pauseController = FindObjectOfType<PauseController>();
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

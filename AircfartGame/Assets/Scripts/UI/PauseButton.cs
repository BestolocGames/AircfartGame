using UnityEngine;
using UnityEngine.Serialization;

namespace UI
{
	public class PauseButton : MonoBehaviour
	{
		public virtual void OnClick()
		{
			if (_active)
			{
				PauseController pauseController = FindObjectOfType<PauseController>();
				if (pauseController != null) 
					pauseController.Pause();
			}
		}

		[FormerlySerializedAs("active")] [Header("Finds PauseController and calls Pause on click/touch.")]
		public bool _active = true;
	}
}

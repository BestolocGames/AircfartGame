using UI;
using UnityEngine;
using UnityEngine.UI;

namespace CodeBase._Main
{
	public class MenuFadeOutController : MonoBehaviour
	{
		private void Start()
		{
			UIEventsPublisher.OnPlayEvent += FadeOut;
		}

		private void OnDeactivate()
		{
			UIEventsPublisher.OnPlayEvent -= FadeOut;
		}

		public virtual void FadeOut()
		{
			if (playButton)
			{
				playButton.interactable = false;
			}
			if (controlsButton)
			{
				controlsButton.interactable = false;
			}
			if (gameLogoImage)
			{
				gameLogoImage.CrossFadeAlpha(0f, 0.5f, false);
			}
			if (playButton)
			{
				Fader.FadeAlpha(this, playButton, false, 0.5f, null);
			}
			if (controlsButton)
			{
				Fader.FadeAlpha(this, controlsButton, false, 0.5f, null);
			}
			if (instructionsImage)
			{
				instructionsImage.CrossFadeAlpha(0f, 0.5f, false);
			}
		}

		public CanvasGroup playButton;

		public CanvasGroup controlsButton;

		public Image gameLogoImage;

		public Image instructionsImage;
	}
}

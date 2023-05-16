using UI;
using UnityEngine;
using UnityEngine.Serialization;
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
			if (_playButton)
			{
				_playButton.interactable = false;
			}
			if (_controlsButton)
			{
				_controlsButton.interactable = false;
			}
			if (_gameLogoImage)
			{
				_gameLogoImage.CrossFadeAlpha(0f, 0.5f, false);
			}
			if (_playButton)
			{
				Fader.FadeAlpha(this, _playButton, false, 0.5f, null);
			}
			if (_controlsButton)
			{
				Fader.FadeAlpha(this, _controlsButton, false, 0.5f, null);
			}
			if (_instructionsImage)
			{
				_instructionsImage.CrossFadeAlpha(0f, 0.5f, false);
			}
		}

		[FormerlySerializedAs("playButton")] public CanvasGroup _playButton;

		[FormerlySerializedAs("controlsButton")] public CanvasGroup _controlsButton;

		[FormerlySerializedAs("gameLogoImage")] public Image _gameLogoImage;

		[FormerlySerializedAs("instructionsImage")] public Image _instructionsImage;
	}
}

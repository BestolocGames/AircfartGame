// dnSpy decompiler from Assembly-CSharp.dll class: FlightKit.MenuFadeOutController
using System;
using UnityEngine;
using UnityEngine.UI;

namespace FlightKit
{
	public class MenuFadeOutController : MonoBehaviour
	{
		private void Start()
		{
			UIEventsPublisher.OnPlayEvent += this.FadeOut;
		}

		private void OnDeactivate()
		{
			UIEventsPublisher.OnPlayEvent -= this.FadeOut;
		}

		public virtual void FadeOut()
		{
			if (this.playButton)
			{
				this.playButton.interactable = false;
			}
			if (this.controlsButton)
			{
				this.controlsButton.interactable = false;
			}
			if (this.gameLogoImage)
			{
				this.gameLogoImage.CrossFadeAlpha(0f, 3f, false);
			}
			if (this.playButton)
			{
				Fader.FadeAlpha(this, this.playButton, false, 1f, null);
			}
			if (this.controlsButton)
			{
				Fader.FadeAlpha(this, this.controlsButton, false, 1.5f, null);
			}
			if (this.instructionsImage)
			{
				this.instructionsImage.CrossFadeAlpha(0f, 2f, false);
			}
		}

		public CanvasGroup playButton;

		public CanvasGroup controlsButton;

		public Image gameLogoImage;

		public Image instructionsImage;
	}
}

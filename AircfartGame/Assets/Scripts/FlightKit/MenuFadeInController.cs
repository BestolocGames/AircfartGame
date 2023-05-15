// dnSpy decompiler from Assembly-CSharp.dll class: FlightKit.MenuFadeInController
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace FlightKit
{
	public class MenuFadeInController : MonoBehaviour
	{
		private IEnumerator Start()
		{
			if (this.mainMenu == null)
			{
				this.mainMenu = GameObject.Find("MainMenu");
				if (this.mainMenu == null)
				{
					UnityEngine.Debug.LogError("Can't find MainMenu object in the scene.");
					yield break;
				}
			}
			this.mainMenu.SetActive(true);
			if (this.gameLogoImage)
			{
				this.gameLogoImage.enabled = false;
			}
			if (this.playButton)
			{
				this.playButton.interactable = false;
				this.playButton.alpha = 0f;
			}
			if (this.controlsButton)
			{
				this.controlsButton.interactable = false;
				this.controlsButton.alpha = 0f;
			}
			if (this.instructionsImage)
			{
				this.instructionsImage.enabled = false;
			}
			yield return new WaitForSeconds(3f);
			if (this.gameLogoImage)
			{
				this.gameLogoImage.enabled = true;
				this.gameLogoImage.canvasRenderer.SetAlpha(0f);
				this.gameLogoImage.CrossFadeAlpha(1f, 3f, false);
				yield return new WaitForSeconds(2f);
			}
			if (this.playButton)
			{
				Fader.FadeAlpha(this, this.playButton, true, 0.7f, null);
				this.playButton.interactable = true;
				yield return new WaitForSeconds(0.5f);
			}
			if (this.controlsButton)
			{
				Fader.FadeAlpha(this, this.controlsButton, true, 0.7f, null);
				this.controlsButton.interactable = true;
				yield return new WaitForSeconds(0.5f);
			}
			if (this.instructionsImage)
			{
				this.instructionsImage.enabled = true;
				this.instructionsImage.canvasRenderer.SetAlpha(0f);
				this.instructionsImage.CrossFadeAlpha(1f, 2.5f, false);
				yield return new WaitForSeconds(2f);
			}
			yield break;
		}

		public GameObject mainMenu;

		[Space]
		public CanvasGroup playButton;

		public CanvasGroup controlsButton;

		public Image gameLogoImage;

		public Image instructionsImage;
	}
}

using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace CodeBase._Main
{
	public class MenuFadeInController : MonoBehaviour
	{
		public GameObject mainMenu;

		[Space]
		public CanvasGroup playButton;

		public CanvasGroup controlsButton;

		public Image gameLogoImage;

		public Image instructionsImage;
		
		private IEnumerator Start()
		{
			if (mainMenu == null)
			{
				mainMenu = GameObject.Find("MainMenu");
				if (mainMenu == null)
				{
					Debug.LogError("Can't find MainMenu object in the scene.");
					yield break;
				}
			}
			mainMenu.SetActive(true);
			if (gameLogoImage)
			{
				gameLogoImage.enabled = false;
			}
			if (playButton)
			{
				playButton.interactable = false;
				playButton.alpha = 0f;
			}
			if (controlsButton)
			{
				controlsButton.interactable = false;
				controlsButton.alpha = 0f;
			}
			if (instructionsImage)
			{
				instructionsImage.enabled = false;
			}
			yield return new WaitForSeconds(1f);
			if (gameLogoImage)
			{
				gameLogoImage.enabled = true;
				gameLogoImage.canvasRenderer.SetAlpha(0f);
				gameLogoImage.CrossFadeAlpha(1f, 1f, false);
				yield return new WaitForSeconds(2f);
			}
			if (playButton)
			{
				Fader.FadeAlpha(this, playButton, true, 0.3f, null);
				playButton.interactable = true;
				yield return new WaitForSeconds(0.5f);
			}
			if (controlsButton)
			{
				Fader.FadeAlpha(this, controlsButton, true, 0.3f, null);
				controlsButton.interactable = true;
				yield return new WaitForSeconds(0.5f);
			}
			if (instructionsImage)
			{
				instructionsImage.enabled = true;
				instructionsImage.canvasRenderer.SetAlpha(0f);
				instructionsImage.CrossFadeAlpha(1f, 2.5f, false);
				yield return new WaitForSeconds(2f);
			}
			yield break;
		}


	}
}

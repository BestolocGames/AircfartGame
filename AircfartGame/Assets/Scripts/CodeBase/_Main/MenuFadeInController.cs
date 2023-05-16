using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace CodeBase._Main
{
	public class MenuFadeInController : MonoBehaviour
	{
		[FormerlySerializedAs("mainMenu")] public GameObject _mainMenu;

		[FormerlySerializedAs("playButton")] [Space]
		public CanvasGroup _playButton;

		[FormerlySerializedAs("controlsButton")] public CanvasGroup _controlsButton;

		[FormerlySerializedAs("gameLogoImage")] public Image _gameLogoImage;

		[FormerlySerializedAs("instructionsImage")] public Image _instructionsImage;
		
		private IEnumerator Start()
		{
			if (_mainMenu == null)
			{
				_mainMenu = GameObject.Find("MainMenu");
				if (_mainMenu == null)
				{
					Debug.LogError("Can't find MainMenu object in the scene.");
					yield break;
				}
			}
			_mainMenu.SetActive(true);
			if (_gameLogoImage)
			{
				_gameLogoImage.enabled = false;
			}
			if (_playButton)
			{
				_playButton.interactable = false;
				_playButton.alpha = 0f;
			}
			if (_controlsButton)
			{
				_controlsButton.interactable = false;
				_controlsButton.alpha = 0f;
			}
			if (_instructionsImage)
			{
				_instructionsImage.enabled = false;
			}
			yield return new WaitForSeconds(1f);
			if (_gameLogoImage)
			{
				_gameLogoImage.enabled = true;
				_gameLogoImage.canvasRenderer.SetAlpha(0f);
				_gameLogoImage.CrossFadeAlpha(1f, 1f, false);
				yield return new WaitForSeconds(2f);
			}
			if (_playButton)
			{
				Fader.FadeAlpha(this, _playButton, true, 0.3f, null);
				_playButton.interactable = true;
				yield return new WaitForSeconds(0.5f);
			}
			if (_controlsButton)
			{
				Fader.FadeAlpha(this, _controlsButton, true, 0.3f, null);
				_controlsButton.interactable = true;
				yield return new WaitForSeconds(0.5f);
			}
			if (_instructionsImage)
			{
				_instructionsImage.enabled = true;
				_instructionsImage.canvasRenderer.SetAlpha(0f);
				_instructionsImage.CrossFadeAlpha(1f, 2.5f, false);
				yield return new WaitForSeconds(2f);
			}
			yield break;
		}


	}
}

using UnityEngine;

namespace CodeBase._Main
{
	public class CrossFadeCanvasGroups : MonoBehaviour
	{
		public virtual void Activate()
		{
			toGroup.gameObject.SetActive(true);
			toGroup.alpha = 0f;
			toGroup.interactable = true;
			fromGroup.interactable = false;
			StartCoroutine(Fader.FadeAlpha(fromGroup, false, speed, delegate()
			{
				fromGroup.gameObject.SetActive(false);
			}));
			StartCoroutine(Fader.FadeAlpha(toGroup, true, speed, null));
		}

		public CanvasGroup fromGroup;

		public CanvasGroup toGroup;

		public float speed = 1f;
	}
}

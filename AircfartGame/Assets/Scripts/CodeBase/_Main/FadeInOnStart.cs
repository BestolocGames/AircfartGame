using UI;
using UnityEngine;
using UnityEngine.UI;

namespace CodeBase._Main
{
	[RequireComponent(typeof(Image))]
	public class FadeInOnStart : MonoBehaviour
	{
		private void Start()
		{
			if (listenToPlayEvent)
			{
				UIEventsPublisher.OnPlayEvent += FadeIn;
			}
		}

		private void OnDestroy()
		{
			UIEventsPublisher.OnPlayEvent -= FadeIn;
		}

		public virtual void FadeIn()
		{
			Image component = GetComponent<Image>();
			if (component != null)
			{
				component.canvasRenderer.SetAlpha(0f);
				component.enabled = true;
				component.CrossFadeAlpha(1f, duration, false);
			}
		}

		public bool listenToPlayEvent = true;

		public float duration = 1f;
	}
}

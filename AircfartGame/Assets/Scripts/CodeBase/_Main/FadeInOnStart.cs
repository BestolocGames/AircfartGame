using UI;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace CodeBase._Main
{
	[RequireComponent(typeof(Image))]
	public class FadeInOnStart : MonoBehaviour
	{
		private void Start()
		{
			if (_listenToPlayEvent)
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
				component.CrossFadeAlpha(1f, _duration, false);
			}
		}

		[FormerlySerializedAs("listenToPlayEvent")] public bool _listenToPlayEvent = true;

		[FormerlySerializedAs("duration")] public float _duration = 1f;
	}
}

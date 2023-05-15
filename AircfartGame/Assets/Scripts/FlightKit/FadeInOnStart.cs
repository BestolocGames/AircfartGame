// dnSpy decompiler from Assembly-CSharp.dll class: FlightKit.FadeInOnStart
using System;
using UnityEngine;
using UnityEngine.UI;

namespace FlightKit
{
	[RequireComponent(typeof(Image))]
	public class FadeInOnStart : MonoBehaviour
	{
		private void Start()
		{
			if (this.listenToPlayEvent)
			{
				UIEventsPublisher.OnPlayEvent += this.FadeIn;
			}
		}

		private void OnDestroy()
		{
			UIEventsPublisher.OnPlayEvent -= this.FadeIn;
		}

		public virtual void FadeIn()
		{
			Image component = base.GetComponent<Image>();
			if (component != null)
			{
				component.canvasRenderer.SetAlpha(0f);
				component.enabled = true;
				component.CrossFadeAlpha(1f, this.duration, false);
			}
		}

		public bool listenToPlayEvent = true;

		public float duration = 1f;
	}
}

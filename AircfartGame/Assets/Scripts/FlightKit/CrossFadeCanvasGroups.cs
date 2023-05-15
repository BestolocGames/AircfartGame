// dnSpy decompiler from Assembly-CSharp.dll class: FlightKit.CrossFadeCanvasGroups
using System;
using UnityEngine;

namespace FlightKit
{
	public class CrossFadeCanvasGroups : MonoBehaviour
	{
		public virtual void Activate()
		{
			this.toGroup.gameObject.SetActive(true);
			this.toGroup.alpha = 0f;
			this.toGroup.interactable = true;
			this.fromGroup.interactable = false;
			base.StartCoroutine(Fader.FadeAlpha(this.fromGroup, false, this.speed, delegate()
			{
				this.fromGroup.gameObject.SetActive(false);
			}));
			base.StartCoroutine(Fader.FadeAlpha(this.toGroup, true, this.speed, null));
		}

		public CanvasGroup fromGroup;

		public CanvasGroup toGroup;

		public float speed = 1f;
	}
}

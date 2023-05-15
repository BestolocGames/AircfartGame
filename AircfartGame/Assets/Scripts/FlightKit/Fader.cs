// dnSpy decompiler from Assembly-CSharp.dll class: FlightKit.Fader
using System;
using System.Collections;
using UnityEngine;

namespace FlightKit
{
	public class Fader : MonoBehaviour
	{
		public static void FadeAlpha(MonoBehaviour container, CanvasGroup group, bool fadeIn, float speed, Action onComplete = null)
		{
			if (group.gameObject.activeSelf)
			{
				container.StartCoroutine(Fader.FadeAlpha(group, fadeIn, speed, onComplete));
			}
		}

		public static IEnumerator FadeAlpha(CanvasGroup group, bool fadeIn, float speed, Action onComplete = null)
		{
			float timeLast = Time.realtimeSinceStartup;
			float timeCurrent = timeLast;
			while ((fadeIn && group.alpha < 1f) || (!fadeIn && group.alpha > 0f))
			{
				timeCurrent = Time.realtimeSinceStartup;
				group.alpha += (float)((!fadeIn) ? -1 : 1) * speed * (timeCurrent - timeLast);
				timeLast = timeCurrent;
				yield return null;
			}
			if (onComplete != null)
			{
				onComplete();
			}
			yield break;
		}
	}
}

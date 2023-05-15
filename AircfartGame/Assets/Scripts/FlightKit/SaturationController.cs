// dnSpy decompiler from Assembly-CSharp.dll class: FlightKit.SaturationController
using System;
using System.Collections;
using UnityEngine;
using UnityStandardAssets.ImageEffects;

namespace FlightKit
{
	public class SaturationController : MonoBehaviour
	{
		private void Start()
		{
			this._colorCorrectionFx = UnityEngine.Object.FindObjectOfType<ColorCorrectionCurves>();
			if (this._colorCorrectionFx)
			{
				TakeOffPublisher.OnTakeOffEvent += this.OnTakeOff;
			}
		}

		private void OnDisable()
		{
			TakeOffPublisher.OnTakeOffEvent -= this.OnTakeOff;
		}

		private void OnTakeOff()
		{
			base.StartCoroutine(this.OnTakeOffCore());
		}

		private IEnumerator OnTakeOffCore()
		{
			yield return new WaitForSeconds(0.5f);
			this._saturationTweenStartTime = Time.time;
			WaitForEndOfFrame wait = new WaitForEndOfFrame();
			while (this._colorCorrectionFx.saturation < 0.99f)
			{
				float deltaTime = Time.time - this._saturationTweenStartTime;
				this._colorCorrectionFx.saturation = Mathf.SmoothStep(0f, 1f, deltaTime * 1.2f);
				yield return wait;
			}
			this._colorCorrectionFx.saturation = 1f;
			yield break;
		}

		private ColorCorrectionCurves _colorCorrectionFx;

		private float _saturationTweenStartTime;
	}
}

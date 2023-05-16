using System.Collections;
using CodeBase._ImageEffects;
using UnityEngine;

namespace CodeBase._Main
{
	public class SaturationController : MonoBehaviour
	{
		private void Start()
		{
			_colorCorrectionFx = FindObjectOfType<ColorCorrectionCurves>();
			if (_colorCorrectionFx)
			{
				TakeOffPublisher.OnTakeOffEvent += OnTakeOff;
			}
		}

		private void OnDisable()
		{
			TakeOffPublisher.OnTakeOffEvent -= OnTakeOff;
		}

		private void OnTakeOff()
		{
			StartCoroutine(OnTakeOffCore());
		}

		private IEnumerator OnTakeOffCore()
		{
			yield return new WaitForSeconds(0.5f);
			_saturationTweenStartTime = Time.time;
			WaitForEndOfFrame wait = new WaitForEndOfFrame();
			while (_colorCorrectionFx._saturation < 0.99f)
			{
				float deltaTime = Time.time - _saturationTweenStartTime;
				_colorCorrectionFx._saturation = Mathf.SmoothStep(0f, 1f, deltaTime * 1.2f);
				yield return wait;
			}
			_colorCorrectionFx._saturation = 1f;
			yield break;
		}

		private ColorCorrectionCurves _colorCorrectionFx;

		private float _saturationTweenStartTime;
	}
}

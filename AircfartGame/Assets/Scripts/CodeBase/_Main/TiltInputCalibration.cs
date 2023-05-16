using System.Collections;
using CodeBase._CrossPlatformInput;
using UI;
using UnityEngine;
using UnityEngine.Serialization;

namespace CodeBase._Main
{
	public class TiltInputCalibration : MonoBehaviour
	{
		private void OnEnable()
		{
			UIEventsPublisher.OnPlayEvent += CalibrateDelayed;
			PauseController.OnUnPauseEvent += Calibrate;
		}

		private void OnDisable()
		{
			UIEventsPublisher.OnPlayEvent -= CalibrateDelayed;
			PauseController.OnUnPauseEvent -= Calibrate;
		}

		public virtual void CalibrateDelayed()
		{
			if (ControlsPrefs.IsTiltEnabled)
			{
				StartCoroutine(CalibrateCoroutine(_delayAfterStartPlay));
			}
		}

		public virtual void Calibrate()
		{
			if (ControlsPrefs.IsTiltEnabled)
			{
				StartCoroutine(CalibrateCoroutine(0f));
			}
		}

		private IEnumerator CalibrateCoroutine(float delay = 0f)
		{
			if (delay > 0f)
			{
				yield return new WaitForSeconds(delay);
			}
			if (_calibrationPopup != null)
			{
				_calibrationPopup.SetActive(true);
			}
			yield return new WaitForSeconds(3f);
			if (_calibrationTarget == null)
			{
				yield break;
			}
			float currentAngle = 0f;
			if (Input.acceleration != Vector3.zero)
			{
				TiltInput.AxisOptions tiltAroundAxis = _calibrationTarget._tiltAroundAxis;
				if (tiltAroundAxis != TiltInput.AxisOptions.ForwardAxis)
				{
					if (tiltAroundAxis == TiltInput.AxisOptions.SidewaysAxis)
					{
						currentAngle = Mathf.Atan2(Input.acceleration.z, -Input.acceleration.y) * 57.29578f;
					}
				}
				else
				{
					currentAngle = Mathf.Atan2(Input.acceleration.x, -Input.acceleration.y) * 57.29578f;
				}
			}
			_calibrationTarget._centreAngleOffset = -currentAngle;
			if (_calibrationPopup != null)
			{
				_calibrationPopup.SetActive(false);
			}
			yield break;
		}

		[FormerlySerializedAs("calibrationTarget")] public TiltInput _calibrationTarget;

		[FormerlySerializedAs("calibrationPopup")] public GameObject _calibrationPopup;

		[FormerlySerializedAs("delayAfterStartPlay")] public float _delayAfterStartPlay = 8f;
	}
}

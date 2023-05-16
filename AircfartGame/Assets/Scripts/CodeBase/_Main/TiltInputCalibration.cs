using System.Collections;
using CodeBase._CrossPlatformInput;
using UI;
using UnityEngine;

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
				StartCoroutine(CalibrateCoroutine(delayAfterStartPlay));
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
			if (calibrationPopup != null)
			{
				calibrationPopup.SetActive(true);
			}
			yield return new WaitForSeconds(3f);
			if (calibrationTarget == null)
			{
				yield break;
			}
			float currentAngle = 0f;
			if (Input.acceleration != Vector3.zero)
			{
				TiltInput.AxisOptions tiltAroundAxis = calibrationTarget.tiltAroundAxis;
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
			calibrationTarget.centreAngleOffset = -currentAngle;
			if (calibrationPopup != null)
			{
				calibrationPopup.SetActive(false);
			}
			yield break;
		}

		public TiltInput calibrationTarget;

		public GameObject calibrationPopup;

		public float delayAfterStartPlay = 8f;
	}
}

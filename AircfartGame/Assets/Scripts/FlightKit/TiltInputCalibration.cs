// dnSpy decompiler from Assembly-CSharp.dll class: FlightKit.TiltInputCalibration
using System;
using System.Collections;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

namespace FlightKit
{
	public class TiltInputCalibration : MonoBehaviour
	{
		private void OnEnable()
		{
			UIEventsPublisher.OnPlayEvent += this.CalibrateDelayed;
			PauseController.OnUnPauseEvent += this.Calibrate;
		}

		private void OnDisable()
		{
			UIEventsPublisher.OnPlayEvent -= this.CalibrateDelayed;
			PauseController.OnUnPauseEvent -= this.Calibrate;
		}

		public virtual void CalibrateDelayed()
		{
			if (ControlsPrefs.IsTiltEnabled)
			{
				base.StartCoroutine(this.CalibrateCoroutine(this.delayAfterStartPlay));
			}
		}

		public virtual void Calibrate()
		{
			if (ControlsPrefs.IsTiltEnabled)
			{
				base.StartCoroutine(this.CalibrateCoroutine(0f));
			}
		}

		private IEnumerator CalibrateCoroutine(float delay = 0f)
		{
			if (delay > 0f)
			{
				yield return new WaitForSeconds(delay);
			}
			if (this.calibrationPopup != null)
			{
				this.calibrationPopup.SetActive(true);
			}
			yield return new WaitForSeconds(3f);
			if (this.calibrationTarget == null)
			{
				yield break;
			}
			float currentAngle = 0f;
			if (Input.acceleration != Vector3.zero)
			{
				TiltInput.AxisOptions tiltAroundAxis = this.calibrationTarget.tiltAroundAxis;
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
			this.calibrationTarget.centreAngleOffset = -currentAngle;
			if (this.calibrationPopup != null)
			{
				this.calibrationPopup.SetActive(false);
			}
			yield break;
		}

		public TiltInput calibrationTarget;

		public GameObject calibrationPopup;

		public float delayAfterStartPlay = 8f;
	}
}

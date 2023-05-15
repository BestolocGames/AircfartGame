// dnSpy decompiler from Assembly-CSharp.dll class: FlightKit.AirplaneUserControl
using System;
using System.Collections;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;
using UnityStandardAssets.Vehicles.Aeroplane;

namespace FlightKit
{
	[RequireComponent(typeof(AeroplaneController))]
	public class AirplaneUserControl : MonoBehaviour
	{
		private void Awake()
		{
			this._airplane = base.GetComponent<AeroplaneController>();
		}

		private IEnumerator Start()
		{
			float aerodynamicEffect = this._airplane.AerodynamicEffect;
			this._airplane.AerodynamicEffect = 0f;
			yield return new WaitForSeconds(3f);
			this._airplane.AerodynamicEffect = aerodynamicEffect;
			yield break;
		}

		private void FixedUpdate()
		{
			float num = (!ControlsPrefs.IsMouseEnabled) ? 0f : CrossPlatformInputManager.GetAxis("Mouse Y");
			float num2 = (!ControlsPrefs.IsMouseEnabled) ? 0f : CrossPlatformInputManager.GetAxis("Mouse X");
			float rollInput = (!ControlsPrefs.IsRollEnabled) ? 0f : (CrossPlatformInputManager.GetAxis("Roll") + num2);
			float pitchInput = ((!ControlsPrefs.IsInversePitch) ? 1f : -1f) * CrossPlatformInputManager.GetAxis("Pitch") + num;
			float axis = CrossPlatformInputManager.GetAxis("Yaw");
			bool button = CrossPlatformInputManager.GetButton("Brakes");
			float throttleInput = (float)((!button) ? 1 : -1);
			rollInput = CrossPlatformInputManager.GetAxis("Roll");
			this.AdjustInputForMobileControls(ref rollInput, ref pitchInput, ref throttleInput);
			this._airplane.Move(rollInput, pitchInput, axis, throttleInput, button);
		}

		private void AdjustInputForMobileControls(ref float roll, ref float pitch, ref float throttle)
		{
			float num = roll * this.maxRollAngle * 0.0174532924f;
			float num2 = pitch * this.maxPitchAngle * 0.0174532924f;
			roll = Mathf.Clamp(num - this._airplane.RollAngle, -1f, 1f);
			pitch = Mathf.Clamp(num2 - this._airplane.PitchAngle, -1f, 1f);
			float num3 = throttle * 0.5f + 0.5f;
			throttle = Mathf.Clamp(num3 - this._airplane.Throttle, -1f, 1f);
		}

		public float maxRollAngle = 80f;

		public float maxPitchAngle = 80f;

		private AeroplaneController _airplane;
	}
}

using System.Collections;
using CodeBase._CrossPlatformInput;
using CodeBase._Main.Player.Vehicles_Aeroplane;
using UnityEngine;

namespace CodeBase._Main.Player
{
	[RequireComponent(typeof(AeroplaneController))]
	public class AirplaneUserControl : MonoBehaviour
	{
		public float maxRollAngle = 80f;

		public float maxPitchAngle = 80f;

		private AeroplaneController _airplane;
		
		private void Awake() => 
			_airplane = GetComponent<AeroplaneController>();

		private IEnumerator Start()
		{
			float aerodynamicEffect = _airplane.AerodynamicEffect;
			_airplane.AerodynamicEffect = 0f;
			yield return new WaitForSeconds(3f);
			_airplane.AerodynamicEffect = aerodynamicEffect;
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
			AdjustInputForMobileControls(ref rollInput, ref pitchInput, ref throttleInput);
			_airplane.Move(rollInput, pitchInput, axis, throttleInput, button);
		}

		private void AdjustInputForMobileControls(ref float roll, ref float pitch, ref float throttle)
		{
			float num = roll * maxRollAngle * 0.0174532924f;
			float num2 = pitch * maxPitchAngle * 0.0174532924f;
			roll = Mathf.Clamp(num - _airplane.RollAngle, -1f, 1f);
			pitch = Mathf.Clamp(num2 - _airplane.PitchAngle, -1f, 1f);
			float num3 = throttle * 0.5f + 0.5f;
			throttle = Mathf.Clamp(num3 - _airplane.Throttle, -1f, 1f);
		}
	}
}

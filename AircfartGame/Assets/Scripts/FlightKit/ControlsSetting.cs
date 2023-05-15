// dnSpy decompiler from Assembly-CSharp.dll class: FlightKit.ControlsSetting
using System;
using UnityEngine;
using UnityEngine.UI;

namespace FlightKit
{
	public class ControlsSetting : MonoBehaviour
	{
		private void Start()
		{
			if (ControlsPrefs.IsMouseEnabled)
			{
				if (this.mouseControls)
				{
					this.mouseControls.isOn = true;
				}
			}
			else if (ControlsPrefs.IsRollEnabled)
			{
				if (this.classicControls)
				{
					this.classicControls.isOn = true;
				}
			}
			else if (this.casualControls)
			{
				this.casualControls.isOn = true;
			}
			if (this.inversePitchStandalone)
			{
				this.inversePitchStandalone.isOn = ControlsPrefs.IsInversePitch;
			}
			if (ControlsPrefs.IsTiltEnabled)
			{
				if (this.tiltControls)
				{
					this.tiltControls.isOn = true;
				}
			}
			else if (this.touchControls)
			{
				this.touchControls.isOn = true;
			}
			if (this.inversePitchMobile)
			{
				this.inversePitchMobile.isOn = ControlsPrefs.IsInversePitch;
			}
		}

		public virtual void OnRollEnabledChanged(bool activated)
		{
			ControlsPrefs.IsRollEnabled = activated;
		}

		public virtual void OnMouseEnabledChanged(bool activated)
		{
			ControlsPrefs.IsMouseEnabled = activated;
			if (activated)
			{
				ControlsPrefs.IsRollEnabled = true;
			}
		}

		public virtual void OnInversePitchChanged(bool activated)
		{
			ControlsPrefs.IsInversePitch = activated;
		}

		public virtual void OnTiltEnabledChanged(bool activated)
		{
			ControlsPrefs.IsTiltEnabled = activated;
		}

		[Header("Standalone controls UI (Leave empty if not targeting standalone):")]
		public Toggle classicControls;

		public Toggle mouseControls;

		public Toggle casualControls;

		[Space]
		public Toggle inversePitchStandalone;

		[Header("Mobile controls UI (Leave empty if not targeting mobile):")]
		public Toggle touchControls;

		public Toggle tiltControls;

		[Space]
		public Toggle inversePitchMobile;
	}
}

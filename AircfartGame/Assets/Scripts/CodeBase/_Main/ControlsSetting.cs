using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace CodeBase._Main
{
	public class ControlsSetting : MonoBehaviour
	{
		private void Start()
		{
			if (ControlsPrefs.IsMouseEnabled)
			{
				if (_mouseControls)
				{
					_mouseControls.isOn = true;
				}
			}
			else if (ControlsPrefs.IsRollEnabled)
			{
				if (_classicControls)
				{
					_classicControls.isOn = true;
				}
			}
			else if (_casualControls)
			{
				_casualControls.isOn = true;
			}
			if (_inversePitchStandalone)
			{
				_inversePitchStandalone.isOn = ControlsPrefs.IsInversePitch;
			}
			if (ControlsPrefs.IsTiltEnabled)
			{
				if (_tiltControls)
				{
					_tiltControls.isOn = true;
				}
			}
			else if (_touchControls)
			{
				_touchControls.isOn = true;
			}
			if (_inversePitchMobile)
			{
				_inversePitchMobile.isOn = ControlsPrefs.IsInversePitch;
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

		[FormerlySerializedAs("classicControls")] [Header("Standalone controls UI (Leave empty if not targeting standalone):")]
		public Toggle _classicControls;

		[FormerlySerializedAs("mouseControls")] public Toggle _mouseControls;

		[FormerlySerializedAs("casualControls")] public Toggle _casualControls;

		[FormerlySerializedAs("inversePitchStandalone")] [Space]
		public Toggle _inversePitchStandalone;

		[FormerlySerializedAs("touchControls")] [Header("Mobile controls UI (Leave empty if not targeting mobile):")]
		public Toggle _touchControls;

		[FormerlySerializedAs("tiltControls")] public Toggle _tiltControls;

		[FormerlySerializedAs("inversePitchMobile")] [Space]
		public Toggle _inversePitchMobile;
	}
}

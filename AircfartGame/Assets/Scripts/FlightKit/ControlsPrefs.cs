// dnSpy decompiler from Assembly-CSharp.dll class: FlightKit.ControlsPrefs
using System;
using UnityEngine;

namespace FlightKit
{
	public class ControlsPrefs
	{
		static ControlsPrefs()
		{
			if (!PlayerPrefs.HasKey(ControlsPrefs.PREF_KEY_ROLL_ENABLED))
			{
				ControlsPrefs.IsRollEnabled = true;
				ControlsPrefs.IsMouseEnabled = false;
				ControlsPrefs.IsTiltEnabled = true;
				ControlsPrefs.IsInversePitch = false;
			}
			else
			{
				ControlsPrefs.IsRollEnabled = (PlayerPrefs.GetInt(ControlsPrefs.PREF_KEY_ROLL_ENABLED) == 1);
				ControlsPrefs.IsMouseEnabled = (PlayerPrefs.GetInt(ControlsPrefs.PREF_KEY_MOUSE_ENABLED) == 1);
				ControlsPrefs.IsTiltEnabled = (PlayerPrefs.GetInt(ControlsPrefs.PREF_KEY_TILT_ENABLED) == 1);
				ControlsPrefs.IsInversePitch = (PlayerPrefs.GetInt(ControlsPrefs.PREF_KEY_INVERSE_PITCH) == 1);
			}
		}

		public static event GameActions.SimpleAction OnTiltEnabledEvent;

		public static event GameActions.SimpleAction OnTiltDisabledEvent;

		public static bool IsRollEnabled
		{
			get
			{
				return ControlsPrefs._isRollEnabled;
			}
			set
			{
				ControlsPrefs._isRollEnabled = value;
				PlayerPrefs.SetInt(ControlsPrefs.PREF_KEY_ROLL_ENABLED, (!value) ? 0 : 1);
				PlayerPrefs.Save();
			}
		}

		public static bool IsMouseEnabled
		{
			get
			{
				return ControlsPrefs._isMouseEnabled;
			}
			set
			{
				ControlsPrefs._isMouseEnabled = value;
				PlayerPrefs.SetInt(ControlsPrefs.PREF_KEY_MOUSE_ENABLED, (!value) ? 0 : 1);
				PlayerPrefs.Save();
			}
		}

		public static bool IsTiltEnabled
		{
			get
			{
				return ControlsPrefs._isTiltEnabled;
			}
			set
			{
				ControlsPrefs._isTiltEnabled = value;
				PlayerPrefs.SetInt(ControlsPrefs.PREF_KEY_TILT_ENABLED, (!value) ? 0 : 1);
				PlayerPrefs.Save();
				if (value)
				{
					if (ControlsPrefs.OnTiltEnabledEvent != null)
					{
						ControlsPrefs.OnTiltEnabledEvent();
					}
				}
				else if (ControlsPrefs.OnTiltDisabledEvent != null)
				{
					ControlsPrefs.OnTiltDisabledEvent();
				}
			}
		}

		public static bool IsInversePitch
		{
			get
			{
				return ControlsPrefs._isInversePitch;
			}
			set
			{
				ControlsPrefs._isInversePitch = value;
				PlayerPrefs.SetInt(ControlsPrefs.PREF_KEY_INVERSE_PITCH, (!value) ? 0 : 1);
				PlayerPrefs.Save();
			}
		}

		private static bool _isRollEnabled;

		private static bool _isMouseEnabled;

		private static bool _isTiltEnabled;

		private static bool _isInversePitch;

		private static string PREF_KEY_ROLL_ENABLED = "FlightControls_RollEnabled";

		private static string PREF_KEY_MOUSE_ENABLED = "FlightControls_MouseEnabled";

		private static string PREF_KEY_TILT_ENABLED = "FlightControls_TiltEnabled";

		private static string PREF_KEY_INVERSE_PITCH = "FlightControls_InversePitch";
	}
}

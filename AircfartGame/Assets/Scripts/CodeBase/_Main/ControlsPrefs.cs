using UnityEngine;

namespace CodeBase._Main
{
	public class ControlsPrefs
	{
		static ControlsPrefs()
		{
			if (!PlayerPrefs.HasKey(PREF_KEY_ROLL_ENABLED))
			{
				IsRollEnabled = true;
				IsMouseEnabled = false;
				IsTiltEnabled = true;
				IsInversePitch = false;
			}
			else
			{
				IsRollEnabled = (PlayerPrefs.GetInt(PREF_KEY_ROLL_ENABLED) == 1);
				IsMouseEnabled = (PlayerPrefs.GetInt(PREF_KEY_MOUSE_ENABLED) == 1);
				IsTiltEnabled = (PlayerPrefs.GetInt(PREF_KEY_TILT_ENABLED) == 1);
				IsInversePitch = (PlayerPrefs.GetInt(PREF_KEY_INVERSE_PITCH) == 1);
			}
		}

		public static event GameActions.SimpleAction OnTiltEnabledEvent;

		public static event GameActions.SimpleAction OnTiltDisabledEvent;

		public static bool IsRollEnabled
		{
			get
			{
				return _isRollEnabled;
			}
			set
			{
				_isRollEnabled = value;
				PlayerPrefs.SetInt(PREF_KEY_ROLL_ENABLED, (!value) ? 0 : 1);
				PlayerPrefs.Save();
			}
		}

		public static bool IsMouseEnabled
		{
			get
			{
				return _isMouseEnabled;
			}
			set
			{
				_isMouseEnabled = value;
				PlayerPrefs.SetInt(PREF_KEY_MOUSE_ENABLED, (!value) ? 0 : 1);
				PlayerPrefs.Save();
			}
		}

		public static bool IsTiltEnabled
		{
			get
			{
				return _isTiltEnabled;
			}
			set
			{
				_isTiltEnabled = value;
				PlayerPrefs.SetInt(PREF_KEY_TILT_ENABLED, (!value) ? 0 : 1);
				PlayerPrefs.Save();
				if (value)
				{
					if (OnTiltEnabledEvent != null)
					{
						OnTiltEnabledEvent();
					}
				}
				else if (OnTiltDisabledEvent != null)
				{
					OnTiltDisabledEvent();
				}
			}
		}

		public static bool IsInversePitch
		{
			get
			{
				return _isInversePitch;
			}
			set
			{
				_isInversePitch = value;
				PlayerPrefs.SetInt(PREF_KEY_INVERSE_PITCH, (!value) ? 0 : 1);
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

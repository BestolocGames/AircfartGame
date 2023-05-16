using UnityEngine;

namespace CodeBase._Main
{
	public class ControlsPrefs
	{
		static ControlsPrefs()
		{
			if (!PlayerPrefs.HasKey(_prefKeyRollEnabled))
			{
				IsRollEnabled = true;
				IsMouseEnabled = false;
				IsTiltEnabled = true;
				IsInversePitch = false;
			}
			else
			{
				IsRollEnabled = (PlayerPrefs.GetInt(_prefKeyRollEnabled) == 1);
				IsMouseEnabled = (PlayerPrefs.GetInt(_prefKeyMouseEnabled) == 1);
				IsTiltEnabled = (PlayerPrefs.GetInt(_prefKeyTiltEnabled) == 1);
				IsInversePitch = (PlayerPrefs.GetInt(_prefKeyInversePitch) == 1);
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
				PlayerPrefs.SetInt(_prefKeyRollEnabled, (!value) ? 0 : 1);
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
				PlayerPrefs.SetInt(_prefKeyMouseEnabled, (!value) ? 0 : 1);
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
				PlayerPrefs.SetInt(_prefKeyTiltEnabled, (!value) ? 0 : 1);
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
				PlayerPrefs.SetInt(_prefKeyInversePitch, (!value) ? 0 : 1);
				PlayerPrefs.Save();
			}
		}

		private static bool _isRollEnabled;

		private static bool _isMouseEnabled;

		private static bool _isTiltEnabled;

		private static bool _isInversePitch;

		private static string _prefKeyRollEnabled = "FlightControls_RollEnabled";

		private static string _prefKeyMouseEnabled = "FlightControls_MouseEnabled";

		private static string _prefKeyTiltEnabled = "FlightControls_TiltEnabled";

		private static string _prefKeyInversePitch = "FlightControls_InversePitch";
	}
}

using System;
using CodeBase._CrossPlatformInput._PlatformSpecific;
using UnityEngine;

namespace CodeBase._CrossPlatformInput
{
	public static class CrossPlatformInputManager
	{
		static CrossPlatformInputManager()
		{
			_activeInput = _sTouchInput;
		}

		public static void SwitchActiveInputMethod(ActiveInputMethod activeInputMethod)
		{
			if (activeInputMethod != ActiveInputMethod.Hardware)
			{
				if (activeInputMethod == ActiveInputMethod.Touch) 
					_activeInput = _sTouchInput;
			}
			else
				_activeInput = _sHardwareInput;
		}

		public static bool AxisExists(string name) => 
			_activeInput.AxisExists(name);

		public static bool ButtonExists(string name) => 
			_activeInput.ButtonExists(name);

		public static void RegisterVirtualAxis(VirtualAxis axis)
		{
			_activeInput.RegisterVirtualAxis(axis);
		}

		public static void RegisterVirtualButton(VirtualButton button)
		{
			_activeInput.RegisterVirtualButton(button);
		}

		public static void UnRegisterVirtualAxis(string name)
		{
			if (name == null)
			{
				throw new ArgumentNullException("name");
			}
			_activeInput.UnRegisterVirtualAxis(name);
		}

		public static void UnRegisterVirtualButton(string name)
		{
			_activeInput.UnRegisterVirtualButton(name);
		}

		public static VirtualAxis VirtualAxisReference(string name)
		{
			return _activeInput.VirtualAxisReference(name);
		}

		public static float GetAxis(string name)
		{
			return GetAxis(name, false);
		}

		public static float GetAxisRaw(string name)
		{
			return GetAxis(name, true);
		}

		private static float GetAxis(string name, bool raw)
		{
			return _activeInput.GetAxis(name, raw);
		}

		public static bool GetButton(string name)
		{
			return _activeInput.GetButton(name);
		}

		public static bool GetButtonDown(string name)
		{
			return _activeInput.GetButtonDown(name);
		}

		public static bool GetButtonUp(string name)
		{
			return _activeInput.GetButtonUp(name);
		}

		public static void SetButtonDown(string name)
		{
			_activeInput.SetButtonDown(name);
		}

		public static void SetButtonUp(string name)
		{
			_activeInput.SetButtonUp(name);
		}

		public static void SetAxisPositive(string name)
		{
			_activeInput.SetAxisPositive(name);
		}

		public static void SetAxisNegative(string name)
		{
			_activeInput.SetAxisNegative(name);
		}

		public static void SetAxisZero(string name)
		{
			_activeInput.SetAxisZero(name);
		}

		public static void SetAxis(string name, float value)
		{
			_activeInput.SetAxis(name, value);
		}

		public static Vector3 MousePosition
		{
			get
			{
				return _activeInput.MousePosition();
			}
		}

		public static void SetVirtualMousePositionX(float f)
		{
			_activeInput.SetVirtualMousePositionX(f);
		}

		public static void SetVirtualMousePositionY(float f)
		{
			_activeInput.SetVirtualMousePositionY(f);
		}

		public static void SetVirtualMousePositionZ(float f)
		{
			_activeInput.SetVirtualMousePositionZ(f);
		}

		private static VirtualInput _activeInput;

		private static VirtualInput _sTouchInput = new MobileInput();

		private static VirtualInput _sHardwareInput = new StandaloneInput();

		public enum ActiveInputMethod
		{
			Hardware,
			Touch
		}

		public class VirtualAxis
		{
			public VirtualAxis(string name) : this(name, true)
			{
			}

			public VirtualAxis(string name, bool matchToInputSettings)
			{
				this.Name = name;
				MatchWithInputManager = matchToInputSettings;
			}

			public string Name { get; private set; }

			public bool MatchWithInputManager { get; private set; }

			public void Remove() => 
				UnRegisterVirtualAxis(Name);

			public void Update(float value) => 
				_value = value;

			public float GetValue
				=> _value;

			public float GetValueRaw
				=> _value;

			private float _value;
		}

		public class VirtualButton
		{
			public VirtualButton(string name) : this(name, true)
			{
			}

			public VirtualButton(string name, bool matchToInputSettings)
			{
				this.Name = name;
				MatchWithInputManager = matchToInputSettings;
			}

			public string Name { get; private set; }

			public bool MatchWithInputManager { get; private set; }

			public void Pressed()
			{
				if (_pressed)
				{
					return;
				}
				_pressed = true;
				_mLastPressedFrame = Time.frameCount;
			}

			public void Released()
			{
				_pressed = false;
				_mReleasedFrame = Time.frameCount;
			}

			public void Remove()
			{
				UnRegisterVirtualButton(Name);
			}

			public bool GetButton
				=> _pressed;

			public bool GetButtonDown
				=> _mLastPressedFrame - Time.frameCount == -1;

			public bool GetButtonUp
				=> _mReleasedFrame == Time.frameCount - 1;

			private int _mLastPressedFrame = -5;

			private int _mReleasedFrame = -5;

			private bool _pressed;
		}
	}
}

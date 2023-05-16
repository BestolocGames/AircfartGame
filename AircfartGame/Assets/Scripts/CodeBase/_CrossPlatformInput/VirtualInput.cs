using System.Collections.Generic;
using UnityEngine;

namespace CodeBase._CrossPlatformInput
{
	public abstract class VirtualInput
	{
		public Vector3 VirtualMousePosition { get; private set; }

		public bool AxisExists(string name) => 
			MVirtualAxes.ContainsKey(name);

		public bool ButtonExists(string name) => 
			MVirtualButtons.ContainsKey(name);

		public void RegisterVirtualAxis(CrossPlatformInputManager.VirtualAxis axis)
		{
			if (MVirtualAxes.ContainsKey(axis.Name))
			{
				UnRegisterVirtualAxis(axis.Name);
				RegisterVirtualAxis(axis);
			}
			else
			{
				MVirtualAxes.Add(axis.Name, axis);
				if (!axis.MatchWithInputManager) MAlwaysUseVirtual.Add(axis.Name);
			}
		}

		public void RegisterVirtualButton(CrossPlatformInputManager.VirtualButton button)
		{
			if (MVirtualButtons.ContainsKey(button.Name))
				Debug.LogError("There is already a virtual button named " + button.Name + " registered.");
			else
			{
				MVirtualButtons.Add(button.Name, button);
				if (!button.MatchWithInputManager) MAlwaysUseVirtual.Add(button.Name);
			}
		}

		public void UnRegisterVirtualAxis(string name)
		{
			if (MVirtualAxes.ContainsKey(name)) 
				MVirtualAxes.Remove(name);
		}

		public void UnRegisterVirtualButton(string name)
		{
			if (MVirtualButtons.ContainsKey(name)) 
				MVirtualButtons.Remove(name);
		}

		public CrossPlatformInputManager.VirtualAxis VirtualAxisReference(string name) => 
			(!MVirtualAxes.ContainsKey(name)) ? null : MVirtualAxes[name];

		public void SetVirtualMousePositionX(float f) => 
			VirtualMousePosition = new Vector3(f, VirtualMousePosition.y, VirtualMousePosition.z);

		public void SetVirtualMousePositionY(float f) => 
			VirtualMousePosition = new Vector3(VirtualMousePosition.x, f, VirtualMousePosition.z);

		public void SetVirtualMousePositionZ(float f) => 
			VirtualMousePosition = new Vector3(VirtualMousePosition.x, VirtualMousePosition.y, f);

		public abstract float GetAxis(string name, bool raw);

		public abstract bool GetButton(string name);

		public abstract bool GetButtonDown(string name);

		public abstract bool GetButtonUp(string name);

		public abstract void SetButtonDown(string name);

		public abstract void SetButtonUp(string name);

		public abstract void SetAxisPositive(string name);

		public abstract void SetAxisNegative(string name);

		public abstract void SetAxisZero(string name);

		public abstract void SetAxis(string name, float value);

		public abstract Vector3 MousePosition();

		protected Dictionary<string, CrossPlatformInputManager.VirtualAxis> MVirtualAxes = new Dictionary<string, CrossPlatformInputManager.VirtualAxis>();

		protected Dictionary<string, CrossPlatformInputManager.VirtualButton> MVirtualButtons = new Dictionary<string, CrossPlatformInputManager.VirtualButton>();

		protected List<string> MAlwaysUseVirtual = new List<string>();
	}
}

using UnityEngine;
using UnityEngine.Serialization;

namespace CodeBase._CrossPlatformInput
{
	public class ButtonHandler : MonoBehaviour
	{
		[FormerlySerializedAs("Name")] public string _name;

		public void SetDownState()
		{
			CrossPlatformInputManager.SetButtonDown(_name);
		}

		public void SetUpState()
		{
			CrossPlatformInputManager.SetButtonUp(_name);
		}

		public void SetAxisPositiveState()
		{
			CrossPlatformInputManager.SetAxisPositive(_name);
		}

		public void SetAxisNeutralState()
		{
			CrossPlatformInputManager.SetAxisZero(_name);
		}

		public void SetAxisNegativeState()
		{
			CrossPlatformInputManager.SetAxisNegative(_name);
		}

	}
}

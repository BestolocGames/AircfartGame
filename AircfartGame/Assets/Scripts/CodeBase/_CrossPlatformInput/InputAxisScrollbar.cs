using UnityEngine;

namespace CodeBase._CrossPlatformInput
{
	public class InputAxisScrollbar : MonoBehaviour
	{		
		public string axis;

		public void HandleInput(float value)
		{
			CrossPlatformInputManager.SetAxis(axis, value * 2f - 1f);
		}

	}
}

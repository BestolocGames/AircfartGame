using UnityEngine;
using UnityEngine.Serialization;

namespace CodeBase._CrossPlatformInput
{
	public class InputAxisScrollbar : MonoBehaviour
	{		
		 [FormerlySerializedAs("Axis")] public string _axis;

		public void HandleInput(float value) => 
			CrossPlatformInputManager.SetAxis(_axis, value * 2f - 1f);
	}
}

using UnityEngine;
using UnityEngine.EventSystems;

namespace CodeBase._CrossPlatformInput
{
	public class AxisTouchButton : MonoBehaviour, IPointerDownHandler, IEventSystemHandler, IPointerUpHandler
	{
		public string axisName = "Horizontal";

		public float axisValue = 1f;

		public float responseSpeed = 3f;

		public float returnToCentreSpeed = 3f;

		private AxisTouchButton _pairedWith;

		private CrossPlatformInputManager.VirtualAxis _axis;
		
		
		private void OnEnable()
		{
			if (!CrossPlatformInputManager.AxisExists(axisName))
			{
				_axis = new CrossPlatformInputManager.VirtualAxis(axisName);
				CrossPlatformInputManager.RegisterVirtualAxis(_axis);
			}
			else
			{
				_axis = CrossPlatformInputManager.VirtualAxisReference(axisName);
			}
			FindPairedButton();
		}

		private void FindPairedButton()
		{
			AxisTouchButton[] array = FindObjectsOfType(typeof(AxisTouchButton)) as AxisTouchButton[];
			if (array != null)
			{
				for (int i = 0; i < array.Length; i++)
				{
					if (array[i].axisName == axisName && array[i] != this)
					{
						_pairedWith = array[i];
					}
				}
			}
		}

		private void OnDisable()
		{
			_axis.Remove();
		}

		public void OnPointerDown(PointerEventData data)
		{
			if (_pairedWith == null)
			{
				FindPairedButton();
			}
			_axis.Update(Mathf.MoveTowards(_axis.GetValue, axisValue, responseSpeed * Time.deltaTime));
		}

		public void OnPointerUp(PointerEventData data)
		{
			_axis.Update(Mathf.MoveTowards(_axis.GetValue, 0f, responseSpeed * Time.deltaTime));
		}
	}
}

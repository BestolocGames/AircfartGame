using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;

namespace CodeBase._CrossPlatformInput
{
	public class AxisTouchButton : MonoBehaviour, IPointerDownHandler, IEventSystemHandler, IPointerUpHandler
	{
		[FormerlySerializedAs("axisName")] public string _axisName = "Horizontal";

		[FormerlySerializedAs("axisValue")] public float _axisValue = 1f;

		[FormerlySerializedAs("responseSpeed")] public float _responseSpeed = 3f;

		[FormerlySerializedAs("returnToCentreSpeed")] public float _returnToCentreSpeed = 3f;

		private AxisTouchButton _pairedWith;

		private CrossPlatformInputManager.VirtualAxis _axis;
		
		
		private void OnEnable()
		{
			if (!CrossPlatformInputManager.AxisExists(_axisName))
			{
				_axis = new CrossPlatformInputManager.VirtualAxis(_axisName);
				CrossPlatformInputManager.RegisterVirtualAxis(_axis);
			}
			else
			{
				_axis = CrossPlatformInputManager.VirtualAxisReference(_axisName);
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
					if (array[i]._axisName == _axisName && array[i] != this)
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
			_axis.Update(Mathf.MoveTowards(_axis.GetValue, _axisValue, _responseSpeed * Time.deltaTime));
		}

		public void OnPointerUp(PointerEventData data)
		{
			_axis.Update(Mathf.MoveTowards(_axis.GetValue, 0f, _responseSpeed * Time.deltaTime));
		}
	}
}

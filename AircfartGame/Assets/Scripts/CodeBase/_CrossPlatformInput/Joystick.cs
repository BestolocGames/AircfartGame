using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;

namespace CodeBase._CrossPlatformInput
{
	public class Joystick : MonoBehaviour, IPointerDownHandler, IEventSystemHandler, IPointerUpHandler, IDragHandler
	{
		[FormerlySerializedAs("MovementRange")] public int _movementRange = 100;

		[FormerlySerializedAs("axesToUse")] public AxisOption _axesToUse;

		[FormerlySerializedAs("horizontalAxisName")] public string _horizontalAxisName = "Horizontal";

		[FormerlySerializedAs("verticalAxisName")] public string _verticalAxisName = "Vertical";

		private Vector3 _mStartPos;

		private bool _useHorizontal;

		private bool _useVertical;

		private CrossPlatformInputManager.VirtualAxis _horizontalVirtualAxis;

		private CrossPlatformInputManager.VirtualAxis _verticalVirtualAxis;

		public enum AxisOption
		{
			Both,
			OnlyHorizontal,
			OnlyVertical
		}
		
		
		private void OnEnable()
		{
			CreateVirtualAxes();
		}

		private void Start()
		{
			_mStartPos = transform.position;
		}

		private void UpdateVirtualAxes(Vector3 value)
		{
			Vector3 a = _mStartPos - value;
			a.y = -a.y;
			a /= (float)_movementRange;
			if (_useHorizontal) 
				_horizontalVirtualAxis.Update(-a.x);
			if (_useVertical) 
				_verticalVirtualAxis.Update(a.y);
		}

		private void CreateVirtualAxes()
		{
			_useHorizontal = (_axesToUse == AxisOption.Both || _axesToUse == AxisOption.OnlyHorizontal);
			_useVertical = (_axesToUse == AxisOption.Both || _axesToUse == AxisOption.OnlyVertical);
			
			if (_useHorizontal)
			{
				_horizontalVirtualAxis = new CrossPlatformInputManager.VirtualAxis(_horizontalAxisName);
				CrossPlatformInputManager.RegisterVirtualAxis(_horizontalVirtualAxis);
			}
			if (_useVertical)
			{
				_verticalVirtualAxis = new CrossPlatformInputManager.VirtualAxis(_verticalAxisName);
				CrossPlatformInputManager.RegisterVirtualAxis(_verticalVirtualAxis);
			}
		}

		public void OnDrag(PointerEventData data)
		{
			Vector3 zero = Vector3.zero;
			if (_useHorizontal)
			{
				int num = (int)(data.position.x - _mStartPos.x);
				num = Mathf.Clamp(num, -_movementRange, _movementRange);
				zero.x = (float)num;
			}
			if (_useVertical)
			{
				int num2 = (int)(data.position.y - _mStartPos.y);
				num2 = Mathf.Clamp(num2, -_movementRange, _movementRange);
				zero.y = (float)num2;
			}
			transform.position = new Vector3(_mStartPos.x + zero.x, _mStartPos.y + zero.y, _mStartPos.z + zero.z);
			UpdateVirtualAxes(transform.position);
		}

		public void OnPointerUp(PointerEventData data)
		{
			transform.position = _mStartPos;
			UpdateVirtualAxes(_mStartPos);
		}

		public void OnPointerDown(PointerEventData data)
		{
		}

		private void OnDisable()
		{
			if (_useHorizontal) 
				_horizontalVirtualAxis.Remove();
			if (_useVertical) 
				_verticalVirtualAxis.Remove();
		}
	}
}

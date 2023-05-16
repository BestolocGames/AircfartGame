using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace CodeBase._CrossPlatformInput
{
	[RequireComponent(typeof(Image))]
	public class TouchPad : MonoBehaviour, IPointerDownHandler, IEventSystemHandler, IPointerUpHandler
	{
		#region PublicFields

		[FormerlySerializedAs("axesToUse")] public AxisOption _axesToUse;

		[FormerlySerializedAs("controlStyle")] public ControlStyle _controlStyle;

		[FormerlySerializedAs("horizontalAxisName")] public string _horizontalAxisName = "Horizontal";

		[FormerlySerializedAs("verticalAxisName")] public string _verticalAxisName = "Vertical";

		[FormerlySerializedAs("Xsensitivity")] public float _xsensitivity = 1f;

		[FormerlySerializedAs("Ysensitivity")] public float _ysensitivity = 1f;

		#endregion

		#region PrivateFields

		private Vector3 _mStartPos;

		private Vector2 _mPreviousDelta;

		private Vector3 _mJoytickOutput;

		private bool _mUseX;

		private bool _mUseY;

		private CrossPlatformInputManager.VirtualAxis _mHorizontalVirtualAxis;

		private CrossPlatformInputManager.VirtualAxis _mVerticalVirtualAxis;

		private bool _mDragging;

		private int _mId = -1;

		private Vector2 _mPreviousTouchPos;

		private Vector3 _mCenter;

		private Image _mImage;

		#endregion


		public enum AxisOption
		{
			Both,
			OnlyHorizontal,
			OnlyVertical
		}

		public enum ControlStyle
		{
			Absolute,
			Relative,
			Swipe
		}
		
		private void OnEnable() => 
			CreateVirtualAxes();

		private void Start()
		{
			_mImage = GetComponent<Image>();
			_mCenter = _mImage.transform.position;
		}

		private void CreateVirtualAxes()
		{
			_mUseX = (_axesToUse == AxisOption.Both || _axesToUse == AxisOption.OnlyHorizontal);
			_mUseY = (_axesToUse == AxisOption.Both || _axesToUse == AxisOption.OnlyVertical);
			if (_mUseX)
			{
				_mHorizontalVirtualAxis = new CrossPlatformInputManager.VirtualAxis(_horizontalAxisName);
				CrossPlatformInputManager.RegisterVirtualAxis(_mHorizontalVirtualAxis);
			}
			if (_mUseY)
			{
				_mVerticalVirtualAxis = new CrossPlatformInputManager.VirtualAxis(_verticalAxisName);
				CrossPlatformInputManager.RegisterVirtualAxis(_mVerticalVirtualAxis);
			}
		}

		private void UpdateVirtualAxes(Vector3 value)
		{
			value = value.normalized;
			if (_mUseX) 
				_mHorizontalVirtualAxis.Update(value.x);
			if (_mUseY) 
				_mVerticalVirtualAxis.Update(value.y);
		}

		public void OnPointerDown(PointerEventData data)
		{
			_mDragging = true;
			_mId = data.pointerId;
			if (_controlStyle != ControlStyle.Absolute) 
				_mCenter = data.position;
		}

		private void Update()
		{
			if (!_mDragging)
				return;
			if (Input.touchCount >= _mId + 1 && _mId != -1)
			{
				if (_controlStyle == ControlStyle.Swipe)
				{
					_mCenter = _mPreviousTouchPos;
					_mPreviousTouchPos = Input.touches[_mId].position;
				}
				Vector2 vector = new Vector2(Input.touches[_mId].position.x - _mCenter.x, Input.touches[_mId].position.y - _mCenter.y);
				Vector2 normalized = vector.normalized;
				normalized.x *= _xsensitivity;
				normalized.y *= _ysensitivity;
				UpdateVirtualAxes(new Vector3(normalized.x, normalized.y, 0f));
			}
		}

		public void OnPointerUp(PointerEventData data)
		{
			_mDragging = false;
			_mId = -1;
			UpdateVirtualAxes(Vector3.zero);
		}

		private void OnDisable()
		{
			if (CrossPlatformInputManager.AxisExists(_horizontalAxisName))
				CrossPlatformInputManager.UnRegisterVirtualAxis(_horizontalAxisName);
			if (CrossPlatformInputManager.AxisExists(_verticalAxisName))
				CrossPlatformInputManager.UnRegisterVirtualAxis(_verticalAxisName);
		}
	}
}

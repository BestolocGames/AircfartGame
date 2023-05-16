using CodeBase._CrossPlatformInput;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace CodeBase._Main.Player
{
	public class AirplaneTouchControl : MonoBehaviour, IPointerDownHandler, IEventSystemHandler, IPointerUpHandler
	{
		#region Fields

		[FormerlySerializedAs("axesToUse")] public AxisOption _axesToUse;

		[FormerlySerializedAs("horizontalAxisName")] public string _horizontalAxisName = "Horizontal";

		[FormerlySerializedAs("verticalAxisName")] public string _verticalAxisName = "Vertical";

		[FormerlySerializedAs("Xsensitivity")] public float _xsensitivity = 1f;

		[FormerlySerializedAs("Ysensitivity")] public float _ysensitivity = 1f;

		[FormerlySerializedAs("baseImage")] [Header("Virtual joystick base to indicate steering center.")]
		public Image _baseImage;

		[FormerlySerializedAs("handleImage")] [Header("Virtual joystick handle to indicate steering direction.")]
		public Image _handleImage;

		[FormerlySerializedAs("maxHandleDistance")] [Header("How far can the handle move from the center of the base image.")]
		public float _maxHandleDistance = 100f;

		private Vector3 _mStartPos;

		private Vector2 _mPreviousDelta;

		private Vector3 _mJoytickOutput;

		private bool _mUseX;

		private bool _mUseY;

		private CrossPlatformInputManager.VirtualAxis _mHorizontalVirtualAxis;

		private CrossPlatformInputManager.VirtualAxis _mVerticalVirtualAxis;

		private bool _mDragging;

		private int _mId = -1;

		private Vector3 _mCenter;

		#endregion

		public enum AxisOption
		{
			Both,
			OnlyHorizontal,
			OnlyVertical
		}
		
		private void OnEnable() => 
			CreateVirtualAxes();

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
			if (_mUseX) 
				_mHorizontalVirtualAxis.Update(value.x);
			if (_mUseY) 
				_mVerticalVirtualAxis.Update(value.y);
		}

		private void Update()
		{
			if (!_mDragging)
			{
				return;
			}
			if (_mDragging)
			{
				Vector3 a = Input.touches[_mId].position;
				Vector3 vector = a - _mCenter;
				float d = Mathf.Min(vector.magnitude / (_maxHandleDistance + 0.01f), 1f);
				vector.Normalize();
				vector *= d;
				vector.x *= _xsensitivity;
				vector.y *= -1f * _ysensitivity;
				UpdateVirtualAxes(vector);
				Vector3 localPosition = a - _mCenter;
				if (localPosition.magnitude > _maxHandleDistance)
				{
					localPosition = localPosition.normalized * _maxHandleDistance;
				}
				_handleImage.transform.localPosition = localPosition;
			}
		}

		public void OnPointerDown(PointerEventData data)
		{
			_mDragging = true;
			_mId = data.pointerId;
			if (_baseImage != null)
			{
				_baseImage.enabled = true;
				_baseImage.transform.position = data.position;
				_handleImage.enabled = true;
				_handleImage.transform.position = _baseImage.transform.position;
			}
			_mCenter = data.position;
		}

		public void OnPointerUp(PointerEventData data)
		{
			_mDragging = false;
			_mId = -1;
			UpdateVirtualAxes(Vector3.zero);
			if (_baseImage != null)
			{
				_baseImage.enabled = false;
				_handleImage.enabled = false;
			}
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

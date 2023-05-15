// dnSpy decompiler from Assembly-CSharp.dll class: FlightKit.AirplaneTouchControl
using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityStandardAssets.CrossPlatformInput;

namespace FlightKit
{
	public class AirplaneTouchControl : MonoBehaviour, IPointerDownHandler, IEventSystemHandler, IPointerUpHandler
	{
		private void OnEnable()
		{
			this.CreateVirtualAxes();
		}

		private void CreateVirtualAxes()
		{
			this.m_UseX = (this.axesToUse == AirplaneTouchControl.AxisOption.Both || this.axesToUse == AirplaneTouchControl.AxisOption.OnlyHorizontal);
			this.m_UseY = (this.axesToUse == AirplaneTouchControl.AxisOption.Both || this.axesToUse == AirplaneTouchControl.AxisOption.OnlyVertical);
			if (this.m_UseX)
			{
				this.m_HorizontalVirtualAxis = new CrossPlatformInputManager.VirtualAxis(this.horizontalAxisName);
				CrossPlatformInputManager.RegisterVirtualAxis(this.m_HorizontalVirtualAxis);
			}
			if (this.m_UseY)
			{
				this.m_VerticalVirtualAxis = new CrossPlatformInputManager.VirtualAxis(this.verticalAxisName);
				CrossPlatformInputManager.RegisterVirtualAxis(this.m_VerticalVirtualAxis);
			}
		}

		private void UpdateVirtualAxes(Vector3 value)
		{
			if (this.m_UseX)
			{
				this.m_HorizontalVirtualAxis.Update(value.x);
			}
			if (this.m_UseY)
			{
				this.m_VerticalVirtualAxis.Update(value.y);
			}
		}

		private void Update()
		{
			if (!this.m_Dragging)
			{
				return;
			}
			if (this.m_Dragging)
			{
				Vector3 a = Input.touches[this.m_Id].position;
				Vector3 vector = a - this.m_Center;
				float d = Mathf.Min(vector.magnitude / (this.maxHandleDistance + 0.01f), 1f);
				vector.Normalize();
				vector *= d;
				vector.x *= this.Xsensitivity;
				vector.y *= -1f * this.Ysensitivity;
				this.UpdateVirtualAxes(vector);
				Vector3 localPosition = a - this.m_Center;
				if (localPosition.magnitude > this.maxHandleDistance)
				{
					localPosition = localPosition.normalized * this.maxHandleDistance;
				}
				this.handleImage.transform.localPosition = localPosition;
			}
		}

		public void OnPointerDown(PointerEventData data)
		{
			this.m_Dragging = true;
			this.m_Id = data.pointerId;
			if (this.baseImage != null)
			{
				this.baseImage.enabled = true;
				this.baseImage.transform.position = data.position;
				this.handleImage.enabled = true;
				this.handleImage.transform.position = this.baseImage.transform.position;
			}
			this.m_Center = data.position;
		}

		public void OnPointerUp(PointerEventData data)
		{
			this.m_Dragging = false;
			this.m_Id = -1;
			this.UpdateVirtualAxes(Vector3.zero);
			if (this.baseImage != null)
			{
				this.baseImage.enabled = false;
				this.handleImage.enabled = false;
			}
		}

		private void OnDisable()
		{
			if (CrossPlatformInputManager.AxisExists(this.horizontalAxisName))
			{
				CrossPlatformInputManager.UnRegisterVirtualAxis(this.horizontalAxisName);
			}
			if (CrossPlatformInputManager.AxisExists(this.verticalAxisName))
			{
				CrossPlatformInputManager.UnRegisterVirtualAxis(this.verticalAxisName);
			}
		}

		public AirplaneTouchControl.AxisOption axesToUse;

		public string horizontalAxisName = "Horizontal";

		public string verticalAxisName = "Vertical";

		public float Xsensitivity = 1f;

		public float Ysensitivity = 1f;

		[Header("Virtual joystick base to indicate steering center.")]
		public Image baseImage;

		[Header("Virtual joystick handle to indicate steering direction.")]
		public Image handleImage;

		[Header("How far can the handle move from the center of the base image.")]
		public float maxHandleDistance = 100f;

		private Vector3 m_StartPos;

		private Vector2 m_PreviousDelta;

		private Vector3 m_JoytickOutput;

		private bool m_UseX;

		private bool m_UseY;

		private CrossPlatformInputManager.VirtualAxis m_HorizontalVirtualAxis;

		private CrossPlatformInputManager.VirtualAxis m_VerticalVirtualAxis;

		private bool m_Dragging;

		private int m_Id = -1;

		private Vector3 m_Center;

		public enum AxisOption
		{
			Both,
			OnlyHorizontal,
			OnlyVertical
		}
	}
}

using CodeBase._CrossPlatformInput;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace CodeBase._Main.Player
{
	public class AirplaneTouchControl : MonoBehaviour, IPointerDownHandler, IEventSystemHandler, IPointerUpHandler
	{
		#region Fields

		public AxisOption axesToUse;

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
			m_UseX = (axesToUse == AxisOption.Both || axesToUse == AxisOption.OnlyHorizontal);
			m_UseY = (axesToUse == AxisOption.Both || axesToUse == AxisOption.OnlyVertical);
			if (m_UseX)
			{
				m_HorizontalVirtualAxis = new CrossPlatformInputManager.VirtualAxis(horizontalAxisName);
				CrossPlatformInputManager.RegisterVirtualAxis(m_HorizontalVirtualAxis);
			}
			if (m_UseY)
			{
				m_VerticalVirtualAxis = new CrossPlatformInputManager.VirtualAxis(verticalAxisName);
				CrossPlatformInputManager.RegisterVirtualAxis(m_VerticalVirtualAxis);
			}
		}

		private void UpdateVirtualAxes(Vector3 value)
		{
			if (m_UseX) 
				m_HorizontalVirtualAxis.Update(value.x);
			if (m_UseY) 
				m_VerticalVirtualAxis.Update(value.y);
		}

		private void Update()
		{
			if (!m_Dragging)
			{
				return;
			}
			if (m_Dragging)
			{
				Vector3 a = Input.touches[m_Id].position;
				Vector3 vector = a - m_Center;
				float d = Mathf.Min(vector.magnitude / (maxHandleDistance + 0.01f), 1f);
				vector.Normalize();
				vector *= d;
				vector.x *= Xsensitivity;
				vector.y *= -1f * Ysensitivity;
				UpdateVirtualAxes(vector);
				Vector3 localPosition = a - m_Center;
				if (localPosition.magnitude > maxHandleDistance)
				{
					localPosition = localPosition.normalized * maxHandleDistance;
				}
				handleImage.transform.localPosition = localPosition;
			}
		}

		public void OnPointerDown(PointerEventData data)
		{
			m_Dragging = true;
			m_Id = data.pointerId;
			if (baseImage != null)
			{
				baseImage.enabled = true;
				baseImage.transform.position = data.position;
				handleImage.enabled = true;
				handleImage.transform.position = baseImage.transform.position;
			}
			m_Center = data.position;
		}

		public void OnPointerUp(PointerEventData data)
		{
			m_Dragging = false;
			m_Id = -1;
			UpdateVirtualAxes(Vector3.zero);
			if (baseImage != null)
			{
				baseImage.enabled = false;
				handleImage.enabled = false;
			}
		}

		private void OnDisable()
		{
			if (CrossPlatformInputManager.AxisExists(horizontalAxisName))
				CrossPlatformInputManager.UnRegisterVirtualAxis(horizontalAxisName);
			if (CrossPlatformInputManager.AxisExists(verticalAxisName))
				CrossPlatformInputManager.UnRegisterVirtualAxis(verticalAxisName);
		}


	}
}

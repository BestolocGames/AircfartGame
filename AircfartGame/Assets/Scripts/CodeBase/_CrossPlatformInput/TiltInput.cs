using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace CodeBase._CrossPlatformInput
{
	public class TiltInput : MonoBehaviour
	{
		[FormerlySerializedAs("mapping")] public AxisMapping _mapping;

		[FormerlySerializedAs("tiltAroundAxis")] public AxisOptions _tiltAroundAxis;

		[FormerlySerializedAs("fullTiltAngle")] public float _fullTiltAngle = 25f;

		[FormerlySerializedAs("centreAngleOffset")] public float _centreAngleOffset;

		private CrossPlatformInputManager.VirtualAxis _mSteerAxis;

		public enum AxisOptions
		{
			ForwardAxis,
			SidewaysAxis
		}

		[Serializable]
		public class AxisMapping
		{
			[FormerlySerializedAs("type")] public MappingType _type;

			[FormerlySerializedAs("axisName")] public string _axisName;

			public enum MappingType
			{
				NamedAxis,
				MousePositionX,
				MousePositionY,
				MousePositionZ
			}
		}
		
		private void OnEnable()
		{
			if (_mapping._type == AxisMapping.MappingType.NamedAxis)
			{
				_mSteerAxis = new CrossPlatformInputManager.VirtualAxis(_mapping._axisName);
				CrossPlatformInputManager.RegisterVirtualAxis(_mSteerAxis);
			}
		}

		private void Update()
		{
			float value = 0f;
			if (Input.acceleration != Vector3.zero)
			{
				AxisOptions axisOptions = _tiltAroundAxis;
				if (axisOptions != AxisOptions.ForwardAxis)
				{
					if (axisOptions == AxisOptions.SidewaysAxis)
					{
						value = Mathf.Atan2(Input.acceleration.z, -Input.acceleration.y) * 57.29578f + _centreAngleOffset;
					}
				}
				else
				{
					value = Mathf.Atan2(Input.acceleration.x, -Input.acceleration.y) * 57.29578f + _centreAngleOffset;
				}
			}
			float num = Mathf.InverseLerp(-_fullTiltAngle, _fullTiltAngle, value) * 2f - 1f;
			switch (_mapping._type)
			{
			case AxisMapping.MappingType.NamedAxis:
				_mSteerAxis.Update(num);
				break;
			case AxisMapping.MappingType.MousePositionX:
				CrossPlatformInputManager.SetVirtualMousePositionX(num * (float)Screen.width);
				break;
			case AxisMapping.MappingType.MousePositionY:
				CrossPlatformInputManager.SetVirtualMousePositionY(num * (float)Screen.width);
				break;
			case AxisMapping.MappingType.MousePositionZ:
				CrossPlatformInputManager.SetVirtualMousePositionZ(num * (float)Screen.width);
				break;
			}
		}

		private void OnDisable()
		{
			CrossPlatformInputManager.UnRegisterVirtualAxis(_mapping._axisName);
			_mSteerAxis.Remove();
		}


	}
}

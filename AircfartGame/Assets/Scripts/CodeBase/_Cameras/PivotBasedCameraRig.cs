using UnityEngine;

namespace CodeBase._Cameras
{
	public abstract class PivotBasedCameraRig : AbstractTargetFollower
	{
		private Transform _cameraTransform;

		private Transform _pivot;

		protected Vector3 LastTargetPosition;
		
		protected virtual void Awake()
		{
			_cameraTransform = GetComponentInChildren<Camera>().transform;
			_pivot = _cameraTransform.parent;
		}
	}
}

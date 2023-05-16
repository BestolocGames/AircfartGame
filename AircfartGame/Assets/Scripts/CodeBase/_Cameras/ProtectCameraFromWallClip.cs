using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;

namespace CodeBase._Cameras
{
	public class ProtectCameraFromWallClip : MonoBehaviour
	{
		#region SerializedFiels

		[SerializeField] private float _clipMoveTime = 0.05f;

		[SerializeField]private float _returnTime = 0.4f;
		
		[SerializeField]private float _sphereCastRadius = 0.1f;

		[SerializeField]private bool _visualiseInEditor;

		[SerializeField] private float _closestDistance = 0.5f;

		[SerializeField] private string _dontClipTag = "Player";

		#endregion
		
		private Transform _caneraTransform;

		private Transform _pivot;

		private float _originalDistance;

		private float _moveVelocity;

		private float _currentDistance;

		private Ray _ray;

		private RaycastHit[] _hits;

		private RayHitComparer _rayHitComparer;
		
		public bool Protecting { get; private set; }

		private void Start()
		{
			_caneraTransform = GetComponentInChildren<Camera>().transform;
			_pivot = _caneraTransform.parent;
			_originalDistance = _caneraTransform.localPosition.magnitude;
			_currentDistance = _originalDistance;
			_rayHitComparer = new RayHitComparer();
		}

		private void LateUpdate()
		{
			float num = _originalDistance;
			_ray.origin = _pivot.position + _pivot.forward * _sphereCastRadius;
			_ray.direction = -_pivot.forward;
			Collider[] array = Physics.OverlapSphere(_ray.origin, _sphereCastRadius);
			bool flag = false;
			bool flag2 = false;
			for (int i = 0; i < array.Length; i++)
			{
				if (!array[i].isTrigger && (!(array[i].attachedRigidbody != null) || !array[i].attachedRigidbody.CompareTag(_dontClipTag)))
				{
					flag = true;
					break;
				}
			}
			if (flag)
			{
				_ray.origin = _ray.origin + _pivot.forward * _sphereCastRadius;
				_hits = Physics.RaycastAll(_ray, _originalDistance - _sphereCastRadius);
			}
			else
				_hits = Physics.SphereCastAll(_ray, _sphereCastRadius, _originalDistance + _sphereCastRadius);

			Array.Sort(_hits, _rayHitComparer);
			float num2 = float.PositiveInfinity;
			for (int j = 0; j < _hits.Length; j++)
			{
				if (_hits[j].distance < num2 && !_hits[j].collider.isTrigger && (!(_hits[j].collider.attachedRigidbody != null) || !_hits[j].collider.attachedRigidbody.CompareTag(_dontClipTag)))
				{
					num2 = _hits[j].distance;
					num = -_pivot.InverseTransformPoint(_hits[j].point).z;
					flag2 = true;
				}
			}
			if (flag2) 
				Debug.DrawRay(_ray.origin, -_pivot.forward * (num + _sphereCastRadius), Color.red);
			Protecting = flag2;
			
			_currentDistance = Mathf.SmoothDamp(_currentDistance, num, ref _moveVelocity, (_currentDistance <= num) ? _returnTime : _clipMoveTime);
			_currentDistance = Mathf.Clamp(_currentDistance, _closestDistance, _originalDistance);
			_caneraTransform.localPosition = -Vector3.forward * _currentDistance;
		}



		public class RayHitComparer : IComparer
		{
			public int Compare(object x, object y) => 
				((RaycastHit)x).distance.CompareTo(((RaycastHit)y).distance);
		}
	}
}

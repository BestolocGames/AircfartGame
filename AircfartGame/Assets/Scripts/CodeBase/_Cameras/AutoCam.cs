using UnityEngine;
using UnityEngine.Serialization;

namespace CodeBase._Cameras
{
	[ExecuteInEditMode]
	public class AutoCam : PivotBasedCameraRig
	{
		#region SerializedFields

		[SerializeField]
		private float _moveSpeed = 3f;

		[SerializeField]
		private float _turnSpeed = 1f;

		[SerializeField]
		private float _rollSpeed = 0.2f;

		 [SerializeField]
		private bool _followVelocity;

		[SerializeField]
		private bool _followTilt = true;

		 [SerializeField]
		private float _spinTurnLimit = 90f;

		 [SerializeField]
		private float _targetVelocityLowerLimit = 4f;

		[SerializeField]
		private float _smoothTurnTime = 0.2f;

		#endregion

		private float _lastFlatAngle;

		private float _currentTurnAmount;

		private float _turnSpeedVelocityChange;

		private Vector3 _rollUp = Vector3.up;
		
		public float TurnSpeed
		{
			get => _turnSpeed;
			set => _turnSpeed = value;
		}

		protected override void FollowTarget(float deltaTime)
		{
			if (deltaTime <= 0f || _target == null)
				return;
			
			Vector3 forward = _target.forward;
			Vector3 up = _target.up;
			if (_followVelocity && Application.isPlaying)
			{
				if (TargetRigidbody.velocity.magnitude > _targetVelocityLowerLimit)
				{
					forward = TargetRigidbody.velocity.normalized;
					up = Vector3.up;
				}
				else
					up = Vector3.up;

				_currentTurnAmount = Mathf.SmoothDamp(_currentTurnAmount, 1f, ref _turnSpeedVelocityChange, _smoothTurnTime);
			}
			else
			{
				float num = Mathf.Atan2(forward.x, forward.z) * 57.29578f;
				if (_spinTurnLimit > 0f)
				{
					float value = Mathf.Abs(Mathf.DeltaAngle(_lastFlatAngle, num)) / deltaTime;
					float num2 = Mathf.InverseLerp(_spinTurnLimit, _spinTurnLimit * 0.75f, value);
					float smoothTime = (_currentTurnAmount <= num2) ? 1f : 0.1f;
					if (Application.isPlaying)
						_currentTurnAmount = Mathf.SmoothDamp(_currentTurnAmount, num2, ref _turnSpeedVelocityChange,
							smoothTime);
					else
						_currentTurnAmount = num2;
				}
				else
					_currentTurnAmount = 1f;

				_lastFlatAngle = num;
			}
			transform.position = Vector3.Lerp(transform.position, _target.position, deltaTime * _moveSpeed);
			
			if (!_followTilt)
			{
				forward.y = 0f;
				if (forward.sqrMagnitude < 1.401298E-45f) 
					forward = transform.forward;
			}
			
			Quaternion b = Quaternion.LookRotation(forward, _rollUp);
			_rollUp = ((_rollSpeed <= 0f) ? Vector3.up : Vector3.Slerp(_rollUp, up, _rollSpeed * deltaTime));
			transform.rotation = Quaternion.Lerp(transform.rotation, b, _turnSpeed * _currentTurnAmount * deltaTime);
		}
	}
}

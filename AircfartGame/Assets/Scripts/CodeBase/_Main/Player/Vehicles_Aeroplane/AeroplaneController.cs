using UnityEngine;
using UnityEngine.Serialization;

namespace CodeBase._Main.Player.Vehicles_Aeroplane
{
	[RequireComponent(typeof(Rigidbody))]
	public class AeroplaneController : MonoBehaviour
	{
		#region SerializedFields

		 [SerializeField]
		private float _maxEnginePower = 40f;

		 [SerializeField]
		private float _lift = 0.002f;

		 [SerializeField]
		private float _zeroLiftSpeed = 300f;

		[SerializeField]
		private float _rollEffect = 1f;

		[SerializeField]
		private float _pitchEffect = 1f;

		[SerializeField]
		private float _yawEffect = 0.2f;

		 [SerializeField]
		private float _bankedTurnEffect = 0.5f;

		 [SerializeField]
		private float _aerodynamicEffect = 0.02f;

		[SerializeField]
		private float _autoTurnPitch = 0.5f;

		[SerializeField]
		private float _autoRollLevel = 0.2f;

		[SerializeField]
		private float _autoPitchLevel = 0.2f;

		[SerializeField]
		private float _airBrakesEffect = 3f;

		 [SerializeField]
		private float _throttleChangeSpeed = 0.3f;

		  [SerializeField]
		private float _dragIncreaseFactor = 0.001f;

		 [FormerlySerializedAs("_mMaxSpeed")] [SerializeField]
		private float _maxSpeed = 10f;

		#endregion

		#region PrivateFiels

		private float _originalDrag;

		private float _originalAngularDrag;

		private float _aeroFactor;

		private bool _immobilized;

		private float _bankedTurnAmount;

		private Rigidbody _rigidbody;

		private WheelCollider[] _wheelColliders;

		#endregion

		#region Properties

		public float Altitude { get; private set; }

		public float Throttle { get; private set; }

		public bool AirBrakes { get; private set; }

		public float ForwardSpeed { get; private set; }

		public float EnginePower { get; private set; }

		public float MaxEnginePower
			=> _maxEnginePower;

		public float RollAngle { get; private set; }

		public float PitchAngle { get; private set; }

		public float RollInput { get; private set; }

		public float PitchInput { get; private set; }

		public float YawInput { get; private set; }

		public float ThrottleInput { get; private set; }

		public float MaxSpeed
			=> _maxSpeed;

		public float AerodynamicEffect
		{
			get => 
				_aerodynamicEffect;
			set => 
				_aerodynamicEffect = value;
		}


		#endregion
		
		private void Start()
		{
			_rigidbody = GetComponent<Rigidbody>();
			_originalDrag = _rigidbody.drag;
			_originalAngularDrag = _rigidbody.angularDrag;
			for (int i = 0; i < transform.childCount; i++)
			{
				foreach (WheelCollider wheelCollider in transform.GetChild(i).GetComponentsInChildren<WheelCollider>())
				{
					wheelCollider.motorTorque = 0.18f;
				}
			}
		}

		public void Move(float rollInput, float pitchInput, float yawInput, float throttleInput, bool airBrakes)
		{
			RollInput = rollInput;
			PitchInput = pitchInput;
			YawInput = yawInput;
			ThrottleInput = throttleInput;
			AirBrakes = airBrakes;
			ClampInputs();
			CalculateRollAndPitchAngles();
			AutoLevel();
			CalculateForwardSpeed();
			ControlThrottle();
			CalculateDrag();
			CaluclateAerodynamicEffect();
			CalculateLinearForces();
			CalculateTorque();
			CalculateAltitude();
			LimitVelocity();
		}

		private void LimitVelocity()
		{
			if (_rigidbody.velocity.sqrMagnitude > _maxSpeed * _maxSpeed)
			{
				_rigidbody.velocity = _rigidbody.velocity.normalized * _maxSpeed;
			}
		}

		private void ClampInputs()
		{
			RollInput = Mathf.Clamp(RollInput, -1f, 1f);
			PitchInput = Mathf.Clamp(PitchInput, -1f, 1f);
			YawInput = Mathf.Clamp(YawInput, -1f, 1f);
			ThrottleInput = Mathf.Clamp(ThrottleInput, -1f, 1f);
		}

		private void CalculateRollAndPitchAngles()
		{
			Vector3 forward = transform.forward;
			forward.y = 0f;
			if (forward.sqrMagnitude > 0f)
			{
				forward.Normalize();
				Vector3 vector = transform.InverseTransformDirection(forward);
				PitchAngle = Mathf.Atan2(vector.y, vector.z);
				Vector3 direction = Vector3.Cross(Vector3.up, forward);
				Vector3 vector2 = transform.InverseTransformDirection(direction);
				RollAngle = Mathf.Atan2(vector2.y, vector2.x);
			}
		}

		private void AutoLevel()
		{
			_bankedTurnAmount = Mathf.Sin(RollAngle);
			if (RollInput == 0f)
			{
				RollInput = -RollAngle * _autoRollLevel;
			}
			if (PitchInput == 0f)
			{
				PitchInput = -PitchAngle * _autoPitchLevel;
				PitchInput -= Mathf.Abs(_bankedTurnAmount * _bankedTurnAmount * _autoTurnPitch);
			}
		}

		private void CalculateForwardSpeed()
		{
			ForwardSpeed = Mathf.Max(0f, transform.InverseTransformDirection(_rigidbody.velocity).z);
		}

		private void ControlThrottle()
		{
			if (_immobilized)
			{
				ThrottleInput = -0.5f;
			}
			Throttle = Mathf.Clamp01(Throttle + ThrottleInput * Time.deltaTime * _throttleChangeSpeed);
			EnginePower = Throttle * _maxEnginePower;
		}

		private void CalculateDrag()
		{
			float num = _rigidbody.velocity.magnitude * _dragIncreaseFactor;
			_rigidbody.drag = ((!AirBrakes) ? (_originalDrag + num) : ((_originalDrag + num) * _airBrakesEffect));
			_rigidbody.angularDrag = _originalAngularDrag * ForwardSpeed;
		}

		private void CaluclateAerodynamicEffect()
		{
			if (_rigidbody.velocity.magnitude > 0f)
			{
				_aeroFactor = Vector3.Dot(transform.forward, _rigidbody.velocity.normalized);
				_aeroFactor *= _aeroFactor;
				Vector3 velocity = Vector3.Lerp(_rigidbody.velocity, transform.forward * ForwardSpeed, _aeroFactor * ForwardSpeed * _aerodynamicEffect * Time.deltaTime);
				_rigidbody.velocity = velocity;
				if ((double)_rigidbody.velocity.sqrMagnitude > (double)(MaxSpeed * MaxSpeed) * 0.01)
				{
					Quaternion b = Quaternion.LookRotation(_rigidbody.velocity, transform.up);
					_rigidbody.rotation = Quaternion.Slerp(_rigidbody.rotation, b, _aerodynamicEffect * Time.deltaTime);
				}
			}
		}

		private void CalculateLinearForces()
		{
			Vector3 vector = Vector3.zero;
			vector += EnginePower * transform.forward;
			Vector3 normalized = Vector3.Cross(_rigidbody.velocity, transform.right).normalized;
			float num = Mathf.InverseLerp(_zeroLiftSpeed, 0f, ForwardSpeed);
			float d = ForwardSpeed * ForwardSpeed * _lift * num * _aeroFactor;
			vector += d * normalized;
			_rigidbody.AddForce(vector);
		}

		private void CalculateTorque()
		{
			Vector3 a = Vector3.zero;
			a += PitchInput * _pitchEffect * transform.right;
			a += YawInput * _yawEffect * transform.up;
			a += -RollInput * _rollEffect * transform.forward;
			a += _bankedTurnAmount * _bankedTurnEffect * transform.up;
			_rigidbody.AddTorque(a * ForwardSpeed * _aeroFactor);
		}

		private void CalculateAltitude()
		{
			Ray ray = new Ray(transform.position - Vector3.up * 10f, -Vector3.up);
			RaycastHit raycastHit;
			Altitude = ((!Physics.Raycast(ray, out raycastHit)) ? transform.position.y : (raycastHit.distance + 10f));
		}

		public void Immobilize() => 
			_immobilized = true;

		public void Reset() => 
			_immobilized = false;
	}
}

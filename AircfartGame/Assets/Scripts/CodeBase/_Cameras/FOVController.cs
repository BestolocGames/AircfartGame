using CodeBase._Main;
using CodeBase._Main.Player;
using CodeBase._Main.Player.Vehicles_Aeroplane;
using UnityEngine;

namespace CodeBase._Cameras
{
	public class FOVController : MonoBehaviour
	{		
		public float maxFovChangeFactor = 1.2f;

		private GameObject _airplane;

		private Camera _mainCamera;

		private Rigidbody _airplaneRigidBody;

		private float _baseFov;

		private float _maxFovIncrease;

		private float _maxSpeedSqr;
		private void Start()
		{
			AirplaneUserControl airplaneUserControl = FindObjectOfType<AirplaneUserControl>();
			if (airplaneUserControl == null)
			{
				Debug.LogError("FLIGHT KIT StartLevelSequence: an AeroplaneUserControl component is missing in the scene");
				return;
			}
			_airplane = airplaneUserControl.gameObject;
			_mainCamera = Camera.main;
			_baseFov = _mainCamera.fieldOfView;
			_maxFovIncrease = _baseFov * (maxFovChangeFactor - 1f);
			_airplaneRigidBody = _airplane.GetComponent<Rigidbody>();
			_maxSpeedSqr = _airplane.GetComponent<AeroplaneController>().MaxSpeed;
			_maxSpeedSqr *= _maxSpeedSqr;
		}

		private void FixedUpdate()
		{
			_mainCamera.fieldOfView = _baseFov + _airplaneRigidBody.velocity.sqrMagnitude / _maxSpeedSqr * _maxFovIncrease;
		}


	}
}

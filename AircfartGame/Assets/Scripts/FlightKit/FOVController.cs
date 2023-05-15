// dnSpy decompiler from Assembly-CSharp.dll class: FlightKit.FOVController
using System;
using UnityEngine;
using UnityStandardAssets.Vehicles.Aeroplane;

namespace FlightKit
{
	public class FOVController : MonoBehaviour
	{
		private void Start()
		{
			AirplaneUserControl airplaneUserControl = UnityEngine.Object.FindObjectOfType<AirplaneUserControl>();
			if (airplaneUserControl == null)
			{
				UnityEngine.Debug.LogError("FLIGHT KIT StartLevelSequence: an AeroplaneUserControl component is missing in the scene");
				return;
			}
			this._airplane = airplaneUserControl.gameObject;
			this._mainCamera = Camera.main;
			this._baseFov = this._mainCamera.fieldOfView;
			this._maxFovIncrease = this._baseFov * (this.maxFovChangeFactor - 1f);
			this._airplaneRigidBody = this._airplane.GetComponent<Rigidbody>();
			this._maxSpeedSqr = this._airplane.GetComponent<AeroplaneController>().MaxSpeed;
			this._maxSpeedSqr *= this._maxSpeedSqr;
		}

		private void FixedUpdate()
		{
			this._mainCamera.fieldOfView = this._baseFov + this._airplaneRigidBody.velocity.sqrMagnitude / this._maxSpeedSqr * this._maxFovIncrease;
		}

		public float maxFovChangeFactor = 1.2f;

		private GameObject _airplane;

		private Camera _mainCamera;

		private Rigidbody _airplaneRigidBody;

		private float _baseFov;

		private float _maxFovIncrease;

		private float _maxSpeedSqr;
	}
}

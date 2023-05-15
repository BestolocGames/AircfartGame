// dnSpy decompiler from Assembly-CSharp.dll class: FlightKit.SimplePropellerAnimator
using System;
using UnityEngine;
using UnityStandardAssets.Vehicles.Aeroplane;

namespace FlightKit
{
	[RequireComponent(typeof(AeroplaneController))]
	public class SimplePropellerAnimator : MonoBehaviour
	{
		private void Awake()
		{
			this._airplane = base.GetComponent<AeroplaneController>();
		}

		private void Update()
		{
			if (!this._airplane || !this.propellerModel)
			{
				return;
			}
			float num = this.maxRpm * this._airplane.Throttle * Time.deltaTime * 60f;
			if (this.rotateAroundX)
			{
				this.propellerModel.Rotate(num, 0f, 0f);
			}
			else
			{
				this.propellerModel.Rotate(0f, num, 0f);
			}
		}

		private const float RPM_TO_DPS = 60f;

		public Transform propellerModel;

		public float maxRpm = 2000f;

		public bool rotateAroundX;

		private AeroplaneController _airplane;

		private Renderer _propellerModelRenderer;
	}
}

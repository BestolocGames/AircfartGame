// dnSpy decompiler from Assembly-CSharp.dll class: FlightKit.FuelController
using System;
using UnityEngine;

namespace FlightKit
{
	public class FuelController : MonoBehaviour
	{
		public static event GameActions.SimpleAction OnFuelLowEvent;

		public static event GameActions.SimpleAction OnFuelEmptyEvent;

		public float fuelAmount { get; protected set; }

		private void Start()
		{
			this.fuelAmount = 1f;
		}

		private void OnEnable()
		{
			PickupSphere.OnCollectEvent += this.HandlePickupCollected;
			TakeOffPublisher.OnTakeOffEvent += this.HandleTakeOff;
			RevivePermissionProvider.OnReviveGranted += this.HandleRevive;
		}

		private void OnDisable()
		{
			PickupSphere.OnCollectEvent -= this.HandlePickupCollected;
			TakeOffPublisher.OnTakeOffEvent -= this.HandleTakeOff;
			RevivePermissionProvider.OnReviveGranted -= this.HandleRevive;
		}

		private void HandlePickupCollected()
		{
			this.fuelAmount += this.pickupFuelAmount;
			if (this.fuelAmount >= 0.25f)
			{
				this._lowFuelRegistered = false;
			}
		}

		private void HandleTakeOff()
		{
			this._isConsuming = true;
		}

		private void HandleRevive()
		{
			this._isConsuming = true;
			this.fuelAmount = this.reviveFuelAmount;
			if (this.fuelAmount > 0.25f)
			{
				this._lowFuelRegistered = false;
			}
			base.gameObject.SetActive(true);
		}

		private void Update()
		{
			if (this._isConsuming)
			{
				this.fuelAmount -= this.consumptionRate * Time.deltaTime * 0.01f;
				if (!this._lowFuelRegistered && this.fuelAmount < 0.25f)
				{
					this._lowFuelRegistered = true;
					if (FuelController.OnFuelLowEvent != null)
					{
						FuelController.OnFuelLowEvent();
					}
				}
			}
			if (this.fuelAmount <= 0f && this._isConsuming && FuelController.OnFuelEmptyEvent != null)
			{
				this._isConsuming = false;
				FuelController.OnFuelEmptyEvent();
			}
		}

		private const float LOW_FUEL_PERCENT = 0.25f;

		[Header("How fast the fuel is used. Higher number - harder gameplay.")]
		public float consumptionRate = 1f;

		[Header("How much fuel is added by each pickup. Higher number - easier gameplay.")]
		public float pickupFuelAmount = 0.25f;

		[Header("Amount of fuel user receives on reviving.")]
		public float reviveFuelAmount = 0.5f;

		private bool _isConsuming;

		private bool _lowFuelRegistered;
	}
}

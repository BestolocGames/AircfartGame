using UnityEngine;

namespace CodeBase._Main
{
	public class FuelController : MonoBehaviour
	{
		private const float LOW_FUEL_PERCENT = 0.25f;

		[Header("How fast the fuel is used. Higher number - harder gameplay.")]
		public float consumptionRate = 1f;

		[Header("How much fuel is added by each pickup. Higher number - easier gameplay.")]
		public float pickupFuelAmount = 0.25f;

		[Header("Amount of fuel user receives on reviving.")]
		public float reviveFuelAmount = 0.5f;

		private bool _isConsuming;

		private bool _lowFuelRegistered;
		
		
		public static event GameActions.SimpleAction OnFuelLowEvent;

		public static event GameActions.SimpleAction OnFuelEmptyEvent;

		public float fuelAmount { get; protected set; }

		private void Start()
		{
			fuelAmount = 1f;
		}

		private void OnEnable()
		{
			PickupSphere.OnCollectEvent += HandlePickupCollected;
			TakeOffPublisher.OnTakeOffEvent += HandleTakeOff;
			RevivePermissionProvider.OnReviveGranted += HandleRevive;
		}

		private void OnDisable()
		{
			PickupSphere.OnCollectEvent -= HandlePickupCollected;
			TakeOffPublisher.OnTakeOffEvent -= HandleTakeOff;
			RevivePermissionProvider.OnReviveGranted -= HandleRevive;
		}

		private void HandlePickupCollected()
		{
			fuelAmount += pickupFuelAmount;
			if (fuelAmount >= 0.25f)
			{
				_lowFuelRegistered = false;
			}
		}

		private void HandleTakeOff()
		{
			_isConsuming = true;
		}

		private void HandleRevive()
		{
			_isConsuming = true;
			fuelAmount = reviveFuelAmount;
			if (fuelAmount > 0.25f)
			{
				_lowFuelRegistered = false;
			}
			gameObject.SetActive(true);
		}

		private void Update()
		{
			if (_isConsuming)
			{
				fuelAmount -= consumptionRate * Time.deltaTime * 0.01f;
				if (!_lowFuelRegistered && fuelAmount < 0.25f)
				{
					_lowFuelRegistered = true;
					if (OnFuelLowEvent != null)
					{
						OnFuelLowEvent();
					}
				}
			}
			if (fuelAmount <= 0f && _isConsuming && OnFuelEmptyEvent != null)
			{
				_isConsuming = false;
				OnFuelEmptyEvent();
			}
		}


	}
}

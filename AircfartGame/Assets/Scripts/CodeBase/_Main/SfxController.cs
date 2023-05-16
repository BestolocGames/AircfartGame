using UnityEngine;

namespace CodeBase._Main
{
	public class SfxController : MonoBehaviour
	{
		private void OnEnable()
		{
			if (audioSource == null)
			{
				enabled = false;
				return;
			}
			FuelController.OnFuelLowEvent += HandleFuelLow;
			FuelController.OnFuelEmptyEvent += HandleFuelEmpty;
			RevivePermissionProvider.OnReviveGranted += HandleRevive;
		}

		private void OnDisable()
		{
			FuelController.OnFuelLowEvent -= HandleFuelLow;
			FuelController.OnFuelEmptyEvent -= HandleFuelEmpty;
			RevivePermissionProvider.OnReviveGranted -= HandleRevive;
		}

		private void HandleFuelLow()
		{
			if (lowFuelSound != null)
			{
				audioSource.PlayOneShot(lowFuelSound);
			}
		}

		private void HandleFuelEmpty()
		{
			if (levelFailSound != null)
			{
				audioSource.PlayOneShot(levelFailSound);
			}
		}

		private void HandleRevive()
		{
			if (userRevivedSound != null)
			{
				audioSource.PlayOneShot(userRevivedSound);
			}
		}

		[Tooltip("2D audio source to play sound effects on.")]
		public AudioSource audioSource;

		[Space]
		[Tooltip("The sound that is played when user's fuel gets low, but not completely gone yet.")]
		public AudioClip lowFuelSound;

		[Tooltip("The sound that is played when a level is failed.")]
		public AudioClip levelFailSound;

		[Tooltip("The sound that is played when user is revived after failing.")]
		public AudioClip userRevivedSound;
	}
}

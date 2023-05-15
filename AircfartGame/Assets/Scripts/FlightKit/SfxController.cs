// dnSpy decompiler from Assembly-CSharp.dll class: FlightKit.SfxController
using System;
using UnityEngine;

namespace FlightKit
{
	public class SfxController : MonoBehaviour
	{
		private void OnEnable()
		{
			if (this.audioSource == null)
			{
				base.enabled = false;
				return;
			}
			FuelController.OnFuelLowEvent += this.HandleFuelLow;
			FuelController.OnFuelEmptyEvent += this.HandleFuelEmpty;
			RevivePermissionProvider.OnReviveGranted += this.HandleRevive;
		}

		private void OnDisable()
		{
			FuelController.OnFuelLowEvent -= this.HandleFuelLow;
			FuelController.OnFuelEmptyEvent -= this.HandleFuelEmpty;
			RevivePermissionProvider.OnReviveGranted -= this.HandleRevive;
		}

		private void HandleFuelLow()
		{
			if (this.lowFuelSound != null)
			{
				this.audioSource.PlayOneShot(this.lowFuelSound);
			}
		}

		private void HandleFuelEmpty()
		{
			if (this.levelFailSound != null)
			{
				this.audioSource.PlayOneShot(this.levelFailSound);
			}
		}

		private void HandleRevive()
		{
			if (this.userRevivedSound != null)
			{
				this.audioSource.PlayOneShot(this.userRevivedSound);
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

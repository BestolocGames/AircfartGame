using UnityEngine;
using UnityEngine.Serialization;

namespace CodeBase._Main
{
	public class SfxController : MonoBehaviour
	{
		[FormerlySerializedAs("audioSource")] [Tooltip("2D audio source to play sound effects on.")]
		public AudioSource _audioSource;

		[FormerlySerializedAs("lowFuelSound")]
		[Space]
		[Tooltip("The sound that is played when user's fuel gets low, but not completely gone yet.")]
		public AudioClip _lowFuelSound;

		[FormerlySerializedAs("levelFailSound")] [Tooltip("The sound that is played when a level is failed.")]
		public AudioClip _levelFailSound;

		[FormerlySerializedAs("userRevivedSound")] [Tooltip("The sound that is played when user is revived after failing.")]
		public AudioClip _userRevivedSound;
		
		private void OnEnable()
		{
			if (_audioSource == null)
			{
				enabled = false;
				return;
			}
			RevivePermissionProvider.OnReviveGranted += HandleRevive;
		}

		private void OnDisable() => 
			RevivePermissionProvider.OnReviveGranted -= HandleRevive;

		private void HandleFuelLow()
		{
			if (_lowFuelSound != null) 
				_audioSource.PlayOneShot(_lowFuelSound);
		}

		private void HandleFuelEmpty()
		{
			if (_levelFailSound != null) 
				_audioSource.PlayOneShot(_levelFailSound);
		}

		private void HandleRevive()
		{
			if (_userRevivedSound != null) 
				_audioSource.PlayOneShot(_userRevivedSound);
		}


	}
}

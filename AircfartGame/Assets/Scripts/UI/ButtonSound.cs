using UnityEngine;

namespace UI
{
	public class ButtonSound : MonoBehaviour
	{
		private void Start()
		{
			GameObject gameObject = GameObject.Find(audioSourceContainerName);
			if (gameObject != null)
			{
				_audioSource = gameObject.GetComponent<AudioSource>();
			}
		}

		public virtual void PlaySound()
		{
			if (_audioSource != null)
			{
				_audioSource.PlayOneShot(sound);
			}
		}

		public virtual void PlaySound(bool activated)
		{
			if (activated)
			{
				PlaySound();
			}
		}

		public string audioSourceContainerName = "SoundFX";

		public AudioClip sound;

		private AudioSource _audioSource;
	}
}

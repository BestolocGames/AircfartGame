using UnityEngine;
using UnityEngine.Serialization;

namespace UI
{
	public class ButtonSound : MonoBehaviour
	{
		[FormerlySerializedAs("audioSourceContainerName")] public string _audioSourceContainerName = "SoundFX";

		[FormerlySerializedAs("sound")] public AudioClip _sound;

		private AudioSource _audioSource;
		
		private void Start()
		{
			GameObject gameObject = GameObject.Find(_audioSourceContainerName);
			if (gameObject != null) 
				_audioSource = gameObject.GetComponent<AudioSource>();
		}

		public virtual void PlaySound()
		{
			if (_audioSource != null) 
				_audioSource.PlayOneShot(_sound);
		}

		public virtual void PlaySound(bool activated)
		{
			if (activated) 
				PlaySound();
		}
	}
}

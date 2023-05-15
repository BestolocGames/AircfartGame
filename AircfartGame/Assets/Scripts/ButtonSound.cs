// dnSpy decompiler from Assembly-CSharp.dll class: ButtonSound
using System;
using UnityEngine;

public class ButtonSound : MonoBehaviour
{
	private void Start()
	{
		GameObject gameObject = GameObject.Find(this.audioSourceContainerName);
		if (gameObject != null)
		{
			this._audioSource = gameObject.GetComponent<AudioSource>();
		}
	}

	public virtual void PlaySound()
	{
		if (this._audioSource != null)
		{
			this._audioSource.PlayOneShot(this.sound);
		}
	}

	public virtual void PlaySound(bool activated)
	{
		if (activated)
		{
			this.PlaySound();
		}
	}

	public string audioSourceContainerName = "SoundFX";

	public AudioClip sound;

	private AudioSource _audioSource;
}

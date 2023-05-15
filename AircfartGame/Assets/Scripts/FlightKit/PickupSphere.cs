// dnSpy decompiler from Assembly-CSharp.dll class: FlightKit.PickupSphere
using System;
using UnityEngine;
using UnityEngine.Events;
using UnityStandardAssets.ImageEffects;

namespace FlightKit
{
	public class PickupSphere : MonoBehaviour
	{
		public static event PickupSphere.OnCollectAction OnCollectEvent;
		[HideInInspector]
		public UnityEvent OnCollect;

		private void Start()
		{
			this._bloom = UnityEngine.Object.FindObjectOfType<BloomOptimized>();
			if (this._bloom != null)
			{
				this._bloomInitValue = this._bloom.intensity;
			}
			base.transform.localRotation = UnityEngine.Random.rotation;
		}

		private void Update()
		{
			if (this._isDestroyed)
			{
				return;
			}
			this.ring1.transform.Rotate(Vector3.right, this.ringRotationSpeed * Time.deltaTime);
			this.ring2.transform.Rotate(Vector3.up, this.ringRotationSpeed * Time.deltaTime);
			if (PickupSphere.growingEnabled && base.transform.localScale.x < this.maxScale)
			{
				base.transform.localScale += Vector3.one * this.growthSpeed * Time.deltaTime;
			}
			if (this._isTweeningOut)
			{
				base.transform.localScale = Vector3.Lerp(base.transform.localScale, Vector3.zero, 5f * Time.deltaTime);
				if (this._bloom != null)
				{
					this._bloom.intensity = Mathf.Lerp(this._bloom.intensity, this._bloomInitValue, 5f * Time.deltaTime);
				}
			}
		}

		private void OnTriggerEnter(Collider collider)
		{
			if (this._activated)
			{
				return;
			}
			if (collider.gameObject.CompareTag(Tags.Player))
			{
				this._activated = true;
				if (PickupSphere.OnCollectEvent != null)
				{
					PickupSphere.OnCollectEvent();
				}
				AudioSource componentInParent = base.GetComponentInParent<AudioSource>();
				if (componentInParent != null)
				{
					componentInParent.Play();
				}
				this._isTweeningOut = true;
				if (this._bloom != null)
				{
					this._bloom.intensity = 0.5f;
				}
				BoidMaster componentInParent2 = base.GetComponentInParent<BoidMaster>();
				if (componentInParent2 != null)
				{
					componentInParent2.neighborDistance = 250f;
				}
				base.Invoke("DestroyNow", 1f);
			}
		}

		public void TweenBloom(float value)
		{
			this._bloom.intensity = value;
		}

		private void DestroyNow()
		{
			this._bloom.intensity = this._bloomInitValue;
			UnityEngine.Object.DestroyObject(this.sphere);
			UnityEngine.Object.DestroyObject(this.ring1);
			UnityEngine.Object.DestroyObject(this.ring2);
			this._isDestroyed = true;
			OnCollect.Invoke();
		}

		public static bool growingEnabled;

		public float growthSpeed = 0.1f;

		public float maxScale = 40f;

		public float ringRotationSpeed = 150f;

		public GameObject ring1;

		public GameObject ring2;

		public GameObject sphere;

		private bool _activated;

		private BloomOptimized _bloom;

		private float _bloomInitValue;

		private bool _isTweeningOut;

		private bool _isDestroyed;

		public delegate void OnCollectAction();
	}
}

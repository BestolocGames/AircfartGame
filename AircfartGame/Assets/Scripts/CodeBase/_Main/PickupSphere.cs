using CodeBase._ImageEffects;
using UnityEngine;
using UnityEngine.Events;

namespace CodeBase._Main
{
	public class PickupSphere : MonoBehaviour
	{
		public static event OnCollectAction OnCollectEvent;
		[HideInInspector]
		public UnityEvent OnCollect;

		private void Start()
		{
			_bloom = FindObjectOfType<BloomOptimized>();
			if (_bloom != null)
			{
				_bloomInitValue = _bloom.intensity;
			}
			transform.localRotation = Random.rotation;
		}

		private void Update()
		{
			if (_isDestroyed)
			{
				return;
			}
			ring1.transform.Rotate(Vector3.right, ringRotationSpeed * Time.deltaTime);
			ring2.transform.Rotate(Vector3.up, ringRotationSpeed * Time.deltaTime);
			if (growingEnabled && transform.localScale.x < maxScale)
			{
				transform.localScale += Vector3.one * growthSpeed * Time.deltaTime;
			}
			if (_isTweeningOut)
			{
				transform.localScale = Vector3.Lerp(transform.localScale, Vector3.zero, 5f * Time.deltaTime);
				if (_bloom != null)
				{
					_bloom.intensity = Mathf.Lerp(_bloom.intensity, _bloomInitValue, 5f * Time.deltaTime);
				}
			}
		}

		private void OnTriggerEnter(Collider collider)
		{
			if (_activated)
			{
				return;
			}
			if (collider.gameObject.CompareTag(Tags.Player))
			{
				_activated = true;
				if (OnCollectEvent != null)
				{
					OnCollectEvent();
				}
				AudioSource componentInParent = GetComponentInParent<AudioSource>();
				if (componentInParent != null)
				{
					componentInParent.Play();
				}
				_isTweeningOut = true;
				if (_bloom != null)
				{
					_bloom.intensity = 0.5f;
				}
				BoidMaster componentInParent2 = GetComponentInParent<BoidMaster>();
				if (componentInParent2 != null)
				{
					componentInParent2.neighborDistance = 250f;
				}
				Invoke("DestroyNow", 1f);
			}
		}

		public void TweenBloom(float value)
		{
			_bloom.intensity = value;
		}

		private void DestroyNow()
		{
			_bloom.intensity = _bloomInitValue;
			DestroyObject(sphere);
			DestroyObject(ring1);
			DestroyObject(ring2);
			_isDestroyed = true;
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

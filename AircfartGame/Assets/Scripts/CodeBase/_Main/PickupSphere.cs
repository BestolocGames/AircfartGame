using CodeBase._ImageEffects;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

namespace CodeBase._Main
{
	public class PickupSphere : MonoBehaviour
	{
		public static event CollectAction OnCollectEvent;
		[FormerlySerializedAs("OnCollect")] [HideInInspector]
		public UnityEvent _collect;

		private void Start()
		{
			_bloom = FindObjectOfType<BloomOptimized>();
			if (_bloom != null)
			{
				_bloomInitValue = _bloom._intensity;
			}
			transform.localRotation = Random.rotation;
		}

		private void Update()
		{
			if (_isDestroyed)
			{
				return;
			}
			_ring1.transform.Rotate(Vector3.right, _ringRotationSpeed * Time.deltaTime);
			_ring2.transform.Rotate(Vector3.up, _ringRotationSpeed * Time.deltaTime);
			if (GrowingEnabled && transform.localScale.x < _maxScale)
			{
				transform.localScale += Vector3.one * _growthSpeed * Time.deltaTime;
			}
			if (_isTweeningOut)
			{
				transform.localScale = Vector3.Lerp(transform.localScale, Vector3.zero, 5f * Time.deltaTime);
				if (_bloom != null)
				{
					_bloom._intensity = Mathf.Lerp(_bloom._intensity, _bloomInitValue, 5f * Time.deltaTime);
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
					_bloom._intensity = 0.5f;
				}
				BoidMaster componentInParent2 = GetComponentInParent<BoidMaster>();
				if (componentInParent2 != null)
				{
					componentInParent2._neighborDistance = 250f;
				}
				Invoke("DestroyNow", 1f);
			}
		}

		public void TweenBloom(float value)
		{
			_bloom._intensity = value;
		}

		private void DestroyNow()
		{
			_bloom._intensity = _bloomInitValue;
			DestroyObject(_sphere);
			DestroyObject(_ring1);
			DestroyObject(_ring2);
			_isDestroyed = true;
			_collect.Invoke();
		}

		public static bool GrowingEnabled;

		[FormerlySerializedAs("growthSpeed")] public float _growthSpeed = 0.1f;

		[FormerlySerializedAs("maxScale")] public float _maxScale = 40f;

		[FormerlySerializedAs("ringRotationSpeed")] public float _ringRotationSpeed = 150f;

		[FormerlySerializedAs("ring1")] public GameObject _ring1;

		[FormerlySerializedAs("ring2")] public GameObject _ring2;

		[FormerlySerializedAs("sphere")] public GameObject _sphere;

		private bool _activated;

		private BloomOptimized _bloom;

		private float _bloomInitValue;

		private bool _isTweeningOut;

		private bool _isDestroyed;

		public delegate void CollectAction();
	}
}

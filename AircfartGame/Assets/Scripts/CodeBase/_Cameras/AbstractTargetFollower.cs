using UnityEngine;
using UnityEngine.Serialization;

namespace CodeBase._Cameras
{
	public abstract class AbstractTargetFollower : MonoBehaviour
	{
		 [SerializeField]
		protected Transform _target;

		 [SerializeField]
		private bool _autoTargetPlayer = true;

		[SerializeField]
		private UpdateType _updateType;

		protected Rigidbody TargetRigidbody;
		
		public Transform Target => _target;

		public enum UpdateType
		{
			FixedUpdate,
			LateUpdate,
			ManualUpdate
		}
		
		protected virtual void Start()
		{
			if (_autoTargetPlayer) 
				FindAndTargetPlayer();
			if (_target == null)
				return;
			TargetRigidbody = _target.GetComponent<Rigidbody>();
		}

		private void FixedUpdate()
		{
			if (_autoTargetPlayer && (_target == null || !_target.gameObject.activeSelf)) 
				FindAndTargetPlayer();
			if (_updateType == UpdateType.FixedUpdate) 
				FollowTarget(Time.deltaTime);
		}

		private void LateUpdate()
		{
			if (_autoTargetPlayer && (_target == null || !_target.gameObject.activeSelf)) 
				FindAndTargetPlayer();
			if (_updateType == UpdateType.LateUpdate) 
				FollowTarget(Time.deltaTime);
		}

		public void ManualUpdate()
		{
			if (_autoTargetPlayer && (_target == null || !_target.gameObject.activeSelf)) 
				FindAndTargetPlayer();
			if (_updateType == UpdateType.ManualUpdate) 
				FollowTarget(Time.deltaTime);
		}

		protected abstract void FollowTarget(float deltaTime);

		private void FindAndTargetPlayer()
		{
			GameObject gameObject = GameObject.FindGameObjectWithTag("Player");
			if (gameObject) 
				SetTarget(gameObject.transform);
		}

		protected virtual void SetTarget(Transform newTransform) => 
			_target = newTransform;
	}
}

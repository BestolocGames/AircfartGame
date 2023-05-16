using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;

namespace CodeBase._Main
{
	public class RevivePermissionProvider : MonoBehaviour
	{
		public static event GameActions.SimpleAction OnReviveRequested;

		public static event GameActions.SimpleAction OnReviveGranted;

		private void OnEnable()
		{
			OnReviveRequested = (GameActions.SimpleAction)Delegate.Combine(OnReviveRequested, new GameActions.SimpleAction(HandleReviveRequested));
		}

		private void OnDisable()
		{
			OnReviveRequested = (GameActions.SimpleAction)Delegate.Remove(OnReviveRequested, new GameActions.SimpleAction(HandleReviveRequested));
		}

		public virtual void RequestRevive()
		{
			if (OnReviveRequested != null)
			{
				OnReviveRequested();
			}
		}

		public virtual void GrantRevive()
		{
			if (OnReviveGranted != null)
			{
				OnReviveGranted();
			}
		}

		protected virtual void HandleReviveRequested()
		{
			if (_bypassAdsProvider )
			{
				StartCoroutine(ReviveNextFrame());
			}
		}

		private IEnumerator ReviveNextFrame()
		{
			yield return new WaitForEndOfFrame();
			GrantRevive();
			yield break;
		}

		[FormerlySerializedAs("bypassAdsProvider")] [Tooltip("Skip all conditions and revive the user one they request it.")]
		public bool _bypassAdsProvider;
	}
}

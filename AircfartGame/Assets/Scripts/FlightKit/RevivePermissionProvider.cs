// dnSpy decompiler from Assembly-CSharp.dll class: FlightKit.RevivePermissionProvider
using System;
using System.Collections;
using UnityEngine;

namespace FlightKit
{
	public class RevivePermissionProvider : MonoBehaviour
	{
		public static event GameActions.SimpleAction OnReviveRequested;

		public static event GameActions.SimpleAction OnReviveGranted;

		private void OnEnable()
		{
			RevivePermissionProvider.OnReviveRequested = (GameActions.SimpleAction)Delegate.Combine(RevivePermissionProvider.OnReviveRequested, new GameActions.SimpleAction(this.HandleReviveRequested));
		}

		private void OnDisable()
		{
			RevivePermissionProvider.OnReviveRequested = (GameActions.SimpleAction)Delegate.Remove(RevivePermissionProvider.OnReviveRequested, new GameActions.SimpleAction(this.HandleReviveRequested));
		}

		public virtual void RequestRevive()
		{
			if (RevivePermissionProvider.OnReviveRequested != null)
			{
				RevivePermissionProvider.OnReviveRequested();
			}
		}

		public virtual void GrantRevive()
		{
			if (RevivePermissionProvider.OnReviveGranted != null)
			{
				RevivePermissionProvider.OnReviveGranted();
			}
		}

		protected virtual void HandleReviveRequested()
		{
			if (this.bypassAdsProvider || this.adsProvider == null)
			{
				base.StartCoroutine(this.ReviveNextFrame());
			}
			else
			{
				this.adsProvider.ShowRewardedAd(new Action<AdShowResult>(this.HandleShowResult));
			}
		}

		protected void HandleShowResult(AdShowResult result)
		{
			switch (result)
			{
			case AdShowResult.Finished:
				UnityEngine.Debug.Log("The ad was successfully shown.");
				this.GrantRevive();
				break;
			case AdShowResult.Skipped:
				UnityEngine.Debug.Log("The ad was skipped before reaching the end.");
				break;
			case AdShowResult.Failed:
				UnityEngine.Debug.LogError("The ad failed to be shown.");
				break;
			}
		}

		private IEnumerator ReviveNextFrame()
		{
			yield return new WaitForEndOfFrame();
			this.GrantRevive();
			yield break;
		}

		[Tooltip("Skip all conditions and revive the user one they request it.")]
		public bool bypassAdsProvider;

		[Tooltip("Implementation of Ads Provider that will show ads, e.g. UnityAdsManager.")]
		public AbstractAdsProvider adsProvider;
	}
}

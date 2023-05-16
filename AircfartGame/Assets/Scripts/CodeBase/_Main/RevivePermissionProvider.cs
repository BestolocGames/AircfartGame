using System;
using System.Collections;
using UnityEngine;

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
			if (bypassAdsProvider || adsProvider == null)
			{
				StartCoroutine(ReviveNextFrame());
			}
			else
			{
				adsProvider.ShowRewardedAd(new Action<AdShowResult>(HandleShowResult));
			}
		}

		protected void HandleShowResult(AdShowResult result)
		{
			switch (result)
			{
			case AdShowResult.Finished:
				Debug.Log("The ad was successfully shown.");
				GrantRevive();
				break;
			case AdShowResult.Skipped:
				Debug.Log("The ad was skipped before reaching the end.");
				break;
			case AdShowResult.Failed:
				Debug.LogError("The ad failed to be shown.");
				break;
			}
		}

		private IEnumerator ReviveNextFrame()
		{
			yield return new WaitForEndOfFrame();
			GrantRevive();
			yield break;
		}

		[Tooltip("Skip all conditions and revive the user one they request it.")]
		public bool bypassAdsProvider;

		[Tooltip("Implementation of Ads Provider that will show ads, e.g. UnityAdsManager.")]
		public AbstractAdsProvider adsProvider;
	}
}

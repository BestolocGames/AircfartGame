using System;
using UnityEngine;

namespace CodeBase._Main
{
	public abstract class AbstractAdsProvider : MonoBehaviour
	{
		public virtual void ShowRewardedAd(Action<AdShowResult> resultCallback)
		{
		}
	}
}

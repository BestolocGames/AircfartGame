// dnSpy decompiler from Assembly-CSharp.dll class: FlightKit.MobileInputUIActivator
using System;
using UnityEngine;

namespace FlightKit
{
	public class MobileInputUIActivator : MonoBehaviour
	{
		private void Start()
		{
			ControlsPrefs.OnTiltEnabledEvent += this.HandleTiltEnabled;
			ControlsPrefs.OnTiltDisabledEvent += this.HandleTiltDisabled;
			if (ControlsPrefs.IsTiltEnabled)
			{
				this.HandleTiltEnabled();
			}
			else
			{
				this.HandleTiltDisabled();
			}
			UIEventsPublisher.OnPlayEvent += this.UpdateUI;
		}

		private void OnDisable()
		{
			ControlsPrefs.OnTiltEnabledEvent -= this.HandleTiltEnabled;
			ControlsPrefs.OnTiltDisabledEvent -= this.HandleTiltDisabled;
			UIEventsPublisher.OnPlayEvent -= this.UpdateUI;
		}

		private void HandleTiltEnabled()
		{
			this._isTiltUiMode = true;
			this.UpdateUI();
		}

		private void HandleTiltDisabled()
		{
			this._isTiltUiMode = false;
			this.UpdateUI();
		}

		private void UpdateUI()
		{
			if (this._isTiltUiMode)
			{
				foreach (GameObject gameObject in this.touchUIElements)
				{
					gameObject.SetActive(false);
				}
				foreach (GameObject gameObject2 in this.tiltUIElements)
				{
					gameObject2.SetActive(true);
				}
			}
			else
			{
				foreach (GameObject gameObject3 in this.tiltUIElements)
				{
					gameObject3.SetActive(false);
				}
				foreach (GameObject gameObject4 in this.touchUIElements)
				{
					gameObject4.SetActive(true);
				}
			}
		}

		public GameObject[] tiltUIElements;

		public GameObject[] touchUIElements;

		private bool _isTiltUiMode;
	}
}

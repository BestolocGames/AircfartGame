using CodeBase._Main;
using UI;
using UnityEngine;

namespace CodeBase._CrossPlatformInput
{
	public class MobileInputUIActivator : MonoBehaviour
	{
		public GameObject[] tiltUIElements;

		public GameObject[] touchUIElements;

		private bool _isTiltUiMode;
		
		private void Start()
		{
			ControlsPrefs.OnTiltEnabledEvent += HandleTiltEnabled;
			ControlsPrefs.OnTiltDisabledEvent += HandleTiltDisabled;
			if (ControlsPrefs.IsTiltEnabled)
			{
				HandleTiltEnabled();
			}
			else
			{
				HandleTiltDisabled();
			}
			UIEventsPublisher.OnPlayEvent += UpdateUI;
		}

		private void OnDisable()
		{
			ControlsPrefs.OnTiltEnabledEvent -= HandleTiltEnabled;
			ControlsPrefs.OnTiltDisabledEvent -= HandleTiltDisabled;
			UIEventsPublisher.OnPlayEvent -= UpdateUI;
		}

		private void HandleTiltEnabled()
		{
			_isTiltUiMode = true;
			UpdateUI();
		}

		private void HandleTiltDisabled()
		{
			_isTiltUiMode = false;
			UpdateUI();
		}

		private void UpdateUI()
		{
			if (_isTiltUiMode)
			{
				foreach (GameObject gameObject in touchUIElements)
				{
					gameObject.SetActive(false);
				}
				foreach (GameObject gameObject2 in tiltUIElements)
				{
					gameObject2.SetActive(true);
				}
			}
			else
			{
				foreach (GameObject gameObject3 in tiltUIElements)
				{
					gameObject3.SetActive(false);
				}
				foreach (GameObject gameObject4 in touchUIElements)
				{
					gameObject4.SetActive(true);
				}
			}
		}


	}
}

// dnSpy decompiler from Assembly-CSharp.dll class: FlightKit.FuelProgressBarCircle
using System;
using UnityEngine;
using UnityEngine.UI;

namespace FlightKit
{
	[RequireComponent(typeof(CanvasGroup))]
	public class FuelProgressBarCircle : MonoBehaviour
	{
		private void Start()
		{
			this._fuelController = UnityEngine.Object.FindObjectOfType<FuelController>();
			if (this._fuelController == null)
			{
				UnityEngine.Debug.LogError("FuelController not found.");
				base.enabled = false;
				return;
			}
			this._canvasGroup = base.GetComponent<CanvasGroup>();
			this._canvasGroup.alpha = 0f;
		}

		private void OnEnable()
		{
			TakeOffPublisher.OnTakeOffEvent += this.FadeIn;
			PauseController.OnPauseEvent += this.FadeOut;
			PauseController.OnUnPauseEvent += this.FadeIn;
			RevivePermissionProvider.OnReviveGranted += this.FadeIn;
			FuelController.OnFuelEmptyEvent += this.FadeOut;
		}

		private void OnDisable()
		{
			TakeOffPublisher.OnTakeOffEvent -= this.FadeIn;
			PauseController.OnPauseEvent -= this.FadeOut;
			PauseController.OnUnPauseEvent -= this.FadeIn;
			RevivePermissionProvider.OnReviveGranted -= this.FadeIn;
			FuelController.OnFuelEmptyEvent -= this.FadeOut;
		}

		private void FadeIn()
		{
			Fader.FadeAlpha(this, this._canvasGroup, true, 1f, null);
		}

		private void FadeOut()
		{
			Fader.FadeAlpha(this, this._canvasGroup, false, 1f, null);
		}

		private void Update()
		{
			if (this.decreaseByScaling)
			{
				Vector3 localScale = new Vector3(this._fuelController.fuelAmount, this._fuelController.fuelAmount, 1f);
				this.barFuelFull.rectTransform.localScale = localScale;
				this.barFuelLow.rectTransform.localScale = localScale;
			}
			else
			{
				this.barFuelFull.fillAmount = this._fuelController.fuelAmount;
				this.barFuelLow.fillAmount = this.barFuelFull.fillAmount;
			}
			if (this._fuelController.fuelAmount < this.blendStart)
			{
				this.barFuelFull.color = new Color(1f, 1f, 1f, (this._fuelController.fuelAmount - this.blendEnd) / (this.blendStart - this.blendEnd));
			}
		}

		public Image barFuelFull;

		public Image barFuelLow;

		[Tooltip("Normalized value where full fuel bar starts to disappear.")]
		[Space]
		public float blendStart = 0.5f;

		[Tooltip("Normalized value where full fuel bar is no longer visible at all.")]
		public float blendEnd = 0.2f;

		[Tooltip("Instead of changing fill amount of the bar, the script will change the scale.")]
		[Space]
		public bool decreaseByScaling;

		protected FuelController _fuelController;

		protected CanvasGroup _canvasGroup;
	}
}

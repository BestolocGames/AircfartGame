// dnSpy decompiler from Assembly-CSharp.dll class: FlightKit.FuelProgressBarRect
using System;
using UnityEngine;
using UnityEngine.UI;

namespace FlightKit
{
	[RequireComponent(typeof(Slider))]
	public class FuelProgressBarRect : MonoBehaviour
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
			this._slider = base.GetComponent<Slider>();
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
			this._slider.value = this._fuelController.fuelAmount;
		}

		public virtual void HandleSliderValueChanged()
		{
			this.barFuelLow.rectTransform.anchorMax = this.barFuelFull.rectTransform.anchorMax;
			if (this._slider.value < this.blendStart)
			{
				this.barFuelFull.color = new Color(1f, 1f, 1f, (this._slider.value - this.blendEnd) / (this.blendStart - this.blendEnd));
			}
		}

		public Image barFuelFull;

		public Image barFuelLow;

		[Space]
		[Tooltip("Normalized value where full fuel bar starts to disappear.")]
		public float blendStart = 0.5f;

		[Tooltip("Normalized value where full fuel bar is no longer visible at all.")]
		public float blendEnd = 0.2f;

		private Slider _slider;

		protected FuelController _fuelController;

		protected CanvasGroup _canvasGroup;
	}
}

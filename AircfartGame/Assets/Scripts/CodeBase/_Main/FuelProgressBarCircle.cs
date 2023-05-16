using UI;
using UnityEngine;
using UnityEngine.UI;

namespace CodeBase._Main
{
	[RequireComponent(typeof(CanvasGroup))]
	public class FuelProgressBarCircle : MonoBehaviour
	{
		private void Start()
		{
			_fuelController = FindObjectOfType<FuelController>();
			if (_fuelController == null)
			{
				Debug.LogError("FuelController not found.");
				enabled = false;
				return;
			}
			_canvasGroup = GetComponent<CanvasGroup>();
			_canvasGroup.alpha = 0f;
		}

		private void OnEnable()
		{
			TakeOffPublisher.OnTakeOffEvent += FadeIn;
			PauseController.OnPauseEvent += FadeOut;
			PauseController.OnUnPauseEvent += FadeIn;
			RevivePermissionProvider.OnReviveGranted += FadeIn;
			FuelController.OnFuelEmptyEvent += FadeOut;
		}

		private void OnDisable()
		{
			TakeOffPublisher.OnTakeOffEvent -= FadeIn;
			PauseController.OnPauseEvent -= FadeOut;
			PauseController.OnUnPauseEvent -= FadeIn;
			RevivePermissionProvider.OnReviveGranted -= FadeIn;
			FuelController.OnFuelEmptyEvent -= FadeOut;
		}

		private void FadeIn()
		{
			Fader.FadeAlpha(this, _canvasGroup, true, 1f, null);
		}

		private void FadeOut()
		{
			Fader.FadeAlpha(this, _canvasGroup, false, 1f, null);
		}

		private void Update()
		{
			if (decreaseByScaling)
			{
				Vector3 localScale = new Vector3(_fuelController.fuelAmount, _fuelController.fuelAmount, 1f);
				barFuelFull.rectTransform.localScale = localScale;
				barFuelLow.rectTransform.localScale = localScale;
			}
			else
			{
				barFuelFull.fillAmount = _fuelController.fuelAmount;
				barFuelLow.fillAmount = barFuelFull.fillAmount;
			}
			if (_fuelController.fuelAmount < blendStart)
			{
				barFuelFull.color = new Color(1f, 1f, 1f, (_fuelController.fuelAmount - blendEnd) / (blendStart - blendEnd));
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

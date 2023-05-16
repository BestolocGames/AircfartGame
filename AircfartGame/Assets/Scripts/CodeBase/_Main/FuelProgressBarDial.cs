using UI;
using UnityEngine;
using UnityEngine.UI;

namespace CodeBase._Main
{
	[RequireComponent(typeof(CanvasGroup))]
	public class FuelProgressBarDial : MonoBehaviour
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
			float angle = -_fuelController.fuelAmount * 180f + 90f;
			dialHand.rectTransform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
			if (_fuelController.fuelAmount < blendStart)
			{
				barFuelFull.color = new Color(1f, 1f, 1f, (_fuelController.fuelAmount - blendEnd) / (blendStart - blendEnd));
			}
		}

		public Image barFuelFull;

		public Image barFuelLow;

		public Image dialHand;

		[Space]
		[Tooltip("Normalized value where full fuel bar starts to disappear.")]
		public float blendStart = 0.5f;

		[Tooltip("Normalized value where full fuel bar is no longer visible at all.")]
		public float blendEnd = 0.2f;

		protected FuelController _fuelController;

		protected CanvasGroup _canvasGroup;
	}
}

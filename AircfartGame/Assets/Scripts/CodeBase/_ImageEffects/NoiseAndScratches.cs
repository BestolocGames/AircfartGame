using UnityEngine;
using UnityEngine.Serialization;

namespace CodeBase._ImageEffects
{
	[ExecuteInEditMode]
	[AddComponentMenu("Image Effects/Noise/Noise and Scratches")]
	[RequireComponent(typeof(Camera))]
	public class NoiseAndScratches : MonoBehaviour
	{
		protected void Start()
		{
			if (!SystemInfo.supportsImageEffects)
			{
				enabled = false;
				return;
			}
			if (_shaderRGB == null || _shaderYuv == null)
			{
				Debug.Log("Noise shaders are not set up! Disabling noise effect.");
				enabled = false;
			}
			else if (!_shaderRGB.isSupported)
			{
				enabled = false;
			}
			else if (!_shaderYuv.isSupported)
			{
				_rgbFallback = true;
			}
		}

		protected Material Material
		{
			get
			{
				if (_mMaterialRGB == null)
				{
					_mMaterialRGB = new Material(_shaderRGB);
					_mMaterialRGB.hideFlags = HideFlags.HideAndDontSave;
				}
				if (_mMaterialYuv == null && !_rgbFallback)
				{
					_mMaterialYuv = new Material(_shaderYuv);
					_mMaterialYuv.hideFlags = HideFlags.HideAndDontSave;
				}
				return (_rgbFallback || _monochrome) ? _mMaterialRGB : _mMaterialYuv;
			}
		}

		protected void OnDisable()
		{
			if (_mMaterialRGB)
			{
				DestroyImmediate(_mMaterialRGB);
			}
			if (_mMaterialYuv)
			{
				DestroyImmediate(_mMaterialYuv);
			}
		}

		private void SanitizeParameters()
		{
			_grainIntensityMin = Mathf.Clamp(_grainIntensityMin, 0f, 5f);
			_grainIntensityMax = Mathf.Clamp(_grainIntensityMax, 0f, 5f);
			_scratchIntensityMin = Mathf.Clamp(_scratchIntensityMin, 0f, 5f);
			_scratchIntensityMax = Mathf.Clamp(_scratchIntensityMax, 0f, 5f);
			_scratchFPS = Mathf.Clamp(_scratchFPS, 1f, 30f);
			_scratchJitter = Mathf.Clamp(_scratchJitter, 0f, 1f);
			_grainSize = Mathf.Clamp(_grainSize, 0.1f, 50f);
		}

		private void OnRenderImage(RenderTexture source, RenderTexture destination)
		{
			SanitizeParameters();
			if (_scratchTimeLeft <= 0f)
			{
				_scratchTimeLeft = Random.value * 2f / _scratchFPS;
				_scratchX = Random.value;
				_scratchY = Random.value;
			}
			_scratchTimeLeft -= Time.deltaTime;
			Material material = this.Material;
			material.SetTexture("_GrainTex", _grainTexture);
			material.SetTexture("_ScratchTex", _scratchTexture);
			float num = 1f / _grainSize;
			material.SetVector("_GrainOffsetScale", new Vector4(Random.value, Random.value, (float)Screen.width / (float)_grainTexture.width * num, (float)Screen.height / (float)_grainTexture.height * num));
			material.SetVector("_ScratchOffsetScale", new Vector4(_scratchX + Random.value * _scratchJitter, _scratchY + Random.value * _scratchJitter, (float)Screen.width / (float)_scratchTexture.width, (float)Screen.height / (float)_scratchTexture.height));
			material.SetVector("_Intensity", new Vector4(Random.Range(_grainIntensityMin, _grainIntensityMax), Random.Range(_scratchIntensityMin, _scratchIntensityMax), 0f, 0f));
			Graphics.Blit(source, destination, material);
		}

		[FormerlySerializedAs("monochrome")] public bool _monochrome = true;

		private bool _rgbFallback;

		[FormerlySerializedAs("grainIntensityMin")] [Range(0f, 5f)]
		public float _grainIntensityMin = 0.1f;

		[FormerlySerializedAs("grainIntensityMax")] [Range(0f, 5f)]
		public float _grainIntensityMax = 0.2f;

		[FormerlySerializedAs("grainSize")] [Range(0.1f, 50f)]
		public float _grainSize = 2f;

		[FormerlySerializedAs("scratchIntensityMin")] [Range(0f, 5f)]
		public float _scratchIntensityMin = 0.05f;

		[FormerlySerializedAs("scratchIntensityMax")] [Range(0f, 5f)]
		public float _scratchIntensityMax = 0.25f;

		[FormerlySerializedAs("scratchFPS")] [Range(1f, 30f)]
		public float _scratchFPS = 10f;

		[FormerlySerializedAs("scratchJitter")] [Range(0f, 1f)]
		public float _scratchJitter = 0.01f;

		[FormerlySerializedAs("grainTexture")] public Texture _grainTexture;

		[FormerlySerializedAs("scratchTexture")] public Texture _scratchTexture;

		[FormerlySerializedAs("shaderRGB")] public Shader _shaderRGB;

		[FormerlySerializedAs("shaderYUV")] public Shader _shaderYuv;

		private Material _mMaterialRGB;

		private Material _mMaterialYuv;

		private float _scratchTimeLeft;

		private float _scratchX;

		private float _scratchY;
	}
}

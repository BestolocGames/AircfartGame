using UnityEngine;
using UnityEngine.Serialization;

namespace CodeBase._ImageEffects
{
	[AddComponentMenu("Image Effects/Color Adjustments/Color Correction (Curves, Saturation)")]
	[ExecuteInEditMode]
	public class ColorCorrectionCurves : PostEffectsBase
	{
		private new void Start()
		{
			base.Start();
			_updateTexturesOnStartup = true;
		}

		private void Awake()
		{
		}

		public override bool CheckResources()
		{
			CheckSupport(_mode == ColorCorrectionMode.Advanced);
			_ccMaterial = CheckShaderAndCreateMaterial(_simpleColorCorrectionCurvesShader, _ccMaterial);
			_ccDepthMaterial = CheckShaderAndCreateMaterial(_colorCorrectionCurvesShader, _ccDepthMaterial);
			_selectiveCcMaterial = CheckShaderAndCreateMaterial(_colorCorrectionSelectiveShader, _selectiveCcMaterial);
			if (!_rgbChannelTex)
			{
				_rgbChannelTex = new Texture2D(256, 4, TextureFormat.ARGB32, false, true);
			}
			if (!_rgbDepthChannelTex)
			{
				_rgbDepthChannelTex = new Texture2D(256, 4, TextureFormat.ARGB32, false, true);
			}
			if (!_zCurveTex)
			{
				_zCurveTex = new Texture2D(256, 1, TextureFormat.ARGB32, false, true);
			}
			_rgbChannelTex.hideFlags = HideFlags.DontSave;
			_rgbDepthChannelTex.hideFlags = HideFlags.DontSave;
			_zCurveTex.hideFlags = HideFlags.DontSave;
			_rgbChannelTex.wrapMode = TextureWrapMode.Clamp;
			_rgbDepthChannelTex.wrapMode = TextureWrapMode.Clamp;
			_zCurveTex.wrapMode = TextureWrapMode.Clamp;
			if (!IsSupported)
			{
				ReportAutoDisable();
			}
			return IsSupported;
		}

		public void UpdateParameters()
		{
			CheckResources();
			if (_redChannel != null && _greenChannel != null && _blueChannel != null)
			{
				for (float num = 0f; num <= 1f; num += 0.003921569f)
				{
					float num2 = Mathf.Clamp(_redChannel.Evaluate(num), 0f, 1f);
					float num3 = Mathf.Clamp(_greenChannel.Evaluate(num), 0f, 1f);
					float num4 = Mathf.Clamp(_blueChannel.Evaluate(num), 0f, 1f);
					_rgbChannelTex.SetPixel((int)Mathf.Floor(num * 255f), 0, new Color(num2, num2, num2));
					_rgbChannelTex.SetPixel((int)Mathf.Floor(num * 255f), 1, new Color(num3, num3, num3));
					_rgbChannelTex.SetPixel((int)Mathf.Floor(num * 255f), 2, new Color(num4, num4, num4));
					float num5 = Mathf.Clamp(_zCurve.Evaluate(num), 0f, 1f);
					_zCurveTex.SetPixel((int)Mathf.Floor(num * 255f), 0, new Color(num5, num5, num5));
					num2 = Mathf.Clamp(_depthRedChannel.Evaluate(num), 0f, 1f);
					num3 = Mathf.Clamp(_depthGreenChannel.Evaluate(num), 0f, 1f);
					num4 = Mathf.Clamp(_depthBlueChannel.Evaluate(num), 0f, 1f);
					_rgbDepthChannelTex.SetPixel((int)Mathf.Floor(num * 255f), 0, new Color(num2, num2, num2));
					_rgbDepthChannelTex.SetPixel((int)Mathf.Floor(num * 255f), 1, new Color(num3, num3, num3));
					_rgbDepthChannelTex.SetPixel((int)Mathf.Floor(num * 255f), 2, new Color(num4, num4, num4));
				}
				_rgbChannelTex.Apply();
				_rgbDepthChannelTex.Apply();
				_zCurveTex.Apply();
			}
		}

		private void UpdateTextures()
		{
			UpdateParameters();
		}

		private void OnRenderImage(RenderTexture source, RenderTexture destination)
		{
			if (!CheckResources())
			{
				Graphics.Blit(source, destination);
				return;
			}
			if (_updateTexturesOnStartup)
			{
				UpdateParameters();
				_updateTexturesOnStartup = false;
			}
			if (_useDepthCorrection)
			{
				GetComponent<Camera>().depthTextureMode |= DepthTextureMode.Depth;
			}
			RenderTexture renderTexture = destination;
			if (_selectiveCc)
			{
				renderTexture = RenderTexture.GetTemporary(source.width, source.height);
			}
			if (_useDepthCorrection)
			{
				_ccDepthMaterial.SetTexture("_RgbTex", _rgbChannelTex);
				_ccDepthMaterial.SetTexture("_ZCurve", _zCurveTex);
				_ccDepthMaterial.SetTexture("_RgbDepthTex", _rgbDepthChannelTex);
				_ccDepthMaterial.SetFloat("_Saturation", _saturation);
				Graphics.Blit(source, renderTexture, _ccDepthMaterial);
			}
			else
			{
				_ccMaterial.SetTexture("_RgbTex", _rgbChannelTex);
				_ccMaterial.SetFloat("_Saturation", _saturation);
				Graphics.Blit(source, renderTexture, _ccMaterial);
			}
			if (_selectiveCc)
			{
				_selectiveCcMaterial.SetColor("selColor", _selectiveFromColor);
				_selectiveCcMaterial.SetColor("targetColor", _selectiveToColor);
				Graphics.Blit(renderTexture, destination, _selectiveCcMaterial);
				RenderTexture.ReleaseTemporary(renderTexture);
			}
		}

		[FormerlySerializedAs("redChannel")] public AnimationCurve _redChannel = new AnimationCurve(new Keyframe[]
		{
			new Keyframe(0f, 0f),
			new Keyframe(1f, 1f)
		});

		[FormerlySerializedAs("greenChannel")] public AnimationCurve _greenChannel = new AnimationCurve(new Keyframe[]
		{
			new Keyframe(0f, 0f),
			new Keyframe(1f, 1f)
		});

		[FormerlySerializedAs("blueChannel")] public AnimationCurve _blueChannel = new AnimationCurve(new Keyframe[]
		{
			new Keyframe(0f, 0f),
			new Keyframe(1f, 1f)
		});

		[FormerlySerializedAs("useDepthCorrection")] public bool _useDepthCorrection;

		[FormerlySerializedAs("zCurve")] public AnimationCurve _zCurve = new AnimationCurve(new Keyframe[]
		{
			new Keyframe(0f, 0f),
			new Keyframe(1f, 1f)
		});

		[FormerlySerializedAs("depthRedChannel")] public AnimationCurve _depthRedChannel = new AnimationCurve(new Keyframe[]
		{
			new Keyframe(0f, 0f),
			new Keyframe(1f, 1f)
		});

		[FormerlySerializedAs("depthGreenChannel")] public AnimationCurve _depthGreenChannel = new AnimationCurve(new Keyframe[]
		{
			new Keyframe(0f, 0f),
			new Keyframe(1f, 1f)
		});

		[FormerlySerializedAs("depthBlueChannel")] public AnimationCurve _depthBlueChannel = new AnimationCurve(new Keyframe[]
		{
			new Keyframe(0f, 0f),
			new Keyframe(1f, 1f)
		});

		private Material _ccMaterial;

		private Material _ccDepthMaterial;

		private Material _selectiveCcMaterial;

		private Texture2D _rgbChannelTex;

		private Texture2D _rgbDepthChannelTex;

		private Texture2D _zCurveTex;

		[FormerlySerializedAs("saturation")] public float _saturation = 1f;

		[FormerlySerializedAs("selectiveCc")] public bool _selectiveCc;

		[FormerlySerializedAs("selectiveFromColor")] public Color _selectiveFromColor = Color.white;

		[FormerlySerializedAs("selectiveToColor")] public Color _selectiveToColor = Color.white;

		[FormerlySerializedAs("mode")] public ColorCorrectionMode _mode;

		[FormerlySerializedAs("updateTextures")] public bool _updateTextures = true;

		[FormerlySerializedAs("colorCorrectionCurvesShader")] public Shader _colorCorrectionCurvesShader;

		[FormerlySerializedAs("simpleColorCorrectionCurvesShader")] public Shader _simpleColorCorrectionCurvesShader;

		[FormerlySerializedAs("colorCorrectionSelectiveShader")] public Shader _colorCorrectionSelectiveShader;

		private bool _updateTexturesOnStartup = true;

		public enum ColorCorrectionMode
		{
			Simple,
			Advanced
		}
	}
}

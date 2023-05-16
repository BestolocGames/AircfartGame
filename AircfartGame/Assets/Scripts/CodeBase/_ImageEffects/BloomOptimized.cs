using UnityEngine;
using UnityEngine.Serialization;

namespace CodeBase._ImageEffects
{
	[AddComponentMenu("Image Effects/Bloom and Glow/Bloom (Optimized)")]
	[RequireComponent(typeof(Camera))]
	[ExecuteInEditMode]
	public class BloomOptimized : PostEffectsBase
	{
		[FormerlySerializedAs("threshold")] [Range(0f, 1.5f)]
		public float _threshold = 0.25f;

		[FormerlySerializedAs("intensity")] [Range(0f, 2.5f)]
		public float _intensity = 0.75f;

		[FormerlySerializedAs("blurSize")] [Range(0.25f, 5.5f)]
		public float _blurSize = 1f;

		private Resolution _resolution;

		[FormerlySerializedAs("blurIterations")] [Range(1f, 4f)]
		public int _blurIterations = 1;

		[FormerlySerializedAs("blurType")] public BlurType _blurType;

		[FormerlySerializedAs("fastBloomShader")] public Shader _fastBloomShader;

		private Material _fastBloomMaterial;

		public enum Resolution
		{
			Low,
			High
		}

		public enum BlurType
		{
			Standard,
			Sgx
		}
		
		public override bool CheckResources()
		{
			CheckSupport(false);
			_fastBloomMaterial = CheckShaderAndCreateMaterial(_fastBloomShader, _fastBloomMaterial);
			if (!IsSupported)
			{
				ReportAutoDisable();
			}
			return IsSupported;
		}

		private void OnDisable()
		{
			if (_fastBloomMaterial) 
				DestroyImmediate(_fastBloomMaterial);
		}

		private void OnRenderImage(RenderTexture source, RenderTexture destination)
		{
			if (!CheckResources())
			{
				Graphics.Blit(source, destination);
				return;
			}
			int num = (_resolution != Resolution.Low) ? 2 : 4;
			float num2 = (_resolution != Resolution.Low) ? 1f : 0.5f;
			_fastBloomMaterial.SetVector("_Parameter", new Vector4(_blurSize * num2, 0f, _threshold, _intensity));
			source.filterMode = FilterMode.Bilinear;
			int width = source.width / num;
			int height = source.height / num;
			RenderTexture renderTexture = RenderTexture.GetTemporary(width, height, 0, source.format);
			renderTexture.filterMode = FilterMode.Bilinear;
			Graphics.Blit(source, renderTexture, _fastBloomMaterial, 1);
			int num3 = (_blurType != BlurType.Standard) ? 2 : 0;
			for (int i = 0; i < _blurIterations; i++)
			{
				_fastBloomMaterial.SetVector("_Parameter", new Vector4(_blurSize * num2 + (float)i * 1f, 0f, _threshold, _intensity));
				RenderTexture temporary = RenderTexture.GetTemporary(width, height, 0, source.format);
				temporary.filterMode = FilterMode.Bilinear;
				Graphics.Blit(renderTexture, temporary, _fastBloomMaterial, 2 + num3);
				RenderTexture.ReleaseTemporary(renderTexture);
				renderTexture = temporary;
				temporary = RenderTexture.GetTemporary(width, height, 0, source.format);
				temporary.filterMode = FilterMode.Bilinear;
				Graphics.Blit(renderTexture, temporary, _fastBloomMaterial, 3 + num3);
				RenderTexture.ReleaseTemporary(renderTexture);
				renderTexture = temporary;
			}
			_fastBloomMaterial.SetTexture("_Bloom", renderTexture);
			Graphics.Blit(source, destination, _fastBloomMaterial, 0);
			RenderTexture.ReleaseTemporary(renderTexture);
		}
	}
}

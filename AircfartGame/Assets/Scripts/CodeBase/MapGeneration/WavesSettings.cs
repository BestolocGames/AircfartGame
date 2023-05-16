using UnityEngine;
using UnityEngine.Serialization;

namespace CodeBase.MapGeneration
{
    public struct SpectrumSettings
    {
        public float Scale;
        public float Angle;
        public float SpreadBlend;
        public float Swell;
        public float Alpha;
        public float PeakOmega;
        public float Gamma;
        public float ShortWavesFade;
    }

    [System.Serializable]
    public struct DisplaySpectrumSettings
    {
        [FormerlySerializedAs("scale")] [Range(0, 1)]
        public float _scale;
        [FormerlySerializedAs("windSpeed")] public float _windSpeed;
        [FormerlySerializedAs("windDirection")] public float _windDirection;
        [FormerlySerializedAs("fetch")] public float _fetch;
        [FormerlySerializedAs("spreadBlend")] [Range(0, 1)]
        public float _spreadBlend;
        [FormerlySerializedAs("swell")] [Range(0, 1)]
        public float _swell;
        [FormerlySerializedAs("peakEnhancement")] public float _peakEnhancement;
        [FormerlySerializedAs("shortWavesFade")] public float _shortWavesFade;
    }

    [CreateAssetMenu(fileName = "New waves settings", menuName = "Ocean/Waves Settings")]
    public class WavesSettings : ScriptableObject
    {
        [FormerlySerializedAs("g")] public float _g;
        [FormerlySerializedAs("depth")] public float _depth;
        [FormerlySerializedAs("lambda")] [Range(0, 1)]
        public float _lambda;
        [FormerlySerializedAs("local")] public DisplaySpectrumSettings _local;
        [FormerlySerializedAs("swell")] public DisplaySpectrumSettings _swell;

        SpectrumSettings[] _spectrums = new SpectrumSettings[2];

        public void SetParametersToShader(ComputeShader shader, int kernelIndex, ComputeBuffer paramsBuffer)
        {
            shader.SetFloat(_gProp, _g);
            shader.SetFloat(_depthProp, _depth);

            FillSettingsStruct(_local, ref _spectrums[0]);
            FillSettingsStruct(_swell, ref _spectrums[1]);

            paramsBuffer.SetData(_spectrums);
            shader.SetBuffer(kernelIndex, _spectrumsProp, paramsBuffer);
        }

        void FillSettingsStruct(DisplaySpectrumSettings display, ref SpectrumSettings settings)
        {
            settings.Scale = display._scale;
            settings.Angle = display._windDirection / 180 * Mathf.PI;
            settings.SpreadBlend = display._spreadBlend;
            settings.Swell = Mathf.Clamp(display._swell, 0.01f, 1);
            settings.Alpha = JonswapAlpha(_g, display._fetch, display._windSpeed);
            settings.PeakOmega = JonswapPeakFrequency(_g, display._fetch, display._windSpeed);
            settings.Gamma = display._peakEnhancement;
            settings.ShortWavesFade = display._shortWavesFade;
        }

        float JonswapAlpha(float g, float fetch, float windSpeed)
        {
            return 0.076f * Mathf.Pow(g * fetch / windSpeed / windSpeed, -0.22f);
        }

        float JonswapPeakFrequency(float g, float fetch, float windSpeed)
        {
            return 22 * Mathf.Pow(windSpeed * fetch / g / g, -0.33f);
        }

        readonly int _gProp = Shader.PropertyToID("GravityAcceleration");
        readonly int _depthProp = Shader.PropertyToID("Depth");
        readonly int _spectrumsProp = Shader.PropertyToID("Spectrums");
    }
}
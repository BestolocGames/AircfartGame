using UnityEngine;

namespace CodeBase.MapGeneration
{
    public class WavesCascade
    {
        public RenderTexture Displacement => _displacement;
        public RenderTexture Derivatives => _derivatives;
        public RenderTexture Turbulence => _turbulence;

        public Texture2D GaussianNoise => _gaussianNoise;
        public RenderTexture PrecomputedData => _precomputedData;
        public RenderTexture InitialSpectrum => _initialSpectrum;

        readonly int _size;
        readonly ComputeShader _initialSpectrumShader;
        readonly ComputeShader _timeDependentSpectrumShader;
        readonly ComputeShader _texturesMergerShader;
        readonly FastFourierTransform _fft;
        readonly Texture2D _gaussianNoise;
        readonly ComputeBuffer _paramsBuffer;
        readonly RenderTexture _initialSpectrum;
        readonly RenderTexture _precomputedData;
    
        readonly RenderTexture _buffer;
        readonly RenderTexture _dxDz;
        readonly RenderTexture _dyDxz;
        readonly RenderTexture _dyxDyz;
        readonly RenderTexture _dxxDzz;

        readonly RenderTexture _displacement;
        readonly RenderTexture _derivatives;
        readonly RenderTexture _turbulence;

        float _lambda;

        public WavesCascade(int size,
            ComputeShader initialSpectrumShader,
            ComputeShader timeDependentSpectrumShader,
            ComputeShader texturesMergerShader,
            FastFourierTransform fft,
            Texture2D gaussianNoise)
        {
            this._size = size;
            this._initialSpectrumShader = initialSpectrumShader;
            this._timeDependentSpectrumShader = timeDependentSpectrumShader;
            this._texturesMergerShader = texturesMergerShader;
            this._fft = fft;
            this._gaussianNoise = gaussianNoise;

            _kernelInitialSpectrum = initialSpectrumShader.FindKernel("CalculateInitialSpectrum");
            _kernelConjugateSpectrum = initialSpectrumShader.FindKernel("CalculateConjugatedSpectrum");
            _kernelTimeDependentSpectrums = timeDependentSpectrumShader.FindKernel("CalculateAmplitudes");
            _kernelResultTextures = texturesMergerShader.FindKernel("FillResultTextures");

            _initialSpectrum = FastFourierTransform.CreateRenderTexture(size, RenderTextureFormat.ARGBFloat);
            _precomputedData = FastFourierTransform.CreateRenderTexture(size, RenderTextureFormat.ARGBFloat);
            _displacement = FastFourierTransform.CreateRenderTexture(size, RenderTextureFormat.ARGBFloat);
            _derivatives = FastFourierTransform.CreateRenderTexture(size, RenderTextureFormat.ARGBFloat, true);
            _turbulence = FastFourierTransform.CreateRenderTexture(size, RenderTextureFormat.ARGBFloat, true);
            _paramsBuffer = new ComputeBuffer(2, 8 * sizeof(float));

            _buffer = FastFourierTransform.CreateRenderTexture(size);
            _dxDz = FastFourierTransform.CreateRenderTexture(size);
            _dyDxz = FastFourierTransform.CreateRenderTexture(size);
            _dyxDyz = FastFourierTransform.CreateRenderTexture(size);
            _dxxDzz = FastFourierTransform.CreateRenderTexture(size);
        }

        public void Dispose()
        {
            _paramsBuffer?.Release();
        }

        public void CalculateInitials(WavesSettings wavesSettings, float lengthScale,
            float cutoffLow, float cutoffHigh)
        {
            _lambda = wavesSettings._lambda;

            _initialSpectrumShader.SetInt(_sizeProp, _size);
            _initialSpectrumShader.SetFloat(_lengthScaleProp, lengthScale);
            _initialSpectrumShader.SetFloat(_cutoffHighProp, cutoffHigh);
            _initialSpectrumShader.SetFloat(_cutoffLowProp, cutoffLow);
            wavesSettings.SetParametersToShader(_initialSpectrumShader, _kernelInitialSpectrum, _paramsBuffer);

            _initialSpectrumShader.SetTexture(_kernelInitialSpectrum, _h0KProp, _buffer);
            _initialSpectrumShader.SetTexture(_kernelInitialSpectrum, _precomputedDataProp, _precomputedData);
            _initialSpectrumShader.SetTexture(_kernelInitialSpectrum, _noiseProp, _gaussianNoise);
            _initialSpectrumShader.Dispatch(_kernelInitialSpectrum, _size / LocalWorkGroupsX, _size / LocalWorkGroupsY, 1);

            _initialSpectrumShader.SetTexture(_kernelConjugateSpectrum, _h0Prop, _initialSpectrum);
            _initialSpectrumShader.SetTexture(_kernelConjugateSpectrum, _h0KProp, _buffer);
            _initialSpectrumShader.Dispatch(_kernelConjugateSpectrum, _size / LocalWorkGroupsX, _size / LocalWorkGroupsY, 1);
        }

        public void CalculateWavesAtTime(float time)
        {
            // Calculating complex amplitudes
            _timeDependentSpectrumShader.SetTexture(_kernelTimeDependentSpectrums, _dxDzProp, _dxDz);
            _timeDependentSpectrumShader.SetTexture(_kernelTimeDependentSpectrums, _dyDxzProp, _dyDxz);
            _timeDependentSpectrumShader.SetTexture(_kernelTimeDependentSpectrums, _dyxDyzProp, _dyxDyz);
            _timeDependentSpectrumShader.SetTexture(_kernelTimeDependentSpectrums, _dxxDzzProp, _dxxDzz);
            _timeDependentSpectrumShader.SetTexture(_kernelTimeDependentSpectrums, _h0Prop, _initialSpectrum);
            _timeDependentSpectrumShader.SetTexture(_kernelTimeDependentSpectrums, _precomputedDataProp, _precomputedData);
            _timeDependentSpectrumShader.SetFloat(_timeProp, time);
            _timeDependentSpectrumShader.Dispatch(_kernelTimeDependentSpectrums, _size / LocalWorkGroupsX, _size / LocalWorkGroupsY, 1);

            // Calculating IFFTs of complex amplitudes
            _fft.Ifft2D(_dxDz, _buffer, true, false, true);
            _fft.Ifft2D(_dyDxz, _buffer, true, false, true);
            _fft.Ifft2D(_dyxDyz, _buffer, true, false, true);
            _fft.Ifft2D(_dxxDzz, _buffer, true, false, true);

            // Filling displacement and normals textures
            _texturesMergerShader.SetFloat("DeltaTime", Time.deltaTime);

            _texturesMergerShader.SetTexture(_kernelResultTextures, _dxDzProp, _dxDz);
            _texturesMergerShader.SetTexture(_kernelResultTextures, _dyDxzProp, _dyDxz);
            _texturesMergerShader.SetTexture(_kernelResultTextures, _dyxDyzProp, _dyxDyz);
            _texturesMergerShader.SetTexture(_kernelResultTextures, _dxxDzzProp, _dxxDzz);
            _texturesMergerShader.SetTexture(_kernelResultTextures, _displacementProp, _displacement);
            _texturesMergerShader.SetTexture(_kernelResultTextures, _derivativesProp, _derivatives);
            _texturesMergerShader.SetTexture(_kernelResultTextures, _turbulenceProp, _turbulence);
            _texturesMergerShader.SetFloat(_lambdaProp, _lambda);
            _texturesMergerShader.Dispatch(_kernelResultTextures, _size / LocalWorkGroupsX, _size / LocalWorkGroupsY, 1);

            _derivatives.GenerateMips();
            _turbulence.GenerateMips();
        }

        const int LocalWorkGroupsX = 8;
        const int LocalWorkGroupsY = 8;

        // Kernel IDs:
        int _kernelInitialSpectrum;
        int _kernelConjugateSpectrum;
        int _kernelTimeDependentSpectrums;
        int _kernelResultTextures;

        // Property IDs
        readonly int _sizeProp = Shader.PropertyToID("Size");
        readonly int _lengthScaleProp = Shader.PropertyToID("LengthScale");
        readonly int _cutoffHighProp = Shader.PropertyToID("CutoffHigh");
        readonly int _cutoffLowProp = Shader.PropertyToID("CutoffLow");

        readonly int _noiseProp = Shader.PropertyToID("Noise");
        readonly int _h0Prop = Shader.PropertyToID("H0");
        readonly int _h0KProp = Shader.PropertyToID("H0K");
        readonly int _precomputedDataProp = Shader.PropertyToID("WavesData");
        readonly int _timeProp = Shader.PropertyToID("Time");

        readonly int _dxDzProp = Shader.PropertyToID("Dx_Dz");
        readonly int _dyDxzProp = Shader.PropertyToID("Dy_Dxz");
        readonly int _dyxDyzProp = Shader.PropertyToID("Dyx_Dyz");
        readonly int _dxxDzzProp = Shader.PropertyToID("Dxx_Dzz");
        readonly int _lambdaProp = Shader.PropertyToID("Lambda");

        readonly int _displacementProp = Shader.PropertyToID("Displacement");
        readonly int _derivativesProp = Shader.PropertyToID("Derivatives");
        readonly int _turbulenceProp = Shader.PropertyToID("Turbulence"); 
    }
}

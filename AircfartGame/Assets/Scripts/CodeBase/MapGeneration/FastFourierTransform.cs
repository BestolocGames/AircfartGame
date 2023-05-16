using UnityEngine;

namespace CodeBase.MapGeneration
{
    public class FastFourierTransform
    {
        const int LocalWorkGroupsX = 8;
        const int LocalWorkGroupsY = 8;

        readonly int _size;
        readonly ComputeShader _fftShader;
        readonly RenderTexture _precomputedData;

        public static RenderTexture CreateRenderTexture(int size, RenderTextureFormat format = RenderTextureFormat.RGFloat, bool useMips = false)
        {
            RenderTexture rt = new RenderTexture(size, size, 0,
                format, RenderTextureReadWrite.Linear);
            rt.useMipMap = useMips;
            rt.autoGenerateMips = false;
            rt.anisoLevel = 6;
            rt.filterMode = FilterMode.Trilinear;
            rt.wrapMode = TextureWrapMode.Repeat;
            rt.enableRandomWrite = true;
            rt.Create();
            return rt;
        }

        public FastFourierTransform(int size, ComputeShader fftShader)
        {
            this._size = size;
            this._fftShader = fftShader;
            _precomputedData = PrecomputeTwiddleFactorsAndInputIndices();

            _kernelPrecompute = fftShader.FindKernel("PrecomputeTwiddleFactorsAndInputIndices");
            _kernelHorizontalStepFFT = fftShader.FindKernel("HorizontalStepFFT");
            _kernelVerticalStepFFT = fftShader.FindKernel("VerticalStepFFT");
            _kernelHorizontalStepIfft = fftShader.FindKernel("HorizontalStepInverseFFT");
            _kernelVerticalStepIfft = fftShader.FindKernel("VerticalStepInverseFFT");
            _kernelScale = fftShader.FindKernel("Scale");
            _kernelPermute = fftShader.FindKernel("Permute");
        }

        public void FFT2D(RenderTexture input, RenderTexture buffer, bool outputToInput = false)
        {
            int logSize = (int)Mathf.Log(_size, 2);
            bool pingPong = false;

            _fftShader.SetTexture(_kernelHorizontalStepFFT, _propIDPrecomputedData, _precomputedData);
            _fftShader.SetTexture(_kernelHorizontalStepFFT, _propIDBuffer0, input);
            _fftShader.SetTexture(_kernelHorizontalStepFFT, _propIDBuffer1, buffer);
            for (int i = 0; i < logSize; i++)
            {
                pingPong = !pingPong;
                _fftShader.SetInt(_propIDStep, i);
                _fftShader.SetBool(_propIDPingpong, pingPong);
                _fftShader.Dispatch(_kernelHorizontalStepFFT, _size / LocalWorkGroupsX, _size / LocalWorkGroupsY, 1);
            }

            _fftShader.SetTexture(_kernelVerticalStepFFT, _propIDPrecomputedData, _precomputedData);
            _fftShader.SetTexture(_kernelVerticalStepFFT, _propIDBuffer0, input);
            _fftShader.SetTexture(_kernelVerticalStepFFT, _propIDBuffer1, buffer);
            for (int i = 0; i < logSize; i++)
            {
                pingPong = !pingPong;
                _fftShader.SetInt(_propIDStep, i);
                _fftShader.SetBool(_propIDPingpong, pingPong);
                _fftShader.Dispatch(_kernelVerticalStepFFT, _size / LocalWorkGroupsX, _size / LocalWorkGroupsY, 1);
            }

            if (pingPong && outputToInput)
            {
                Graphics.Blit(buffer, input);
            }

            if (!pingPong && !outputToInput)
            {
                Graphics.Blit(input, buffer);
            }
        }

        public void Ifft2D(RenderTexture input, RenderTexture buffer, bool outputToInput = false, bool scale = true, bool permute = false)
        {
            int logSize = (int)Mathf.Log(_size, 2);
            bool pingPong = false;

            _fftShader.SetTexture(_kernelHorizontalStepIfft, _propIDPrecomputedData, _precomputedData);
            _fftShader.SetTexture(_kernelHorizontalStepIfft, _propIDBuffer0, input);
            _fftShader.SetTexture(_kernelHorizontalStepIfft, _propIDBuffer1, buffer);
            for (int i = 0; i < logSize; i++)
            {
                pingPong = !pingPong;
                _fftShader.SetInt(_propIDStep, i);
                _fftShader.SetBool(_propIDPingpong, pingPong);
                _fftShader.Dispatch(_kernelHorizontalStepIfft, _size / LocalWorkGroupsX, _size / LocalWorkGroupsY, 1);
            }

            _fftShader.SetTexture(_kernelVerticalStepIfft, _propIDPrecomputedData, _precomputedData);
            _fftShader.SetTexture(_kernelVerticalStepIfft, _propIDBuffer0, input);
            _fftShader.SetTexture(_kernelVerticalStepIfft, _propIDBuffer1, buffer);
            for (int i = 0; i < logSize; i++)
            {
                pingPong = !pingPong;
                _fftShader.SetInt(_propIDStep, i);
                _fftShader.SetBool(_propIDPingpong, pingPong);
                _fftShader.Dispatch(_kernelVerticalStepIfft, _size / LocalWorkGroupsX, _size / LocalWorkGroupsY, 1);
            }

            if (pingPong && outputToInput)
            {
                Graphics.Blit(buffer, input);
            }

            if (!pingPong && !outputToInput)
            {
                Graphics.Blit(input, buffer);
            }

            if (permute)
            {
                _fftShader.SetInt(_propIDSize, _size);
                _fftShader.SetTexture(_kernelPermute, _propIDBuffer0, outputToInput ? input : buffer);
                _fftShader.Dispatch(_kernelPermute, _size / LocalWorkGroupsX, _size / LocalWorkGroupsY, 1);
            }
        
            if (scale)
            {
                _fftShader.SetInt(_propIDSize, _size);
                _fftShader.SetTexture(_kernelScale, _propIDBuffer0, outputToInput ? input : buffer);
                _fftShader.Dispatch(_kernelScale, _size / LocalWorkGroupsX, _size / LocalWorkGroupsY, 1);
            }
        }

        RenderTexture PrecomputeTwiddleFactorsAndInputIndices()
        {
            int logSize = (int)Mathf.Log(_size, 2);
            RenderTexture rt = new RenderTexture(logSize, _size, 0,
                RenderTextureFormat.ARGBFloat, RenderTextureReadWrite.Linear);
            rt.filterMode = FilterMode.Point;
            rt.wrapMode = TextureWrapMode.Repeat;
            rt.enableRandomWrite = true;
            rt.Create();

            _fftShader.SetInt(_propIDSize, _size);
            _fftShader.SetTexture(_kernelPrecompute, _propIDPrecomputeBuffer, rt);
            _fftShader.Dispatch(_kernelPrecompute, logSize, _size / 2 / LocalWorkGroupsY, 1);
            return rt;
        }

        // Kernel IDs:
        readonly int _kernelPrecompute;
        readonly int _kernelHorizontalStepFFT;
        readonly int _kernelVerticalStepFFT;
        readonly int _kernelHorizontalStepIfft;
        readonly int _kernelVerticalStepIfft;
        readonly int _kernelScale;
        readonly int _kernelPermute;

        // Property IDs:
        readonly int _propIDPrecomputeBuffer = Shader.PropertyToID("PrecomputeBuffer");
        readonly int _propIDPrecomputedData = Shader.PropertyToID("PrecomputedData");
        readonly int _propIDBuffer0 = Shader.PropertyToID("Buffer0");
        readonly int _propIDBuffer1 = Shader.PropertyToID("Buffer1");
        readonly int _propIDSize = Shader.PropertyToID("Size");
        readonly int _propIDStep = Shader.PropertyToID("Step");
        readonly int _propIDPingpong = Shader.PropertyToID("PingPong");
    }
}

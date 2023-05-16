using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Serialization;

namespace CodeBase.MapGeneration
{
    public class WavesGenerator : MonoBehaviour
    {
        public WavesCascade Cascade0;
        public WavesCascade Cascade1;
        public WavesCascade Cascade2;

        // must be a power of 2
        [FormerlySerializedAs("size")] [SerializeField]
        int _size = 256;

        [FormerlySerializedAs("wavesSettings")] [SerializeField]
        WavesSettings _wavesSettings;
        [FormerlySerializedAs("alwaysRecalculateInitials")] [SerializeField]
        bool _alwaysRecalculateInitials = false;
        [FormerlySerializedAs("lengthScale0")] [SerializeField]
        float _lengthScale0 = 250;
        [FormerlySerializedAs("lengthScale1")] [SerializeField]
        float _lengthScale1 = 17;
        [FormerlySerializedAs("lengthScale2")] [SerializeField]
        float _lengthScale2 = 5;

        [FormerlySerializedAs("fftShader")] [SerializeField]
        ComputeShader _fftShader;
        [FormerlySerializedAs("initialSpectrumShader")] [SerializeField]
        ComputeShader _initialSpectrumShader;
        [FormerlySerializedAs("timeDependentSpectrumShader")] [SerializeField]
        ComputeShader _timeDependentSpectrumShader;
        [FormerlySerializedAs("texturesMergerShader")] [SerializeField]
        ComputeShader _texturesMergerShader;

        Texture2D _gaussianNoise;
        FastFourierTransform _fft;
        Texture2D _physicsReadback;

        private void Awake()
        {
            Application.targetFrameRate = -1;
            _fft = new FastFourierTransform(_size, _fftShader);
            _gaussianNoise = GetNoiseTexture(_size);

            Cascade0 = new WavesCascade(_size, _initialSpectrumShader, _timeDependentSpectrumShader, _texturesMergerShader, _fft, _gaussianNoise);
            Cascade1 = new WavesCascade(_size, _initialSpectrumShader, _timeDependentSpectrumShader, _texturesMergerShader, _fft, _gaussianNoise);
            Cascade2 = new WavesCascade(_size, _initialSpectrumShader, _timeDependentSpectrumShader, _texturesMergerShader, _fft, _gaussianNoise);

            InitialiseCascades();

            _physicsReadback = new Texture2D(_size, _size, TextureFormat.RGBAFloat, false);
        }

        void InitialiseCascades()
        {
            float boundary1 = 2 * Mathf.PI / _lengthScale1 * 6f;
            float boundary2 = 2 * Mathf.PI / _lengthScale2 * 6f;
            Cascade0.CalculateInitials(_wavesSettings, _lengthScale0, 0.0001f, boundary1);
            Cascade1.CalculateInitials(_wavesSettings, _lengthScale1, boundary1, boundary2);
            Cascade2.CalculateInitials(_wavesSettings, _lengthScale2, boundary2, 9999);

            Shader.SetGlobalFloat("LengthScale0", _lengthScale0);
            Shader.SetGlobalFloat("LengthScale1", _lengthScale1);
            Shader.SetGlobalFloat("LengthScale2", _lengthScale2);
        }

        private void Update()
        {
            if (_alwaysRecalculateInitials)
            {
                InitialiseCascades();
            }

            Cascade0.CalculateWavesAtTime(Time.time);
            Cascade1.CalculateWavesAtTime(Time.time);
            Cascade2.CalculateWavesAtTime(Time.time);

            RequestReadbacks();
        }

        Texture2D GetNoiseTexture(int size)
        {
            string filename = "GaussianNoiseTexture" + size.ToString() + "x" + size.ToString();
            Texture2D noise = Resources.Load<Texture2D>("GaussianNoiseTextures/" + filename);
            return noise ? noise : GenerateNoiseTexture(size, true);
        }

        Texture2D GenerateNoiseTexture(int size, bool saveIntoAssetFile)
        {
            Texture2D noise = new Texture2D(size, size, TextureFormat.RGFloat, false, true);
            noise.filterMode = FilterMode.Point;
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    noise.SetPixel(i, j, new Vector4(NormalRandom(), NormalRandom()));
                }
            }
            noise.Apply();

#if UNITY_EDITOR
            if (saveIntoAssetFile)
            {
                string filename = "GaussianNoiseTexture" + size.ToString() + "x" + size.ToString();
                string path = "Assets/Resources/GaussianNoiseTextures/";
                AssetDatabase.CreateAsset(noise, path + filename + ".asset");
                Debug.Log("Texture \"" + filename + "\" was created at path \"" + path + "\".");
            }
#endif
            return noise;
        }

        float NormalRandom()
        {
            return Mathf.Cos(2 * Mathf.PI * Random.value) * Mathf.Sqrt(-2 * Mathf.Log(Random.value));
        }

        private void OnDestroy()
        {
            Cascade0.Dispose();
            Cascade1.Dispose();
            Cascade2.Dispose();
        }

        void RequestReadbacks()
        {
            AsyncGPUReadback.Request(Cascade0.Displacement, 0, TextureFormat.RGBAFloat, OnCompleteReadback);
        }

        public float GetWaterHeight(Vector3 position)
        {
            Vector3 displacement = GetWaterDisplacement(position);
            displacement = GetWaterDisplacement(position - displacement);
            displacement = GetWaterDisplacement(position - displacement);

            return GetWaterDisplacement(position - displacement).y;
        }

        public Vector3 GetWaterDisplacement(Vector3 position)
        {
            Color c = _physicsReadback.GetPixelBilinear(position.x / _lengthScale0, position.z / _lengthScale0);
            return new Vector3(c.r, c.g, c.b);
        }

        void OnCompleteReadback(AsyncGPUReadbackRequest request) => OnCompleteReadback(request, _physicsReadback);

        void OnCompleteReadback(AsyncGPUReadbackRequest request, Texture2D result)
        {
            if (request.hasError)
            {
                Debug.Log("GPU readback error detected.");
                return;
            }
            if (result != null)
            {
                result.LoadRawTextureData(request.GetData<Color>());
                result.Apply();
            }
        }
    }
}

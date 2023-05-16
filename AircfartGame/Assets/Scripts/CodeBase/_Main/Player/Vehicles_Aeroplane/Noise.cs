using UnityEngine;
using UnityEngine.Serialization;

namespace CodeBase._Main.Player.Vehicles_Aeroplane
{
	public static class Noise {

		public enum NormalizeMode {Local, Global};

		public static float[,] GenerateNoiseMap(int mapWidth, int mapHeight, NoiseSettings settings, Vector2 sampleCentre) {
			float[,] noiseMap = new float[mapWidth,mapHeight];

			System.Random prng = new System.Random (settings._seed);
			Vector2[] octaveOffsets = new Vector2[settings._octaves];

			float maxPossibleHeight = 0;
			float amplitude = 1;
			float frequency = 1;

			for (int i = 0; i < settings._octaves; i++) {
				float offsetX = prng.Next (-100000, 100000) + settings._offset.x + sampleCentre.x;
				float offsetY = prng.Next (-100000, 100000) - settings._offset.y - sampleCentre.y;
				octaveOffsets [i] = new Vector2 (offsetX, offsetY);

				maxPossibleHeight += amplitude;
				amplitude *= settings._persistance;
			}

			float maxLocalNoiseHeight = float.MinValue;
			float minLocalNoiseHeight = float.MaxValue;

			float halfWidth = mapWidth / 2f;
			float halfHeight = mapHeight / 2f;


			for (int y = 0; y < mapHeight; y++) {
				for (int x = 0; x < mapWidth; x++) {

					amplitude = 1;
					frequency = 1;
					float noiseHeight = 0;

					for (int i = 0; i < settings._octaves; i++) {
						float sampleX = (x-halfWidth + octaveOffsets[i].x) / settings._scale * frequency;
						float sampleY = (y-halfHeight + octaveOffsets[i].y) / settings._scale * frequency;

						float perlinValue = Mathf.PerlinNoise (sampleX, sampleY) * 2 - 1;
						noiseHeight += perlinValue * amplitude;

						amplitude *= settings._persistance;
						frequency *= settings._lacunarity;
					}

					if (noiseHeight > maxLocalNoiseHeight) {
						maxLocalNoiseHeight = noiseHeight;
					} 
					if (noiseHeight < minLocalNoiseHeight) {
						minLocalNoiseHeight = noiseHeight;
					}
					noiseMap [x, y] = noiseHeight;

					if (settings._normalizeMode == NormalizeMode.Global) {
						float normalizedHeight = (noiseMap [x, y] + 1) / (maxPossibleHeight / 0.9f);
						noiseMap [x, y] = Mathf.Clamp (normalizedHeight, 0, int.MaxValue);
					}
				}
			}

			if (settings._normalizeMode == NormalizeMode.Local) {
				for (int y = 0; y < mapHeight; y++) {
					for (int x = 0; x < mapWidth; x++) {
						noiseMap [x, y] = Mathf.InverseLerp (minLocalNoiseHeight, maxLocalNoiseHeight, noiseMap [x, y]);
					}
				}
			}

			return noiseMap;
		}

	}

	[System.Serializable]
	public class NoiseSettings {
		[FormerlySerializedAs("normalizeMode")] public Noise.NormalizeMode _normalizeMode;

		[FormerlySerializedAs("scale")] public float _scale = 50;

		[FormerlySerializedAs("octaves")] public int _octaves = 6;
		[FormerlySerializedAs("persistance")] [Range(0,1)]
		public float _persistance =.6f;
		[FormerlySerializedAs("lacunarity")] public float _lacunarity = 2;

		[FormerlySerializedAs("seed")] public int _seed;
		[FormerlySerializedAs("offset")] public Vector2 _offset;

		public void ValidateValues() {
			_scale = Mathf.Max (_scale, 0.01f);
			_octaves = Mathf.Max (_octaves, 1);
			_lacunarity = Mathf.Max (_lacunarity, 1);
			_persistance = Mathf.Clamp01 (_persistance);
		}
	}
}
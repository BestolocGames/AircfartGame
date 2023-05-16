using CodeBase._Main.Player.Vehicles_Aeroplane;
using Data;
using UnityEngine;

namespace CodeBase.MapGeneration
{
	public static class HeightMapGenerator {

		public static HeightMap GenerateHeightMap(int width, int height, HeightMapSettings settings, Vector2 sampleCentre) {
			float[,] values = Noise.GenerateNoiseMap (width, height, settings._noiseSettings, sampleCentre);

			AnimationCurve heightCurveThreadsafe = new AnimationCurve (settings._heightCurve.keys);

			float minValue = float.MaxValue;
			float maxValue = float.MinValue;

			for (int i = 0; i < width; i++) {
				for (int j = 0; j < height; j++) {
					values [i, j] *= heightCurveThreadsafe.Evaluate (values [i, j]) * settings._heightMultiplier;

					if (values [i, j] > maxValue) {
						maxValue = values [i, j];
					}
					if (values [i, j] < minValue) {
						minValue = values [i, j];
					}
				}
			}

			return new HeightMap (values, minValue, maxValue);
		}

	}

	public struct HeightMap {
		public readonly float[,] Values;
		public readonly float MinValue;
		public readonly float MaxValue;

		public HeightMap (float[,] values, float minValue, float maxValue)
		{
			this.Values = values;
			this.MinValue = minValue;
			this.MaxValue = maxValue;
		}
	}
}
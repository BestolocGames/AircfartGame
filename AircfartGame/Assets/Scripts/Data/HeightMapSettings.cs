using CodeBase._Main.Player.Vehicles_Aeroplane;
using UnityEngine;
using UnityEngine.Serialization;

namespace Data
{
	[CreateAssetMenu()]
	public class HeightMapSettings : UpdatableData {

		[FormerlySerializedAs("noiseSettings")] public NoiseSettings _noiseSettings;

		[FormerlySerializedAs("useFalloff")] public bool _useFalloff;

		[FormerlySerializedAs("heightMultiplier")] public float _heightMultiplier;
		[FormerlySerializedAs("heightCurve")] public AnimationCurve _heightCurve;

		public float MinHeight {
			get {
				return _heightMultiplier * _heightCurve.Evaluate (0);
			}
		}

		public float MaxHeight {
			get {
				return _heightMultiplier * _heightCurve.Evaluate (1);
			}
		}

#if UNITY_EDITOR

		protected override void OnValidate() {
			_noiseSettings.ValidateValues ();
			base.OnValidate ();
		}
#endif

	}
}

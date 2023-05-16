using UnityEngine;
using UnityEngine.Serialization;

namespace Data
{
	[CreateAssetMenu()]
	public class MeshSettings : UpdatableData {

		public const int NumSupportedLoDs = 5;
		public const int NumSupportedChunkSizes = 9;
		public const int NumSupportedFlatshadedChunkSizes = 3;
		public static readonly int[] SupportedChunkSizes = {48,72,96,120,144,168,192,216,240};
	
		[FormerlySerializedAs("meshScale")] public float _meshScale = 2.5f;
		[FormerlySerializedAs("useFlatShading")] public bool _useFlatShading;

		[FormerlySerializedAs("chunkSizeIndex")] [Range(0,NumSupportedChunkSizes-1)]
		public int _chunkSizeIndex;
		[FormerlySerializedAs("flatshadedChunkSizeIndex")] [Range(0,NumSupportedFlatshadedChunkSizes-1)]
		public int _flatshadedChunkSizeIndex;


		// num verts per line of mesh rendered at LOD = 0. Includes the 2 extra verts that are excluded from final mesh, but used for calculating normals
		public int NumVertsPerLine {
			get {
				return SupportedChunkSizes [(_useFlatShading) ? _flatshadedChunkSizeIndex : _chunkSizeIndex] + 5;
			}
		}

		public float MeshWorldSize {
			get {
				return (NumVertsPerLine - 3) * _meshScale;
			}
		}


	}
}

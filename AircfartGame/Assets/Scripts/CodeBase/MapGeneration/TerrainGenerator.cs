using System.Collections.Generic;
using Data;
using UnityEngine;
using UnityEngine.Serialization;

namespace CodeBase.MapGeneration
{
	public class TerrainGenerator : MonoBehaviour {

		const float ViewerMoveThresholdForChunkUpdate = 25f;
		const float SqrViewerMoveThresholdForChunkUpdate = ViewerMoveThresholdForChunkUpdate * ViewerMoveThresholdForChunkUpdate;


		[FormerlySerializedAs("colliderLODIndex")] public int _colliderLODIndex;
		[FormerlySerializedAs("detailLevels")] public LODInfo[] _detailLevels;

		[FormerlySerializedAs("meshSettings")] public MeshSettings _meshSettings;
		[FormerlySerializedAs("heightMapSettings")] public HeightMapSettings _heightMapSettings;
		[FormerlySerializedAs("textureSettings")] public TextureData _textureSettings;

		[FormerlySerializedAs("viewer")] public Transform _viewer;
		[FormerlySerializedAs("mapMaterial")] public Material _mapMaterial;

		Vector2 _viewerPosition;
		Vector2 _viewerPositionOld;

		float _meshWorldSize;
		int _chunksVisibleInViewDst;

		Dictionary<Vector2, TerrainChunk> _terrainChunkDictionary = new Dictionary<Vector2, TerrainChunk>();
		List<TerrainChunk> _visibleTerrainChunks = new List<TerrainChunk>();

		void Start() {

			_textureSettings.ApplyToMaterial (_mapMaterial);
			_textureSettings.UpdateMeshHeights (_mapMaterial, _heightMapSettings.MinHeight, _heightMapSettings.MaxHeight);

			float maxViewDst = _detailLevels [_detailLevels.Length - 1]._visibleDstThreshold;
			_meshWorldSize = _meshSettings.MeshWorldSize;
			_chunksVisibleInViewDst = Mathf.RoundToInt(maxViewDst / _meshWorldSize);

			UpdateVisibleChunks ();
		}

		void Update() {
			_viewerPosition = new Vector2 (_viewer.position.x, _viewer.position.z);

			if (_viewerPosition != _viewerPositionOld) {
				foreach (TerrainChunk chunk in _visibleTerrainChunks) {
					chunk.UpdateCollisionMesh ();
				}
			}

			if ((_viewerPositionOld - _viewerPosition).sqrMagnitude > SqrViewerMoveThresholdForChunkUpdate) {
				_viewerPositionOld = _viewerPosition;
				UpdateVisibleChunks ();
			}
		}
		
		void UpdateVisibleChunks() {
			HashSet<Vector2> alreadyUpdatedChunkCoords = new HashSet<Vector2> ();
			for (int i = _visibleTerrainChunks.Count-1; i >= 0; i--) {
				alreadyUpdatedChunkCoords.Add (_visibleTerrainChunks [i].Coord);
				_visibleTerrainChunks [i].UpdateTerrainChunk ();
			}
			
			int currentChunkCoordX = Mathf.RoundToInt (_viewerPosition.x / _meshWorldSize);
			int currentChunkCoordY = Mathf.RoundToInt (_viewerPosition.y / _meshWorldSize);

			for (int yOffset = -_chunksVisibleInViewDst; yOffset <= _chunksVisibleInViewDst; yOffset++) {
				for (int xOffset = -_chunksVisibleInViewDst; xOffset <= _chunksVisibleInViewDst; xOffset++) {
					Vector2 viewedChunkCoord = new Vector2 (currentChunkCoordX + xOffset, currentChunkCoordY + yOffset);
					if (!alreadyUpdatedChunkCoords.Contains (viewedChunkCoord)) {
						if (_terrainChunkDictionary.ContainsKey (viewedChunkCoord)) {
							_terrainChunkDictionary [viewedChunkCoord].UpdateTerrainChunk ();
						} else {
							TerrainChunk newChunk = new TerrainChunk (viewedChunkCoord,_heightMapSettings,_meshSettings, _detailLevels, _colliderLODIndex, transform, _viewer, _mapMaterial);
							_terrainChunkDictionary.Add (viewedChunkCoord, newChunk);
							newChunk.OnOnVisibilityChanged += OnTerrainChunkVisibilityChanged;
							newChunk.Load ();
						}
					}

				}
			}
		}

		void OnTerrainChunkVisibilityChanged(TerrainChunk chunk, bool isVisible) {
			if (isVisible) {
				_visibleTerrainChunks.Add (chunk);
			} else {
				_visibleTerrainChunks.Remove (chunk);
			}
		}

	}

	[System.Serializable]
	public struct LODInfo {
		[FormerlySerializedAs("lod")] [Range(0,MeshSettings.NumSupportedLoDs-1)]
		public int _lod;
		[FormerlySerializedAs("visibleDstThreshold")] public float _visibleDstThreshold;


		public float SqrVisibleDstThreshold {
			get {
				return _visibleDstThreshold * _visibleDstThreshold;
			}
		}
	}
}
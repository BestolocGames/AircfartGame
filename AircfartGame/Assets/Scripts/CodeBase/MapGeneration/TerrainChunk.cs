using Data;
using UnityEngine;

namespace CodeBase.MapGeneration
{
	public class TerrainChunk {
	
		const float ColliderGenerationDistanceThreshold = 5;
		public event System.Action<TerrainChunk, bool> OnOnVisibilityChanged;
		public Vector2 Coord;
	 
		GameObject _meshObject;
		Vector2 _sampleCentre;
		Bounds _bounds;

		MeshRenderer _meshRenderer;
		MeshFilter _meshFilter;
		MeshCollider _meshCollider;

		LODInfo[] _detailLevels;
		LODMesh[] _lodMeshes;
		int _colliderLODIndex;

		HeightMap _heightMap;
		bool _heightMapReceived;
		int _previousLODIndex = -1;
		bool _hasSetCollider;
		float _maxViewDst;

		HeightMapSettings _heightMapSettings;
		MeshSettings _meshSettings;
		Transform _viewer;

		public TerrainChunk(Vector2 coord, HeightMapSettings heightMapSettings, MeshSettings meshSettings, LODInfo[] detailLevels, int colliderLODIndex, Transform parent, Transform viewer, Material material) {
			this.Coord = coord;
			this._detailLevels = detailLevels;
			this._colliderLODIndex = colliderLODIndex;
			this._heightMapSettings = heightMapSettings;
			this._meshSettings = meshSettings;
			this._viewer = viewer;

			_sampleCentre = coord * meshSettings.MeshWorldSize / meshSettings._meshScale;
			Vector2 position = coord * meshSettings.MeshWorldSize ;
			_bounds = new Bounds(position,Vector2.one * meshSettings.MeshWorldSize );


			_meshObject = new GameObject("Terrain Chunk");
			_meshRenderer = _meshObject.AddComponent<MeshRenderer>();
			_meshFilter = _meshObject.AddComponent<MeshFilter>();
			_meshCollider = _meshObject.AddComponent<MeshCollider>();
			_meshRenderer.material = material;

			_meshObject.transform.position = new Vector3(position.x, 0, position.y);
			_meshObject.transform.parent = parent;
			SetVisible(false);

			_lodMeshes = new LODMesh[detailLevels.Length];
			for (int i = 0; i < detailLevels.Length; i++) {
				_lodMeshes[i] = new LODMesh(detailLevels[i]._lod);
				_lodMeshes[i].OnUpdateCallback += UpdateTerrainChunk;
				if (i == colliderLODIndex) {
					_lodMeshes[i].OnUpdateCallback += UpdateCollisionMesh;
				}
			}

			_maxViewDst = detailLevels [detailLevels.Length - 1]._visibleDstThreshold;

		}

		public void Load() {
			ThreadedDataRequester.RequestData(() => HeightMapGenerator.GenerateHeightMap (_meshSettings.NumVertsPerLine, _meshSettings.NumVertsPerLine, _heightMapSettings, _sampleCentre), OnHeightMapReceived);
		}



		void OnHeightMapReceived(object heightMapObject) {
			_heightMap = (HeightMap)heightMapObject;
			_heightMapReceived = true;

			UpdateTerrainChunk ();
		}

		Vector2 ViewerPosition {
			get {
				return new Vector2 (_viewer.position.x, _viewer.position.z);
			}
		}


		public void UpdateTerrainChunk() {
			if (_heightMapReceived) {
				float viewerDstFromNearestEdge = Mathf.Sqrt (_bounds.SqrDistance (ViewerPosition));

				bool wasVisible = IsVisible ();
				bool visible = viewerDstFromNearestEdge <= _maxViewDst;

				if (visible) {
					int lodIndex = 0;

					for (int i = 0; i < _detailLevels.Length - 1; i++) {
						if (viewerDstFromNearestEdge > _detailLevels [i]._visibleDstThreshold) {
							lodIndex = i + 1;
						} else {
							break;
						}
					}

					if (lodIndex != _previousLODIndex) {
						LODMesh lodMesh = _lodMeshes [lodIndex];
						if (lodMesh.HasMesh) {
							_previousLODIndex = lodIndex;
							_meshFilter.mesh = lodMesh.Mesh;
						} else if (!lodMesh.HasRequestedMesh) {
							lodMesh.RequestMesh (_heightMap, _meshSettings);
						}
					}


				}

				if (wasVisible != visible) {
				
					SetVisible (visible);
					if (OnOnVisibilityChanged != null) {
						OnOnVisibilityChanged (this, visible);
					}
				}
			}
		}

		public void UpdateCollisionMesh() {
			if (!_hasSetCollider) {
				float sqrDstFromViewerToEdge = _bounds.SqrDistance (ViewerPosition);

				if (sqrDstFromViewerToEdge < _detailLevels [_colliderLODIndex].SqrVisibleDstThreshold) {
					if (!_lodMeshes [_colliderLODIndex].HasRequestedMesh) {
						_lodMeshes [_colliderLODIndex].RequestMesh (_heightMap, _meshSettings);
					}
				}

				if (sqrDstFromViewerToEdge < ColliderGenerationDistanceThreshold * ColliderGenerationDistanceThreshold) {
					if (_lodMeshes [_colliderLODIndex].HasMesh) {
						_meshCollider.sharedMesh = _lodMeshes [_colliderLODIndex].Mesh;
						_hasSetCollider = true;
					}
				}
			}
		}

		public void SetVisible(bool visible) {
			_meshObject.SetActive (visible);
		}

		public bool IsVisible() {
			return _meshObject.activeSelf;
		}

	}

	class LODMesh {

		public Mesh Mesh;
		public bool HasRequestedMesh;
		public bool HasMesh;
		int _lod;
		public event System.Action OnUpdateCallback;

		public LODMesh(int lod) {
			this._lod = lod;
		}

		void OnMeshDataReceived(object meshDataObject) {
			Mesh = ((MeshData)meshDataObject).CreateMesh ();
			HasMesh = true;

			OnUpdateCallback ();
		}

		public void RequestMesh(HeightMap heightMap, MeshSettings meshSettings) {
			HasRequestedMesh = true;
			ThreadedDataRequester.RequestData (() => MeshGenerator.GenerateTerrainMesh (heightMap.Values, meshSettings, _lod), OnMeshDataReceived);
		}

	}
}
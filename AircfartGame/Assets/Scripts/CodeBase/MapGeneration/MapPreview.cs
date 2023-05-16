using Data;
using UnityEngine;
using UnityEngine.Serialization;

namespace CodeBase.MapGeneration
{
	public class MapPreview : MonoBehaviour {

		[FormerlySerializedAs("textureRender")] public Renderer _textureRender;
		[FormerlySerializedAs("meshFilter")] public MeshFilter _meshFilter;
		[FormerlySerializedAs("meshRenderer")] public MeshRenderer _meshRenderer;

		public enum DrawMode {NoiseMap, Mesh, FalloffMap};
		[FormerlySerializedAs("drawMode")] public DrawMode _drawMode;

		[FormerlySerializedAs("meshSettings")] public MeshSettings _meshSettings;
		[FormerlySerializedAs("heightMapSettings")] public HeightMapSettings _heightMapSettings;
		[FormerlySerializedAs("textureData")] public TextureData _textureData;

		[FormerlySerializedAs("terrainMaterial")] public Material _terrainMaterial;



		[FormerlySerializedAs("editorPreviewLOD")] [Range(0,MeshSettings.NumSupportedLoDs-1)]
		public int _editorPreviewLOD;
		[FormerlySerializedAs("autoUpdate")] public bool _autoUpdate;




		public void DrawMapInEditor() {
			_textureData.ApplyToMaterial (_terrainMaterial);
			_textureData.UpdateMeshHeights (_terrainMaterial, _heightMapSettings.MinHeight, _heightMapSettings.MaxHeight);
			HeightMap heightMap = HeightMapGenerator.GenerateHeightMap (_meshSettings.NumVertsPerLine, _meshSettings.NumVertsPerLine, _heightMapSettings, Vector2.zero);

			if (_drawMode == DrawMode.NoiseMap) {
				DrawTexture (TextureGenerator.TextureFromHeightMap (heightMap));
			} else if (_drawMode == DrawMode.Mesh) {
				DrawMesh (MeshGenerator.GenerateTerrainMesh (heightMap.Values,_meshSettings, _editorPreviewLOD));
			} else if (_drawMode == DrawMode.FalloffMap) {
				DrawTexture(TextureGenerator.TextureFromHeightMap(new HeightMap(FalloffGenerator.GenerateFalloffMap(_meshSettings.NumVertsPerLine),0,1)));
			}
		}





		public void DrawTexture(Texture2D texture) {
			_textureRender.sharedMaterial.mainTexture = texture;
			_textureRender.transform.localScale = new Vector3 (texture.width, 1, texture.height) /10f;

			_textureRender.gameObject.SetActive (true);
			_meshFilter.gameObject.SetActive (false);
		}

		public void DrawMesh(MeshData meshData) {
			_meshFilter.sharedMesh = meshData.CreateMesh ();

			_textureRender.gameObject.SetActive (false);
			_meshFilter.gameObject.SetActive (true);
		}



		void OnValuesUpdated() {
			if (!Application.isPlaying) {
				DrawMapInEditor ();
			}
		}

		void OnTextureValuesUpdated() {
			_textureData.ApplyToMaterial (_terrainMaterial);
		}

		void OnValidate() {

			if (_meshSettings != null) {
				_meshSettings.OnValuesUpdated -= OnValuesUpdated;
				_meshSettings.OnValuesUpdated += OnValuesUpdated;
			}
			if (_heightMapSettings != null) {
				_heightMapSettings.OnValuesUpdated -= OnValuesUpdated;
				_heightMapSettings.OnValuesUpdated += OnValuesUpdated;
			}
			if (_textureData != null) {
				_textureData.OnValuesUpdated -= OnTextureValuesUpdated;
				_textureData.OnValuesUpdated += OnTextureValuesUpdated;
			}

		}

	}
}

using UnityEngine;
using UnityEngine.Serialization;

namespace CodeBase.MapGeneration
{
    public class MapDisplay : MonoBehaviour
    {
        [FormerlySerializedAs("textureRenderer")] public Renderer _textureRenderer;
        [FormerlySerializedAs("meshFilter")] public MeshFilter _meshFilter;
        [FormerlySerializedAs("meshRenderer")] public MeshRenderer _meshRenderer;

        public void DrawTexture(Texture2D texture)
        {
            _textureRenderer.sharedMaterial.mainTexture = texture;
            _textureRenderer.transform.localScale = new Vector3(texture.width, 1, texture.height);
        }

        public void DrawMesh(MeshData meshData)
        {
            _meshFilter.sharedMesh = meshData.CreateMesh();
            _meshFilter.transform.localScale = Vector3.one * FindObjectOfType<MapPreview>()._meshSettings._meshScale;
        }
    }
}

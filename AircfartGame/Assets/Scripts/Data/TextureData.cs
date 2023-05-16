using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;

namespace Data
{
	[CreateAssetMenu()]
	public class TextureData : UpdatableData {

		const int TextureSize = 512;
		const TextureFormat TextureFormat = UnityEngine.TextureFormat.RGB565;

		[FormerlySerializedAs("layers")] public Layer[] _layers;

		float _savedMinHeight;
		float _savedMaxHeight;

		public void ApplyToMaterial(Material material) {
		
			material.SetInt ("layerCount", _layers.Length);
			material.SetColorArray ("baseColours", _layers.Select(x => x._tint).ToArray());
			material.SetFloatArray ("baseStartHeights", _layers.Select(x => x._startHeight).ToArray());
			material.SetFloatArray ("baseBlends", _layers.Select(x => x._blendStrength).ToArray());
			material.SetFloatArray ("baseColourStrength", _layers.Select(x => x._tintStrength).ToArray());
			material.SetFloatArray ("baseTextureScales", _layers.Select(x => x._textureScale).ToArray());
			Texture2DArray texturesArray = GenerateTextureArray (_layers.Select (x => x._texture).ToArray ());
			material.SetTexture ("baseTextures", texturesArray);

			UpdateMeshHeights (material, _savedMinHeight, _savedMaxHeight);
		}

		public void UpdateMeshHeights(Material material, float minHeight, float maxHeight) {
			_savedMinHeight = minHeight;
			_savedMaxHeight = maxHeight;

			material.SetFloat ("minHeight", minHeight);
			material.SetFloat ("maxHeight", maxHeight);
		}

		Texture2DArray GenerateTextureArray(Texture2D[] textures) {
			Texture2DArray textureArray = new Texture2DArray (TextureSize, TextureSize, textures.Length, TextureFormat, true);
			for (int i = 0; i < textures.Length; i++) {
				textureArray.SetPixels (textures [i].GetPixels (), i);
			}
			textureArray.Apply ();
			return textureArray;
		}

		[System.Serializable]
		public class Layer {
			[FormerlySerializedAs("texture")] public Texture2D _texture;
			[FormerlySerializedAs("tint")] public Color _tint;
			[FormerlySerializedAs("tintStrength")] [Range(0,1)]
			public float _tintStrength;
			[FormerlySerializedAs("startHeight")] [Range(0,1)]
			public float _startHeight;
			[FormerlySerializedAs("blendStrength")] [Range(0,1)]
			public float _blendStrength;
			[FormerlySerializedAs("textureScale")] public float _textureScale;
		}
		
	 
	}
}

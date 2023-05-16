using UnityEngine;
using UnityEngine.Serialization;

namespace CodeBase._ImageEffects
{
	[RequireComponent(typeof(Camera))]
	[AddComponentMenu("")]
	public class ImageEffectBase : MonoBehaviour
	{
		protected virtual void Start()
		{
			if (!SystemInfo.supportsImageEffects)
			{
				enabled = false;
				return;
			}
			if (!_shader || !_shader.isSupported)
			{
				enabled = false;
			}
		}

		protected Material Material
		{
			get
			{
				if (_mMaterial == null)
				{
					_mMaterial = new Material(_shader);
					_mMaterial.hideFlags = HideFlags.HideAndDontSave;
				}
				return _mMaterial;
			}
		}

		protected virtual void OnDisable()
		{
			if (_mMaterial)
			{
				DestroyImmediate(_mMaterial);
			}
		}

		[FormerlySerializedAs("shader")] public Shader _shader;

		private Material _mMaterial;
	}
}

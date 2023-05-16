using UnityEngine;
using UnityEngine.Serialization;

namespace CodeBase._Main
{
	public class CrossFadeCanvasGroups : MonoBehaviour
	{
		public virtual void Activate()
		{
			_toGroup.gameObject.SetActive(true);
			_toGroup.alpha = 0f;
			_toGroup.interactable = true;
			_fromGroup.interactable = false;
			StartCoroutine(Fader.FadeAlpha(_fromGroup, false, _speed, delegate()
			{
				_fromGroup.gameObject.SetActive(false);
			}));
			StartCoroutine(Fader.FadeAlpha(_toGroup, true, _speed, null));
		}

		[FormerlySerializedAs("fromGroup")] public CanvasGroup _fromGroup;

		[FormerlySerializedAs("toGroup")] public CanvasGroup _toGroup;

		[FormerlySerializedAs("speed")] public float _speed = 1f;
	}
}

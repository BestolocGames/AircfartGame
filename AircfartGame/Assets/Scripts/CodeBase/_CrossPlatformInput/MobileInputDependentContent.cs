using UnityEngine;

namespace CodeBase._CrossPlatformInput
{
	public class MobileInputDependentContent : MonoBehaviour
	{
		[SerializeField]
		private InputMode _inputMode;

		[SerializeField]
		private GameObject[] _content = new GameObject[0];

		[SerializeField]
		private MonoBehaviour[] _monoBehaviours = new MonoBehaviour[0];

		[SerializeField]
		private bool _childrenOfThisObject;

		private enum InputMode
		{
			StandaloneInput,
			MobileInput
		}
		
		private void OnEnable()
		{
			CheckEnableContent();
		}

		private void CheckEnableContent()
		{
			if (_inputMode == InputMode.MobileInput)
			{
				EnableContent(true);
			}
			else
			{
				EnableContent(false);
			}
		}

		private void EnableContent(bool enabled)
		{
			if (_content.Length > 0)
			{
				foreach (GameObject gameObject in _content)
				{
					if (gameObject != null)
					{
						gameObject.SetActive(enabled);
					}
				}
			}
			if (_childrenOfThisObject)
			{
				foreach (object obj in this.transform)
				{
					Transform transform = (Transform)obj;
					transform.gameObject.SetActive(enabled);
				}
			}
			if (_monoBehaviours.Length > 0)
			{
				foreach (MonoBehaviour monoBehaviour in _monoBehaviours)
				{
					monoBehaviour.enabled = enabled;
				}
			}
		}


	}
}

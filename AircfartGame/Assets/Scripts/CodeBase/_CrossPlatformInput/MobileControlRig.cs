using UnityEngine;
using UnityEngine.EventSystems;

namespace CodeBase._CrossPlatformInput
{
	[ExecuteInEditMode]
	public class MobileControlRig : MonoBehaviour
	{
		private void OnEnable()
		{
			CheckEnableControlRig();
		}

		private void Start()
		{
			EventSystem x = FindObjectOfType<EventSystem>();
			if (x == null)
			{
				GameObject gameObject = new GameObject("EventSystem");
				gameObject.AddComponent<EventSystem>();
				gameObject.AddComponent<StandaloneInputModule>();
			}
		}

		private void CheckEnableControlRig()
		{
			EnableControlRig(true);
		}

		private void EnableControlRig(bool enabled)
		{
			foreach (object obj in this.transform)
			{
				Transform transform = (Transform)obj;
				transform.gameObject.SetActive(enabled);
			}
		}
	}
}

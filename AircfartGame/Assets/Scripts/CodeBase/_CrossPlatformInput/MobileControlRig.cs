// dnSpy decompiler from Assembly-CSharp.dll class: UnityStandardAssets.CrossPlatformInput.MobileControlRig
using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace UnityStandardAssets.CrossPlatformInput
{
	[ExecuteInEditMode]
	public class MobileControlRig : MonoBehaviour
	{
		private void OnEnable()
		{
			this.CheckEnableControlRig();
		}

		private void Start()
		{
			EventSystem x = UnityEngine.Object.FindObjectOfType<EventSystem>();
			if (x == null)
			{
				GameObject gameObject = new GameObject("EventSystem");
				gameObject.AddComponent<EventSystem>();
				gameObject.AddComponent<StandaloneInputModule>();
			}
		}

		private void CheckEnableControlRig()
		{
			this.EnableControlRig(true);
		}

		private void EnableControlRig(bool enabled)
		{
			foreach (object obj in base.transform)
			{
				Transform transform = (Transform)obj;
				transform.gameObject.SetActive(enabled);
			}
		}
	}
}

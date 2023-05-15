// dnSpy decompiler from Assembly-CSharp.dll class: FlightKit.MobileInputDependentContent
using System;
using UnityEngine;

namespace FlightKit
{
	public class MobileInputDependentContent : MonoBehaviour
	{
		private void OnEnable()
		{
			this.CheckEnableContent();
		}

		private void CheckEnableContent()
		{
			if (this._inputMode == MobileInputDependentContent.InputMode.MobileInput)
			{
				this.EnableContent(true);
			}
			else
			{
				this.EnableContent(false);
			}
		}

		private void EnableContent(bool enabled)
		{
			if (this._content.Length > 0)
			{
				foreach (GameObject gameObject in this._content)
				{
					if (gameObject != null)
					{
						gameObject.SetActive(enabled);
					}
				}
			}
			if (this._childrenOfThisObject)
			{
				foreach (object obj in base.transform)
				{
					Transform transform = (Transform)obj;
					transform.gameObject.SetActive(enabled);
				}
			}
			if (this._monoBehaviours.Length > 0)
			{
				foreach (MonoBehaviour monoBehaviour in this._monoBehaviours)
				{
					monoBehaviour.enabled = enabled;
				}
			}
		}

		[SerializeField]
		private MobileInputDependentContent.InputMode _inputMode;

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
	}
}

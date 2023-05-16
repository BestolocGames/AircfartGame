using CodeBase._CrossPlatformInput;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace UI
{
	[RequireComponent(typeof(Button))]
	public class KeyButton : MonoBehaviour
	{
		private Button Button { get; set; }

		private void Awake()
		{
			Button = GetComponent<Button>();
		}

		private void Update()
		{
			if (!Button.interactable)
			{
				return;
			}
			bool flag = false;
			foreach (string name in _axis)
			{
				if (CrossPlatformInputManager.GetButtonDown(name))
				{
					flag = true;
					break;
				}
			}
			foreach (KeyCode key in _keys)
			{
				if (Input.GetKeyDown(key))
				{
					flag = true;
					break;
				}
			}
			if (flag)
			{
				Button.onClick.Invoke();
			}
		}

		[FormerlySerializedAs("axis")] public string[] _axis;

		[FormerlySerializedAs("keys")] public KeyCode[] _keys;
	}
}

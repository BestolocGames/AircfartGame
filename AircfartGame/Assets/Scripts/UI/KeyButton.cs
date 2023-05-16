using CodeBase._CrossPlatformInput;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
	[RequireComponent(typeof(Button))]
	public class KeyButton : MonoBehaviour
	{
		public Button button { get; private set; }

		private void Awake()
		{
			button = GetComponent<Button>();
		}

		private void Update()
		{
			if (!button.interactable)
			{
				return;
			}
			bool flag = false;
			foreach (string name in axis)
			{
				if (CrossPlatformInputManager.GetButtonDown(name))
				{
					flag = true;
					break;
				}
			}
			foreach (KeyCode key in keys)
			{
				if (Input.GetKeyDown(key))
				{
					flag = true;
					break;
				}
			}
			if (flag)
			{
				button.onClick.Invoke();
			}
		}

		public string[] axis;

		public KeyCode[] keys;
	}
}

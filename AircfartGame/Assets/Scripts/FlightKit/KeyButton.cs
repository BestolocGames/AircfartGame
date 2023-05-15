// dnSpy decompiler from Assembly-CSharp.dll class: FlightKit.KeyButton
using System;
using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets.CrossPlatformInput;

namespace FlightKit
{
	[RequireComponent(typeof(Button))]
	public class KeyButton : MonoBehaviour
	{
		public Button button { get; private set; }

		private void Awake()
		{
			this.button = base.GetComponent<Button>();
		}

		private void Update()
		{
			if (!this.button.interactable)
			{
				return;
			}
			bool flag = false;
			foreach (string name in this.axis)
			{
				if (CrossPlatformInputManager.GetButtonDown(name))
				{
					flag = true;
					break;
				}
			}
			foreach (KeyCode key in this.keys)
			{
				if (UnityEngine.Input.GetKeyDown(key))
				{
					flag = true;
					break;
				}
			}
			if (flag)
			{
				this.button.onClick.Invoke();
			}
		}

		public string[] axis;

		public KeyCode[] keys;
	}
}

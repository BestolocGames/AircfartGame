// dnSpy decompiler from Assembly-CSharp.dll class: FlightKit.SoundButton
using System;
using UnityEngine;

namespace FlightKit
{
	public class SoundButton : MonoBehaviour
	{
		public virtual void TurnOff()
		{
			AudioListener.volume = 0f;
		}

		public virtual void TurnOn()
		{
			AudioListener.volume = 1f;
		}
	}
}

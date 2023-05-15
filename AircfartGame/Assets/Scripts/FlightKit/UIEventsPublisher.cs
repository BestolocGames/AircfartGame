// dnSpy decompiler from Assembly-CSharp.dll class: FlightKit.UIEventsPublisher
using System;
using UnityEngine;

namespace FlightKit
{
	public class UIEventsPublisher : MonoBehaviour
	{
		public static event GameActions.SimpleAction OnPlayEvent;

		public virtual void PublishPlay()
		{
			if (UIEventsPublisher.OnPlayEvent != null)
			{
				UIEventsPublisher.OnPlayEvent();
			}
		}
	}
}

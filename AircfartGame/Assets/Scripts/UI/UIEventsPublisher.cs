using UnityEngine;

namespace UI
{
	public class UIEventsPublisher : MonoBehaviour
	{
		public static event GameActions.SimpleAction OnPlayEvent;

		public virtual void PublishPlay()
		{
			if (OnPlayEvent != null)
			{
				OnPlayEvent();
			}
		}
	}
}

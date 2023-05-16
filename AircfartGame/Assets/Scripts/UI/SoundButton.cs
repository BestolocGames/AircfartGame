using UnityEngine;

namespace UI
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

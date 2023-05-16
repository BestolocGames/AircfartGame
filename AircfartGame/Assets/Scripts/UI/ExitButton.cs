using UnityEngine;

namespace UI
{
	public sealed class ExitButton : MonoBehaviour
	{
		public void Activate() => 
			Application.Quit();
	}
}

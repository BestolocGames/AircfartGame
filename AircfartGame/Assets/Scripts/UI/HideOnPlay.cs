using UnityEngine;

namespace UI
{
	public class HideOnPlay : MonoBehaviour {
		private void Start () {
			gameObject.SetActive (false);
		}
	}
}

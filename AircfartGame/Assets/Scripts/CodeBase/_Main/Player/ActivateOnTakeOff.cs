using UnityEngine;

namespace CodeBase._Main.Player
{
	public class ActivateOnTakeOff : MonoBehaviour
	{
		public float delay;

		public GameObject[] objectsToActivate;
		
		private void OnEnable() => 
			TakeOffPublisher.OnTakeOffEvent += OnTakeOff;

		private void OnDisable() => 
			TakeOffPublisher.OnTakeOffEvent -= OnTakeOff;

		private void OnTakeOff() => 
			Invoke("OnTakeOffCore", delay);

		private void OnTakeOffCore()
		{
			foreach (GameObject gameObject in objectsToActivate)
			{
				if (gameObject != null) 
					gameObject.SetActive(true);
			}
		}
	}
}

using UnityEngine;
using UnityEngine.Serialization;

namespace CodeBase._Main.Player
{
	public class ActivateOnTakeOff : MonoBehaviour
	{
		[FormerlySerializedAs("delay")] public float _delay;

		[FormerlySerializedAs("objectsToActivate")] public GameObject[] _objectsToActivate;
		
		private void OnEnable() => 
			TakeOffPublisher.OnTakeOffEvent += OnTakeOff;

		private void OnDisable() => 
			TakeOffPublisher.OnTakeOffEvent -= OnTakeOff;

		private void OnTakeOff() => 
			Invoke("OnTakeOffCore", _delay);

		private void OnTakeOffCore()
		{
			foreach (GameObject gameObject in _objectsToActivate)
			{
				if (gameObject != null) 
					gameObject.SetActive(true);
			}
		}
	}
}

using UnityEngine;
using UnityEngine.Serialization;

namespace CodeBase._Main.Player
{
	public class AirplaneTrails : MonoBehaviour
	{
		[FormerlySerializedAs("trailsContainer")] public GameObject _trailsContainer;

		public virtual void ActivateTrails()
		{
			if (_trailsContainer != null) 
				_trailsContainer.SetActive(true);
		}

		public virtual void DeactivateTrails()
		{
			if (_trailsContainer != null) 
				_trailsContainer.SetActive(false);
		}

		public virtual void ClearTrails()
		{
			if (_trailsContainer != null)
			{
				TrailRenderer[] componentsInChildren = _trailsContainer.GetComponentsInChildren<TrailRenderer>();
				foreach (TrailRenderer trailRenderer in componentsInChildren) 
					trailRenderer.Clear();
			}
		}
	}
}

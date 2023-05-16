using UnityEngine;

namespace CodeBase._Main.Player
{
	public class AirplaneTrails : MonoBehaviour
	{
		public GameObject trailsContainer;

		public virtual void ActivateTrails()
		{
			if (trailsContainer != null) 
				trailsContainer.SetActive(true);
		}

		public virtual void DeactivateTrails()
		{
			if (trailsContainer != null) 
				trailsContainer.SetActive(false);
		}

		public virtual void ClearTrails()
		{
			if (trailsContainer != null)
			{
				TrailRenderer[] componentsInChildren = trailsContainer.GetComponentsInChildren<TrailRenderer>();
				foreach (TrailRenderer trailRenderer in componentsInChildren) 
					trailRenderer.Clear();
			}
		}
	}
}

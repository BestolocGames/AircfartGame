// dnSpy decompiler from Assembly-CSharp.dll class: FlightKit.AirplaneTrails
using System;
using UnityEngine;

namespace FlightKit
{
	public class AirplaneTrails : MonoBehaviour
	{
		public virtual void ActivateTrails()
		{
			if (this.trailsContainer != null)
			{
				this.trailsContainer.SetActive(true);
			}
		}

		public virtual void DeactivateTrails()
		{
			if (this.trailsContainer != null)
			{
				this.trailsContainer.SetActive(false);
			}
		}

		public virtual void ClearTrails()
		{
			if (this.trailsContainer != null)
			{
				TrailRenderer[] componentsInChildren = this.trailsContainer.GetComponentsInChildren<TrailRenderer>();
				foreach (TrailRenderer trailRenderer in componentsInChildren)
				{
					trailRenderer.Clear();
				}
			}
		}

		public GameObject trailsContainer;
	}
}

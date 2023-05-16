using CodeBase._Main.Player.Vehicles_Aeroplane;
using UnityEngine;

namespace CodeBase._Main
{
	[RequireComponent(typeof(AeroplaneController))]
	public class SimplePropellerAnimator : MonoBehaviour
	{
		private void Awake()
		{
			_airplane = GetComponent<AeroplaneController>();
		}

		private void Update()
		{
			if (!_airplane || !propellerModel)
			{
				return;
			}
			float num = maxRpm * _airplane.Throttle * Time.deltaTime * 60f;
			if (rotateAroundX)
			{
				propellerModel.Rotate(num, 0f, 0f);
			}
			else
			{
				propellerModel.Rotate(0f, num, 0f);
			}
		}

		private const float RPM_TO_DPS = 60f;

		public Transform propellerModel;

		public float maxRpm = 2000f;

		public bool rotateAroundX;

		private AeroplaneController _airplane;

		private Renderer _propellerModelRenderer;
	}
}

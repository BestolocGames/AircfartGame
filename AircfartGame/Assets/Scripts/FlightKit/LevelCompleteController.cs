// dnSpy decompiler from Assembly-CSharp.dll class: FlightKit.LevelCompleteController
using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace FlightKit
{
	public class LevelCompleteController : MonoBehaviour
	{
		public virtual void HandleLevelComplete()
		{
			SceneManager.LoadScene(SceneManager.GetActiveScene().name);
		}
	}
}

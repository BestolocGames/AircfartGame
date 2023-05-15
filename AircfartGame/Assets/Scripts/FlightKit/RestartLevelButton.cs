// dnSpy decompiler from Assembly-CSharp.dll class: FlightKit.RestartLevelButton
using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace FlightKit
{
	public class RestartLevelButton : MonoBehaviour
	{
		public virtual void Restart()
		{
			Time.timeScale = 1f;
			SceneManager.LoadScene(SceneManager.GetActiveScene().name);
		}
	}
}

using UnityEngine;
using UnityEngine.SceneManagement;

namespace UI
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

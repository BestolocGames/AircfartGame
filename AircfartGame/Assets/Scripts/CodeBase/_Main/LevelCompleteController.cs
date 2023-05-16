using UnityEngine;
using UnityEngine.SceneManagement;

namespace CodeBase._Main
{
	public class LevelCompleteController : MonoBehaviour
	{
		public virtual void HandleLevelComplete() => 
			SceneManager.LoadScene(SceneManager.GetActiveScene().name);
	}
}

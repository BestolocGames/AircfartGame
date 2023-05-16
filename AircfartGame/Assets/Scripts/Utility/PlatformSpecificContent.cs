using UnityEngine;

namespace Utility
{
	public class PlatformSpecificContent : MonoBehaviour
	{
		private void OnEnable()
		{
			CheckEnableContent();
		}

		private void CheckEnableContent()
		{
			if (m_BuildTargetGroup == BuildTargetGroup.Mobile)
			{
				EnableContent(true);
			}
			else
			{
				EnableContent(false);
			}
		}

		private void EnableContent(bool enabled)
		{
			if (m_Content.Length > 0)
			{
				foreach (GameObject gameObject in m_Content)
				{
					if (gameObject != null)
					{
						gameObject.SetActive(enabled);
					}
				}
			}
			if (m_ChildrenOfThisObject)
			{
				foreach (object obj in this.transform)
				{
					Transform transform = (Transform)obj;
					transform.gameObject.SetActive(enabled);
				}
			}
			if (m_MonoBehaviours.Length > 0)
			{
				foreach (MonoBehaviour monoBehaviour in m_MonoBehaviours)
				{
					monoBehaviour.enabled = enabled;
				}
			}
		}

		[SerializeField]
		private BuildTargetGroup m_BuildTargetGroup;

		[SerializeField]
		private GameObject[] m_Content = new GameObject[0];

		[SerializeField]
		private MonoBehaviour[] m_MonoBehaviours = new MonoBehaviour[0];

		[SerializeField]
		private bool m_ChildrenOfThisObject;

		private enum BuildTargetGroup
		{
			Standalone,
			Mobile
		}
	}
}

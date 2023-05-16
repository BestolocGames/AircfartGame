using UnityEngine;
using UnityEngine.Serialization;

namespace Utility
{
	public class PlatformSpecificContent : MonoBehaviour
	{
		 [SerializeField]
		private BuildTargetGroup _mBuildTargetGroup;

		 [SerializeField]
		private GameObject[] _mContent = new GameObject[0];

		 [SerializeField]
		private MonoBehaviour[] _mMonoBehaviours = new MonoBehaviour[0];

		[FormerlySerializedAs("m_ChildrenOfThisObject")] [SerializeField]
		private bool _mChildrenOfThisObject;

		private enum BuildTargetGroup
		{
			Standalone,
			Mobile
		}
		
		private void OnEnable() => 
			CheckEnableContent();

		private void CheckEnableContent()
		{
			if (_mBuildTargetGroup == BuildTargetGroup.Mobile)
				EnableContent(true);
			else
				EnableContent(false);
		}

		private void EnableContent(bool enabled)
		{
			if (_mContent.Length > 0)
			{
				foreach (GameObject gameObject in _mContent)
				{
					if (gameObject != null) 
						gameObject.SetActive(enabled);
				}
			}
			if (_mChildrenOfThisObject)
			{
				foreach (object obj in this.transform)
				{
					Transform transform = (Transform)obj;
					transform.gameObject.SetActive(enabled);
				}
			}
			if (_mMonoBehaviours.Length > 0)
			{
				foreach (MonoBehaviour monoBehaviour in _mMonoBehaviours) 
					monoBehaviour.enabled = enabled;
			}
		}

	}
}

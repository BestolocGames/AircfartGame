using UnityEngine;
using UnityEngine.Serialization;

namespace CodeBase._CrossPlatformInput
{
	[ExecuteInEditMode]
	public class PlatformDependentActivator : MonoBehaviour
	{
		[FormerlySerializedAs("standalone")] public GameObject _standalone;

		[FormerlySerializedAs("mobile")] public GameObject _mobile;
	}
}

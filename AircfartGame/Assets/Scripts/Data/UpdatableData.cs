using UnityEngine;
using UnityEngine.Serialization;

namespace Data
{
	public class UpdatableData : ScriptableObject {

		public event System.Action OnValuesUpdated;
		[FormerlySerializedAs("autoUpdate")] public bool _autoUpdate;

#if UNITY_EDITOR

		protected virtual void OnValidate() {
			if (_autoUpdate) {
				UnityEditor.EditorApplication.update += NotifyOfUpdatedValues;
			}
		}

		public void NotifyOfUpdatedValues() {
			UnityEditor.EditorApplication.update -= NotifyOfUpdatedValues;
			if (OnValuesUpdated != null) {
				OnValuesUpdated ();
			}
		}

#endif

	}
}

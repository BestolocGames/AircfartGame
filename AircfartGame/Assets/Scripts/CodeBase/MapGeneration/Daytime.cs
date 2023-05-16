using UnityEngine;
using UnityEngine.Serialization;

namespace CodeBase.MapGeneration
{
    public class Daytime : MonoBehaviour
    {
        [FormerlySerializedAs("sun")] public GameObject _sun;
        private void FixedUpdate()
        {
            _sun.transform.Rotate(Vector3.right * Time.deltaTime * -0.5f);
        }
    }
}

using UnityEngine;

namespace CodeBase.MapGeneration
{
    public class Daytime : MonoBehaviour
    {
        public GameObject sun;
        private void FixedUpdate()
        {
            sun.transform.Rotate(Vector3.right * Time.deltaTime * -0.5f);
        }
    }
}

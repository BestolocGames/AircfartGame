using CodeBase._Main;
using UnityEngine;

namespace CodeBase
{
    public class EnemyPointer : MonoBehaviour {

        [SerializeField] PickupSphere _enemyHealth;

        private void Start() {
            PointerManager.Instance.AddToList(this);
            _enemyHealth.OnCollect.AddListener(Destroy);
        }

        private void Destroy() {
            PointerManager.Instance.RemoveFromList(this);
        }

    }
}

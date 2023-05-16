using CodeBase._Main;
using UnityEngine;

namespace CodeBase
{
    public class EnemyPointer : MonoBehaviour {

        [SerializeField] PickupSphere _enemyHealth;

        private void Start() {
            PointerManager.Instance.AddToList(this);
            _enemyHealth._collect.AddListener(Destroy);
        }

        private void Destroy() => 
            PointerManager.Instance.RemoveFromList(this);
    }
}

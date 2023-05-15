using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

public class EnemyPointer : MonoBehaviour {

    [SerializeField] EnemyHealth _enemyHealth;

    private void Start() {
        PointerManager.Instance.AddToList(this);
        _enemyHealth.OnDie.AddListener(Destroy);
    }

    private void Destroy() {
        PointerManager.Instance.RemoveFromList(this);
    }

}

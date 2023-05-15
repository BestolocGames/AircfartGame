using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using FlightKit;
using UnityEngine;

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

using System;
using System.Collections.Generic;
using UnityEngine;

public class DestroyablePlant : MonoBehaviour
{
    public event EventHandler OnDestroyableTakeDamage;
    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.GetComponent<Sword>()) {
            OnDestroyableTakeDamage?.Invoke(this, EventArgs.Empty);
            Destroy(gameObject);
            NavMeshSurfaceManagment.Instance.RebakeNavmeshSurface();
        }
    }
}

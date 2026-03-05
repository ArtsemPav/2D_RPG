using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    [SerializeField] private DestroyablePlant destroyablePlant;
    [SerializeField] private GameObject deathVFX;

    private void Start() {
        destroyablePlant.OnDestroyableTakeDamage += DestroyablePlant_OnDestroyableTakeDamage;
    }

    private void DestroyablePlant_OnDestroyableTakeDamage(object sender, System.EventArgs e) {
        ShowVFX();
    }

    private void OnDestroy() {
        destroyablePlant.OnDestroyableTakeDamage -= DestroyablePlant_OnDestroyableTakeDamage;
    }

    private void ShowVFX() {
        Instantiate(deathVFX, destroyablePlant.transform.position, Quaternion.identity);
    }
}

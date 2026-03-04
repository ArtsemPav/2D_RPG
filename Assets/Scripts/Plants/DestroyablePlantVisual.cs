using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    [SerializeField] private DestroyablePlant _destroyablePlant;
    [SerializeField] private GameObject _deathVFX;

    private void Start() {
        _destroyablePlant.OnDestroyableTakeDamage += DestroyablePlant_OnDestroyableTakeDamage;
    }

    private void DestroyablePlant_OnDestroyableTakeDamage(object sender, System.EventArgs e) {
        ShowVFX();
    }

    private void OnDestroy() {
        _destroyablePlant.OnDestroyableTakeDamage -= DestroyablePlant_OnDestroyableTakeDamage;
    }

    private void ShowVFX() {
        Instantiate(_deathVFX, _destroyablePlant.transform.position, Quaternion.identity);
    }
}

using System;
using UnityEngine;

public class Sword : MonoBehaviour
{
    [SerializeField] private int swordDamage = 2;
    public event EventHandler OnSwordSwing;
    private PolygonCollider2D _polygonCollider2D;

    private void Awake() {
        _polygonCollider2D = GetComponent<PolygonCollider2D>();
    }
    private void Start() {
        AttackColliderTurnOff();
    }

    public void Attack()
    {
        AttackColliderTurnOffOn();
        OnSwordSwing?.Invoke(this, EventArgs.Empty);
    }

    public void AttackColliderTurnOff() {
        _polygonCollider2D.enabled = false;
    }

    private void AttackColliderTurnOn() {
        _polygonCollider2D.enabled = true;
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.transform.TryGetComponent(out EnemyHp enemyhp)){
            enemyhp.TakeDamage(swordDamage);
        }
    }

    private void AttackColliderTurnOffOn() {
        AttackColliderTurnOff();
        AttackColliderTurnOn();
    }
}

using System;
using UnityEngine;

[RequireComponent(typeof(PolygonCollider2D))]
public class EnemyHp : MonoBehaviour {
    [SerializeField] private EnemySO enemySO;
    [SerializeField] private EnemyAI enemyAI;
    private int _currentHealh;

    private PolygonCollider2D _polygonCollider2D;
    private BoxCollider2D _boxCollider2D;

    public event EventHandler OnTakeHit;
    public event EventHandler OnDeath;

    private void Awake() {
        _polygonCollider2D = GetComponent<PolygonCollider2D>();
        _boxCollider2D = GetComponent<BoxCollider2D>();
    }

    private void Start() {
        _currentHealh = enemySO.EnemyHealth;
        _polygonCollider2D.enabled = false;
    }

    private void OnTriggerStay2D(Collider2D collision) {
        if (collision.transform.TryGetComponent(out Player player)) {
            player.TakeDamage(transform, enemySO.EnemyDamageAmount);
        }
    }

    public void TakeDamage(int damage) {
        _currentHealh -= damage;
        OnTakeHit?.Invoke(this, EventArgs.Empty);
        DetectDeath();
    }

    public void PolygonColliderTurnOff() {
        _polygonCollider2D.enabled = false;
    }

    public void PolygonColliderTurnOn() {
        _polygonCollider2D.enabled = true;
    }

    private void DetectDeath() {
        if (_currentHealh <= 0) {
            enemyAI.SetDeathState();
            _boxCollider2D.enabled = false;
            _polygonCollider2D.enabled = false;
            OnDeath?.Invoke(this, EventArgs.Empty);
        }
    }
}

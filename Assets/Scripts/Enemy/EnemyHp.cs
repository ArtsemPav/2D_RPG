using System;
using UnityEngine;

[RequireComponent(typeof(PolygonCollider2D))]
public class EnemyHp : MonoBehaviour {
    [SerializeField] private EnemySO _enemySO;
    [SerializeField] private EnemyAI _enemyAI;
    private int _currentHealh;

    private PolygonCollider2D _polygonCollider2D;

    public event EventHandler OnTakeHit;
    public event EventHandler OnDeath;

    private void Awake() {
        _polygonCollider2D = GetComponent<PolygonCollider2D>();
    }

    private void Start() {
        _currentHealh = _enemySO.EnemyHealth;
        _polygonCollider2D.enabled = false;
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        Debug.Log("Attack");
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
            _enemyAI.SetDeathState();
            OnDeath?.Invoke(this, EventArgs.Empty);
        }
    }
}

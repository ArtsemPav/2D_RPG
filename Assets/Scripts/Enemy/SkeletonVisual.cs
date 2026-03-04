using System;
using UnityEngine;

public class SkeletonVisual : MonoBehaviour
{
    [SerializeField] private EnemyAI _enemyAI;
    [SerializeField] private EnemyHp _enemyHp;
    [SerializeField] private GameObject _shadow;
    private Animator _animator;
    private const string IS_RUNNING = "IsRunning";
    private const string CHASING_SPEED_MULTIPLIER = "ChasingSpeedMultiplier";
    private const string ATTACK = "Attack";
    private const string TAKE_HIT = "TakeHit";
    private const string ISDIE = "IsDie";
    private SpriteRenderer _spriteRenderer;
    private void Awake() {
        _animator = GetComponent<Animator>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }
    private void Start() {
        _enemyAI.OnEnemyAttack += _enemyAI_OnEnemyAttack;
        _enemyHp.OnTakeHit += _enemyHp_OnTakeHit;
        _enemyHp.OnDeath += _enemyHp_OnDeath;
    }

    private void Update() {
        _animator.SetBool(IS_RUNNING, _enemyAI.IsRunning());
        _animator.SetFloat(CHASING_SPEED_MULTIPLIER, _enemyAI.GetRoamingAnimationSpeed());
    }

    private void _enemyHp_OnTakeHit(object sender, EventArgs e) {
        _animator.SetTrigger(TAKE_HIT);
    }

    private void _enemyHp_OnDeath(object sender, EventArgs e) {
        _animator.SetBool(ISDIE, true);
        _spriteRenderer.sortingOrder = -1;
        _shadow.SetActive(false);
    }
    public void TriggerAyyackAnimationTurnOn() {
        _enemyHp.PolygonColliderTurnOn();
    }
    public void TriggerAyyackAnimationTurnOff() {
        _enemyHp.PolygonColliderTurnOff();
    }
    private void _enemyAI_OnEnemyAttack(object sender, EventArgs e) {
        _animator.SetTrigger(ATTACK);
    }
    private void OnDestroy() {
        _enemyAI.OnEnemyAttack -= _enemyAI_OnEnemyAttack;
        _enemyHp.OnTakeHit -= _enemyHp_OnTakeHit;
        _enemyHp.OnDeath -= _enemyHp_OnDeath;
    }
}

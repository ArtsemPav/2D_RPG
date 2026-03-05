using System;
using UnityEngine;

public class SkeletonVisual : MonoBehaviour
{
    private static readonly int Running = Animator.StringToHash(IsRunning);
    private static readonly int SpeedMultiplier = Animator.StringToHash(ChasingSpeedMultiplier);
    private static readonly int Attack = Animator.StringToHash(IsAttack);
    private static readonly int TakeHit = Animator.StringToHash(IsTakeHit);
    private static readonly int Die = Animator.StringToHash(IsDie);
    [SerializeField] private EnemyAI enemyAI;
    [SerializeField] private EnemyHp enemyHp;
    [SerializeField] private GameObject shadow;
    private Animator _animator;
    private const string IsRunning = "IsRunning";
    private const string ChasingSpeedMultiplier = "ChasingSpeedMultiplier";
    private const string IsAttack = "Attack";
    private const string IsTakeHit = "TakeHit";
    private const string IsDie = "IsDie";
    private SpriteRenderer _spriteRenderer;
    private void Awake() {
        _animator = GetComponent<Animator>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }
    private void Start() {
        enemyAI.OnEnemyAttack += _enemyAI_OnEnemyAttack;
        enemyHp.OnTakeHit += _enemyHp_OnTakeHit;
        enemyHp.OnDeath += _enemyHp_OnDeath;
    }

    private void Update() {
        _animator.SetBool(Running, enemyAI.IsRunning());
        _animator.SetFloat(SpeedMultiplier, enemyAI.GetRoamingAnimationSpeed());
    }

    private void _enemyHp_OnTakeHit(object sender, EventArgs e) {
        _animator.SetTrigger(TakeHit);
    }

    private void _enemyHp_OnDeath(object sender, EventArgs e) {
        _animator.SetBool(Die, true);
        _spriteRenderer.sortingOrder = -1;
        shadow.SetActive(false);
    }
    public void TriggerAyyackAnimationTurnOn() {
        enemyHp.PolygonColliderTurnOn();
    }
    public void TriggerAyyackAnimationTurnOff() {
        enemyHp.PolygonColliderTurnOff();
    }
    private void _enemyAI_OnEnemyAttack(object sender, EventArgs e) {
        _animator.SetTrigger(Attack);
    }

    private void OnDestroy() {
        enemyAI.OnEnemyAttack -= _enemyAI_OnEnemyAttack;
        enemyHp.OnTakeHit -= _enemyHp_OnTakeHit;
        enemyHp.OnDeath -= _enemyHp_OnDeath;
    }
}

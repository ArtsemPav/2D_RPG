using System;
using UnityEngine;

public class SwordVisual : MonoBehaviour
{
    private static readonly int AttackHash = Animator.StringToHash(IsAttack);
    [SerializeField] private Sword sword;
    private Animator animator;
    private const string IsAttack = "Attack";

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        sword.OnSwordSwing += Sword_OnSwordSwing;
    }

    private void Sword_OnSwordSwing(object sender, EventArgs e)
    {
        animator.SetTrigger(AttackHash);
    }

    public void TriggerEndAnimation() {
        sword.AttackColliderTurnOff();
    }

    private void OnDestroy() {
        sword.OnSwordSwing -= Sword_OnSwordSwing;
    }
}

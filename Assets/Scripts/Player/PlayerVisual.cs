using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerVisual : MonoBehaviour
{
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    private const string IS_RUNNING = "IsRunning";

    private void Awake()
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        isPlayerRunnig();
        adjustPlayerFacingDirection();

    }

    private void isPlayerRunnig()
    {
        animator.SetBool(IS_RUNNING, Player.Instance.IsRunning());
    }

    private void adjustPlayerFacingDirection()
    {
        Vector3 playerPos = Player.Instance.GetPlayerScreenPosition();
        Vector3 mousePos = GameInput.Instance.GetMousePosition();
        if (mousePos.x >= playerPos.x)
            spriteRenderer.flipX = false;
        else
            spriteRenderer.flipX = true;
    }
}

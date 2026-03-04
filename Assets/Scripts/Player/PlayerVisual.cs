using UnityEngine;

public class PlayerVisual : MonoBehaviour
{
    private Animator _animator;
    private SpriteRenderer _spriteRenderer;
    private Flashblink _flashblink;
    private const string IS_RUNNING = "IsRunning";
    private const string IS_DIE = "IsDie";

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _flashblink = GetComponent<Flashblink>();
    }

    private void Update()
    {
        isPlayerRunnig();
        if (Player.Instance.IsAlive())
            adjustPlayerFacingDirection();

    }
    private void Start() {
        Player.Instance.OnPlayerDeath += Player_OnPlayerDeath; ;
    }

    private void Player_OnPlayerDeath(object sender, System.EventArgs e) {
        _animator.SetBool(IS_DIE, true);
        _flashblink.StopBlinking();
    }

    private void isPlayerRunnig()
    {
        _animator.SetBool(IS_RUNNING, Player.Instance.IsRunning());
    }

    private void adjustPlayerFacingDirection()
    {
        Vector3 playerPos = Player.Instance.GetPlayerScreenPosition();
        Vector3 mousePos = GameInput.Instance.GetMousePosition();
        if (mousePos.x >= playerPos.x)
            _spriteRenderer.flipX = false;
        else
            _spriteRenderer.flipX = true;
    }
    private void OnDestroy() {
        Player.Instance.OnPlayerDeath -= Player_OnPlayerDeath; ;
    }
}

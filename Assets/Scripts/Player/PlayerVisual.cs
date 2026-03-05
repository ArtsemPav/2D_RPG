using UnityEngine;

public class PlayerVisual : MonoBehaviour
{
    private static readonly int Die = Animator.StringToHash(IsDie);
    private static readonly int Running = Animator.StringToHash(IsRunning);
    private Animator _animator;
    private SpriteRenderer _spriteRenderer;
    private Flashblink _flashblink;
    private const string IsRunning = "IsRunning";
    private const string IsDie = "IsDie";

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
        _animator.SetBool(Die, true);
        _flashblink.StopBlinking();
    }

    private void isPlayerRunnig()
    {
        _animator.SetBool(Running, Player.Instance.IsRunning());
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

using System;
using UnityEngine;
using System.Collections;

[SelectionBase]
public class Player : MonoBehaviour
{
    public static Player Instance { get; private set; }

    public event EventHandler OnPlayerDeath;
    public event EventHandler OnFlashBlink;

    [SerializeField] private float movingSpeed = 5f;
    [SerializeField] private int maxHealth = 20;
    [SerializeField] private float damageRecoveryTime = 0.5f;
    [SerializeField] private int dashSpeed = 4;
    [SerializeField] private float dashTime = 1f;
    [SerializeField] private TrailRenderer trailRenderer;

    private Vector2 _inputVector;
    private Rigidbody2D _rb;
    private KnockBack _knockBack;
    private readonly float _minMovementSpeed = 0.1f;
    private bool _isRunning = false;
    private int _currentHealth;
    private bool _canTakedamage;
    private bool _isAlive;
    private bool _isDash;
    private Camera _mainCamera;

    private void Awake()
    {
        Instance = this;
        DontDestroyOnLoad(this);
        _rb = GetComponent<Rigidbody2D>();
        _knockBack = GetComponent<KnockBack>();
        _mainCamera = Camera.main;
    }

    private void Start()
    {
        GameInput.Instance.OnPlayerAttack += Player_OnPlayerAttack;
        GameInput.Instance.OnPlayerDash += Player_OnPlayerDash;
        _currentHealth = maxHealth;
        _canTakedamage = true;
        _isAlive = true;
        trailRenderer.emitting = false;
    }

    private void Update() {
        _inputVector = GameInput.Instance.GetMovementVector();
    }

    private void FixedUpdate()
    {
        if (_knockBack.IsKnockBack)
            return;
        HandleMovement();
    }
    public bool IsAlive() {
        return _isAlive;
    }

    public Vector3 GetPlayerScreenPosition() {
        Vector3 playerScreenPos = _mainCamera.WorldToScreenPoint(transform.position);
        return playerScreenPos;
    }

    public bool IsRunning() {
        return _isRunning;
    }

    public void TakeDamage(Transform damageSource, int damage) {
        if (_canTakedamage && _isAlive) {
            _canTakedamage = false;
            _currentHealth = Mathf.Max(0, _currentHealth - damage);
            Debug.Log(_currentHealth);
            _knockBack.GetKnockedBack(damageSource);
            StartCoroutine(RecoveryTime());
            OnFlashBlink?.Invoke(this, EventArgs.Empty);
        }
        DetectDeath();
    }
    private void DetectDeath() {
        if (_currentHealth <= 0 && _isAlive) {
            _isAlive = false;
            GameInput.Instance.DisableMovement();
            _knockBack.StopKnockBackMovement();
            OnPlayerDeath?.Invoke(this, EventArgs.Empty);
        }
    }

    private void Player_OnPlayerAttack(object sender, EventArgs e) {
        ActiveWeapon.Instance.GetActiveWeapon().Attack();
    }

    private void Player_OnPlayerDash(object sender, EventArgs e) {
        if (!_isDash) Dash();
    }

    private void Dash() {
        StartCoroutine(DashTime());
    }

    private void HandleMovement()
    {
        _rb.MovePosition(_rb.position + _inputVector * (movingSpeed * Time.fixedDeltaTime));
        if (Mathf.Abs(_inputVector.x) > _minMovementSpeed || Mathf.Abs(_inputVector.y) > _minMovementSpeed)
            _isRunning = true;
        else
            _isRunning = false;
    }

    private IEnumerator RecoveryTime() {
        yield return new WaitForSeconds(damageRecoveryTime);
        _canTakedamage = true;
    }

    private IEnumerator DashTime() {
        float _defaultMovingSpeed = movingSpeed;
        _isDash = true;
        trailRenderer.emitting = true;
        movingSpeed *= dashSpeed;
        yield return new WaitForSeconds(dashTime);
        _isDash = false;
        trailRenderer.emitting = false;
        movingSpeed = _defaultMovingSpeed;
    }

    private void OnDestroy() {
        GameInput.Instance.OnPlayerAttack -= Player_OnPlayerAttack;
        GameInput.Instance.OnPlayerDash -= Player_OnPlayerDash;
    }
}

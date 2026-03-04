using System;
using UnityEngine;
using System.Collections;

[SelectionBase]
public class Player : MonoBehaviour
{
    public static Player Instance { get; private set; }

    public event EventHandler OnPlayerDeath;
    public event EventHandler OnFlashBlink;

    [SerializeField] private float _movingSpeed = 5f;
    [SerializeField] private int _maxHealth = 20;
    [SerializeField] private float _damageRecoveryTime = 0.5f;

    private Vector2 _inputVector;
    private Rigidbody2D _rb;
    private KnockBack _knockBack;
    private float _minMovementSpeed = 0.1f;
    private bool _isRunning = false;
    private int _currentHealth;
    private bool _canTakedamage;
    private bool _isAlive;

    private void Awake()
    {
        Instance = this;
        DontDestroyOnLoad(this);
        _rb = GetComponent<Rigidbody2D>();
        _knockBack = GetComponent<KnockBack>();
    }

    private void Start()
    {
        GameInput.Instance.OnPlayerAttack += Player_OnPlayerAttack;
        _currentHealth = _maxHealth;
        _canTakedamage = true;
        _isAlive = true;
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
        Vector3 playerScreenPos = Camera.main.WorldToScreenPoint(transform.position);
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
            GameInput.Instance.OnPlayerAttack -= Player_OnPlayerAttack;
        }
    }

    private void Player_OnPlayerAttack(object sender, EventArgs e) {
        ActiveWeapon.Instance.GetActiveWeapon().Attack();
    }

    private void HandleMovement()
    {
        Vector2 inputVector = GameInput.Instance.GetMovementVector();
        _rb.MovePosition(_rb.position + inputVector * (_movingSpeed * Time.fixedDeltaTime));
        if (Mathf.Abs(inputVector.x) > _minMovementSpeed || Mathf.Abs(inputVector.y) > _minMovementSpeed)
            _isRunning = true;
        else
            _isRunning = false;
    }

    private IEnumerator RecoveryTime() {
        yield return new WaitForSeconds(_damageRecoveryTime);
        _canTakedamage = true;
    }
    private void OnDestroy() {
        GameInput.Instance.OnPlayerAttack -= Player_OnPlayerAttack;
    }
}

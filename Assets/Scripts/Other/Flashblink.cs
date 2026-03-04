using System;
using UnityEngine;

public class Flashblink : MonoBehaviour
{
    [SerializeField] private MonoBehaviour _damagableObject;
    [SerializeField] private Material _blinkMaterial;
    [SerializeField] private float _blinkDuration = 0.1f;

    private float _blinkTimer;
    private Material _defaultMaterial;
    private SpriteRenderer _spriteRenderer;
    private bool _isBlinking;

    private void Awake() {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _defaultMaterial = _spriteRenderer.material;
        _isBlinking = true;
    }

    private void Start() {
        if (_damagableObject is Player) {
            (_damagableObject as Player).OnFlashBlink += Player_OnFlashBlink;
        }
    }

    private void Update() {
        if (_isBlinking) {
            _blinkTimer -= Time.deltaTime;
            if (_blinkTimer < 0) {
                SetDefaultMaterial();
            }
        }
    }
    public void StopBlinking() {
        SetDefaultMaterial();
        _isBlinking = false;
    }

    private void Player_OnFlashBlink(object sender, EventArgs e) {
        SetBlinkingMaterial();
    }

    private void SetDefaultMaterial() {
        _spriteRenderer.material = _defaultMaterial;
    }

    private void SetBlinkingMaterial() {
        _blinkTimer = _blinkDuration;
        _spriteRenderer.material = _blinkMaterial;
    }

    private void OnDestroy() {
        if (_damagableObject is Player) {
            (_damagableObject as Player).OnFlashBlink -= Player_OnFlashBlink;
        }
    }
}

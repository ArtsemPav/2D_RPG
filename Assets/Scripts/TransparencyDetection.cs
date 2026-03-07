using UnityEngine;
using System.Collections;

public class TransparencyDetection:MonoBehaviour
{
    [Range(0f,1f)]
    [SerializeField] private float transparencyAmount = 0.8f;
    [SerializeField] private float fadeTime = 0.01f;
    private SpriteRenderer _spriteRenderer;
    private const float NonTransparent = 1f;

    private void Awake() {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void OnTriggerEnter2D(Collider2D collider) {
        if (collider.gameObject.GetComponent<Player>() && collider is CapsuleCollider2D) {
            StartCoroutine(Fade(_spriteRenderer, fadeTime, _spriteRenderer.color.a, transparencyAmount));
            Debug.Log("Transparency On");
        }
    }

    private void OnTriggerExit2D(Collider2D collider) {
        if (collider.gameObject.GetComponent<Player>() && collider is CapsuleCollider2D) {
            StartCoroutine(Fade(_spriteRenderer, fadeTime, _spriteRenderer.color.a, NonTransparent));
            Debug.Log("Transparency Off");
        }
    }

    private IEnumerator Fade(SpriteRenderer spriterenderer, float fadeTime, float startTransparencyAmount, float targetTransparencyAmount) {
        float elapsedTime = 0f;
        while (elapsedTime < fadeTime) {
            elapsedTime += Time.deltaTime;
            float newAlpha = Mathf.Lerp(startTransparencyAmount, targetTransparencyAmount, elapsedTime / fadeTime);
            spriterenderer.color = new Color(spriterenderer.color.r, spriterenderer.color.g, spriterenderer.color.b, newAlpha);

            yield return null;
        }
    }

}

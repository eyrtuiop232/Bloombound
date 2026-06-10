using System.Collections;
using UnityEngine;

public class SpriteFader : MonoBehaviour
{
    public float FadeDuration = 1f;

    private SpriteRenderer _sprite;
    private Coroutine _activeCoroutine;

    private void Awake()
    {
        _sprite = GetComponentInChildren<SpriteRenderer>();
    }

    public void FadeOut()
    {
        StartFade(0f);
    }

    public void FadeIn()
    {
        StartFade(1f);
    }

    private void StartFade(float targetAlpha)
    {
        if (_activeCoroutine != null)
            StopCoroutine(_activeCoroutine);
        _activeCoroutine = StartCoroutine(FadeRoutine(targetAlpha));
    }

    private IEnumerator FadeRoutine(float targetAlpha)
    {
        float startAlpha = _sprite.color.a;
        float elapsed = 0f;

        while (elapsed < FadeDuration)
        {
            elapsed += Time.deltaTime;
            float alpha = Mathf.Lerp(startAlpha, targetAlpha, elapsed / FadeDuration);
            SetAlpha(alpha);
            yield return null;
        }

        SetAlpha(targetAlpha);
        _activeCoroutine = null;
    }

    private void SetAlpha(float alpha)
    {
        Color c = _sprite.color;
        c.a = alpha;
        _sprite.color = c;
    }
}

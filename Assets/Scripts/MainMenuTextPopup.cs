using System.Collections;
using TMPro;
using UnityEngine;

public class MainMenuTextPopup : MonoBehaviour
{
    public float PopupY = 20f;
    public float Duration = 0.3f;

    private TMP_Text _text;
    private Vector3 _originPos;
    private Coroutine _activeCoroutine;

    private void Awake()
    {
        _text = GetComponent<TMP_Text>();
        _originPos = transform.localPosition;
        SetAlpha(0f);
    }

    public void FadeIn()
    {
        StartTween(_originPos + new Vector3(0f, PopupY, 0f), 1f);
    }

    public void FadeOut()
    {
        StartTween(_originPos, 0f);
    }

    private void StartTween(Vector3 targetPos, float targetAlpha)
    {
        if (_activeCoroutine != null)
            StopCoroutine(_activeCoroutine);
        _activeCoroutine = StartCoroutine(TweenRoutine(targetPos, targetAlpha));
    }

    private IEnumerator TweenRoutine(Vector3 targetPos, float targetAlpha)
    {
        Vector3 startPos = transform.localPosition;
        float startAlpha = _text.color.a;
        float elapsed = 0f;

        while (elapsed < Duration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.SmoothStep(0f, 1f, elapsed / Duration);
            transform.localPosition = Vector3.Lerp(startPos, targetPos, t);
            SetAlpha(Mathf.Lerp(startAlpha, targetAlpha, t));
            yield return null;
        }

        transform.localPosition = targetPos;
        SetAlpha(targetAlpha);
        _activeCoroutine = null;
    }

    private void SetAlpha(float alpha)
    {
        Color c = _text.color;
        c.a = alpha;
        _text.color = c;
    }
}

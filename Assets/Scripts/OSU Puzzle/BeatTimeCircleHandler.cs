using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class BeatTimeCircleHandler : MonoBehaviour
{
    [Header("References")]
    public Image  circle;
    public Button button;

    [Header("Timing")]

    public float pressTime;
    public float approachDuration = 1f;
    public float startScale = 3f;

    [Header("Hit Windows (seconds)")]
    public float perfectWindow = 0.05f;
    public float goodWindow    = 0.10f;
    public float okWindow      = 0.20f;

    [Header("Indicator")]
    public Color hitColor  = Color.green;
    public Color missColor = Color.red;
    public float fadeDuration = 0.3f;

    [Header("Events")]
    public UnityEvent<int> onHit;
    public UnityEvent      onMiss;

    private bool _resolved;

    void Awake()
    {
        if (circle == null)
            circle = GetComponent<Image>();
    }

    void OnEnable()
    {
        _resolved = false;
        circle.color = Color.white;
        circle.transform.localScale = Vector3.one * startScale;
        button.onClick.AddListener(OnButtonClick);
    }

    void OnDisable()
    {
        button.onClick.RemoveListener(OnButtonClick);
    }

    void Update()
    {

        if (_resolved || pressTime <= 0f) return;

        float t = Mathf.Clamp01((Time.time - (pressTime - approachDuration)) / approachDuration);
        circle.transform.localScale = Vector3.one * Mathf.Lerp(startScale, 1f, t);

        if (Time.time > pressTime + okWindow)
            Resolve(miss: true);
    }

    void OnButtonClick()
    {
        if (_resolved) return;

        float diff = Mathf.Abs(Time.time - pressTime);

        if (Time.time < pressTime - okWindow) return;

        int score = diff <= perfectWindow ? 300
                  : diff <= goodWindow    ? 100
                  : diff <= okWindow      ?  50
                  : 0;

        if (score == 0) return;

        Resolve(miss: false, score: score);
    }

    void Resolve(bool miss, int score = 0)
    {
        _resolved = true;

        if (miss)
            onMiss?.Invoke();
        else
            onHit?.Invoke(score);

        StartCoroutine(FlashAndFade(miss ? missColor : hitColor));
    }

    IEnumerator FlashAndFade(Color flashColor)
    {
        circle.color = flashColor;
        float elapsed = 0f;

        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            float a = Mathf.Lerp(1f, 0f, elapsed / fadeDuration);
            circle.color = new Color(flashColor.r, flashColor.g, flashColor.b, a);
            yield return null;
        }

        gameObject.SetActive(false);
    }
}

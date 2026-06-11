using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class OSUPuzzleHandler : MonoBehaviour
{
    public GameObject beatPrefab;
    public GameObject OSU_Canvas;
    public int   beat_to_spawn;
    public float spawn_delay;
    public float spawnMargin = 100f;

    [Header("Events")]
    public UnityEvent onMinigameWin;
    public UnityEvent onMinigameLose;

    private static readonly Vector2 CanvasHalfSize = new(960f, 540f);
    private const float WinThreshold = 0.8f;
    private static readonly WaitForSeconds ResultDelay = new(1f);

    private int _hitCount;
    private int _resolvedCount;

    void Start()
    {
        _hitCount      = 0;
        _resolvedCount = 0;
        StartCoroutine(SpawnBeats());
    }

    IEnumerator SpawnBeats()
    {
        for (int i = 0; i < beat_to_spawn; i++)
        {
            SpawnBeat();
            yield return new WaitForSeconds(spawn_delay);
        }
    }

    void SpawnBeat()
    {
        GameObject obj = Instantiate(beatPrefab, OSU_Canvas.transform);

        RectTransform rt = obj.GetComponent<RectTransform>();
        rt.anchorMin = new Vector2(0.5f, 0.5f);
        rt.anchorMax = new Vector2(0.5f, 0.5f);
        rt.anchoredPosition = new Vector2(
            Random.Range(-CanvasHalfSize.x + spawnMargin, CanvasHalfSize.x - spawnMargin),
            Random.Range(-CanvasHalfSize.y + spawnMargin, CanvasHalfSize.y - spawnMargin)
        );

        BeatTimeCircleHandler handler = obj.GetComponent<BeatTimeCircleHandler>();
        handler.pressTime = Time.time + handler.approachDuration;
        handler.onHit.AddListener(OnBeatHit);
        handler.onMiss.AddListener(OnBeatMiss);
    }

    void OnBeatHit(int score)
    {
        _hitCount++;
        _resolvedCount++;
        Debug.Log($"Hit! Score: {score}");
        TryEvaluateResult();
    }

    void OnBeatMiss()
    {
        _resolvedCount++;
        Debug.Log("Miss!");
        TryEvaluateResult();
    }

    void TryEvaluateResult()
    {
        if (_resolvedCount < beat_to_spawn) return;

        StartCoroutine(FireResultAfterDelay());
    }

    IEnumerator FireResultAfterDelay()
    {
        yield return ResultDelay;

        float hitRate = (float)_hitCount / beat_to_spawn;
        if (hitRate >= WinThreshold)
            onMinigameWin.Invoke();
        else
            onMinigameLose.Invoke();

        Destroy(gameObject);
    }
}

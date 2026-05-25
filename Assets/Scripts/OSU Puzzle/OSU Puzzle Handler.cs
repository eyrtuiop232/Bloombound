using System.Collections;
using UnityEngine;

public class OSUPuzzleHandler : MonoBehaviour
{
    public GameObject beatPrefab;
    public GameObject OSU_Canvas;
    public int   beat_to_spawn;
    public float spawn_delay;
    public float spawnMargin = 100f;  // Keep beats this many pixels from screen edges

    private static readonly Vector2 CanvasHalfSize = new(960f, 540f);

    void Start()
    {
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
        Debug.Log($"Hit! Score: {score}");
    }

    void OnBeatMiss()
    {
        Debug.Log("Miss!");
    }
}

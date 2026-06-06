using UnityEngine;

[RequireComponent(typeof(MovementSystem))]
public class FootstepSFX : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip[] clips;

    [Tooltip("Time in seconds between each footstep.")]
    public float interval = 0.35f;

    private MovementSystem _movement;
    private float _timer;
    private int _lastIndex = -1;

    private void Awake()
    {
        _movement = GetComponent<MovementSystem>();
    }

    private void Update()
    {
        if (_movement.movedir == Vector2.zero)
        {
            _timer = 0f;
            return;
        }

        _timer -= Time.deltaTime;
        if (_timer > 0f) return;

        _timer = interval;
        PlayStep();
    }

    private void PlayStep()
    {
        if (audioSource == null || clips == null || clips.Length == 0) return;

        int index;
        if (clips.Length == 1)
        {
            index = 0;
        }
        else
        {
            do { index = Random.Range(0, clips.Length); }
            while (index == _lastIndex);
        }

        _lastIndex = index;
        audioSource.PlayOneShot(clips[index]);
    }
}

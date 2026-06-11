using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class MortalKombatMinigame : MonoBehaviour
{
    [Header("Bar")]
    public Image bar;

    [Header("Settings")]
    public float fillPerPress = 0.1f;
    [Tooltip("Fill amount lost per second.")]
    public float drainRate = 0.15f;

    [Header("Events")]
    public UnityEvent onMinigameStart;
    public UnityEvent onMinigameFinished;

    private bool _active;

    private void Start()
    {
        StartMinigame();
    }

    public void StartMinigame()
    {
        if (bar != null)
            bar.fillAmount = 0f;

        _active = true;
        onMinigameStart.Invoke();
    }

    private void Update()
    {
        if (!_active) return;

        if (Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            bar.fillAmount = Mathf.Clamp01(bar.fillAmount + fillPerPress);

            if (bar.fillAmount >= 1f)
            {
                _active = false;
                onMinigameFinished.Invoke();
                return;
            }
        }

        bar.fillAmount = Mathf.Clamp01(bar.fillAmount - drainRate * Time.deltaTime);
    }
}

using NUnit.Framework;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Place this on any GameObject that should react when the waterfall hits it.
/// The Waterfall script calls NotifyEnter/NotifyExit automatically — no
/// manual wiring needed beyond assigning events in the Inspector.
/// </summary>
public class WaterfallDetector : MonoBehaviour
{
    [Tooltip("Fired once when the waterfall first starts hitting this object.")]
    public UnityEvent OnWaterEnter;

    [Tooltip("Fired once when the waterfall stops hitting this object.")]
    public UnityEvent OnWaterExit;
    private bool hasExit = true;
    // Called by Waterfall — not meant to be called from other scripts.
    public void NotifyEnter()
    {
        if (hasExit == true)
        {
            hasExit = false;
            OnWaterEnter?.Invoke();
        }
    }
    public void NotifyExit()
    {
        OnWaterExit?.Invoke();
        hasExit = true;
    }
}

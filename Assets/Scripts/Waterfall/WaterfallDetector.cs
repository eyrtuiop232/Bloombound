using NUnit.Framework;
using UnityEngine;
using UnityEngine.Events;

public class WaterfallDetector : MonoBehaviour
{
    [Tooltip("Fired once when the waterfall first starts hitting this object.")]
    public UnityEvent OnWaterEnter;

    [Tooltip("Fired once when the waterfall stops hitting this object.")]
    public UnityEvent OnWaterExit;
    private bool hasExit = true;

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

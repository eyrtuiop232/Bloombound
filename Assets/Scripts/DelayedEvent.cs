using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class DelayedEvent : MonoBehaviour
{
    public UnityEvent myevent;
    public float waitFor;
    IEnumerator delayEvent()
    {
        yield return new WaitForSeconds(waitFor);
        myevent.Invoke();
    }

    public void Trigger()
    {
        StartCoroutine(delayEvent());
    }
}

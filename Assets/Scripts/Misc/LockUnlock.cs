using System.Collections.Generic;
using UnityEngine;

public class LockUnlock : MonoBehaviour
{
    public List<MonoBehaviour> targets;

    public void Lock()
    {
        foreach (MonoBehaviour target in targets)
            if (target != null)
                target.enabled = false;
    }

    public void Unlock()
    {
        foreach (MonoBehaviour target in targets)
            if (target != null)
                target.enabled = true;
    }
}

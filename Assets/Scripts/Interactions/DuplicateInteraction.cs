using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DuplicateInteraction : Interaction
{
    public UnityEvent interactStuffs;
    public List<GameObject> objectToDuplicate;
    public bool isEnabled = true;
    public override void Interact(GameObject interactor)
    {
        if (!isEnabled) return;

        foreach (GameObject obj in objectToDuplicate)
        {
            if (obj == null) continue;

            GameObject duplicate = Instantiate(obj, obj.transform.parent);
            duplicate.SetActive(true);
        }

        interactStuffs.Invoke();
    }

    public void disable()
    {
        isEnabled = false;
    }

    public void enable()
    {
        isEnabled = true;
    }
}

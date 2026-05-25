using UnityEngine;
using UnityEngine.Events;

public class GeneralInteraction : Interaction
{
    public UnityEvent interactStuffs;
    public bool isEnabled = true;
    public override void Interact(GameObject interactor)
    {
        if (isEnabled)
        {
            interactStuffs.Invoke();
        }
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

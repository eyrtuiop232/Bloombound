using UnityEngine;
using UnityEngine.Events;

public class GeneralInteraction : Interaction
{
    public UnityEvent interactStuffs;

    public override void Interact(GameObject interactor)
    {
        interactStuffs.Invoke();
    }
}

using UnityEngine;

// Attach to any object that should start a dialog (e.g. an NPC).
// Pair with the Interaction system: call Trigger() from Interaction.Interact().
public class DialogTrigger : Interaction
{
    public DialogData dialog;

    public void Trigger()
    {
        if (DialogManager.Instance == null)
        {
            Debug.LogWarning("DialogTrigger: No DialogManager found in scene.");
            return;
        }

        if (DialogManager.Instance.IsActive) return;

        DialogManager.Instance.StartDialog(dialog);
    }

    public override void Interact(GameObject interactor)
    {
        Trigger();
    }
}

using System.Collections.Generic;
using UnityEngine;

public class DialogActivator : MonoBehaviour
{
    public DialogData dialog;
    public List<DialogCanvas> displays;

    public void Trigger()
    {
        if (DialogManager.Instance == null)
        {
            Debug.LogWarning("DialogActivator: No DialogManager found in scene.");
            return;
        }

        if (displays.Count > 0 && DialogManager.Instance.IsActiveFor(displays[0])) return;

        DialogManager.Instance.StartDialog(dialog, displays);
    }
}

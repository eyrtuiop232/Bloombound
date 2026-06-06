using UnityEngine;
using UnityEngine.Events;

public class DialogEvents : MonoBehaviour
{
    public DialogData dialog;

    public UnityEvent onDialogStart;
    public UnityEvent onDialogContinue;
    public UnityEvent onDialogEnd;

    private void OnEnable()
    {
        if (dialog == null) return;
        dialog.onDialogStart.AddListener(onDialogStart.Invoke);
        dialog.onDialogContinue.AddListener(onDialogContinue.Invoke);
        dialog.onDialogEnd.AddListener(onDialogEnd.Invoke);
    }

    private void OnDisable()
    {
        if (dialog == null) return;
        dialog.onDialogStart.RemoveListener(onDialogStart.Invoke);
        dialog.onDialogContinue.RemoveListener(onDialogContinue.Invoke);
        dialog.onDialogEnd.RemoveListener(onDialogEnd.Invoke);
    }
}

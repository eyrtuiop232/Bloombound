using UnityEngine;

public class DialogManager : MonoBehaviour
{
    public static DialogManager Instance { get; private set; }

    [SerializeField] private DialogCanvas _canvas;

    private DialogData _current;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    public void StartDialog(DialogData data)
    {
        if (data == null) return;

        _current = data;
        _current.onDialogStart.Invoke();
        _canvas.ShowDialog(data, this);
    }

    // Called by the continue button (no-choice dialogs)
    public void Continue()
    {
        if (_current == null) return;
        if (_current.HasChoices) return;

        if (_current.HasNext)
        {
            _current.onDialogContinue.Invoke();
            StartDialog(_current.nextDialog);
        }
        else
        {
            EndDialog();
        }
    }

    // Called when a choice button is pressed
    public void SelectChoice(DialogData nextDialog)
    {
        if (_current == null) return;

        _current.onDialogContinue.Invoke();

        if (nextDialog != null)
            StartDialog(nextDialog);
        else
            EndDialog();
    }

    private void EndDialog()
    {
        _current.onDialogEnd.Invoke();
        _current = null;
        _canvas.HideDialog();
    }

    public bool IsActive => _current != null;
}

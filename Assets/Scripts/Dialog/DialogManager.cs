using UnityEngine;

public class DialogManager : MonoBehaviour
{
    public static DialogManager Instance { get; private set; }

    private DialogData _current;
    private IDialogDisplay _activeDisplay;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    public void StartDialog(DialogData data, IDialogDisplay display)
    {
        if (data == null) return;

        _activeDisplay = display;
        _current = data;
        _current.onDialogStart.Invoke();
        _activeDisplay.ShowDialog(data, this);
    }

    // Called by the continue button (no-choice dialogs)
    public void Continue()
    {
        if (_current == null) return;
        if (_current.HasChoices) return;

        if (_current.HasNext)
        {
            _current.onDialogContinue.Invoke();
            StartDialog(_current.nextDialog, _activeDisplay);
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
            StartDialog(nextDialog, _activeDisplay);
        else
            EndDialog();
    }

    private void EndDialog()
    {
        _current.onDialogEnd.Invoke();
        _current = null;
        _activeDisplay.HideDialog();
        _activeDisplay = null;
    }

    public bool IsActive => _current != null;
}

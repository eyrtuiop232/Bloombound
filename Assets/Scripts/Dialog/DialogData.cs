using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "NewDialog", menuName = "Dialog/Dialog Data")]
public class DialogData : ScriptableObject
{
    [Header("Content")]
    public string speakerName;
    [TextArea(2, 5)]
    public string dialogText;

    [Header("Progression")]
    public DialogData nextDialog;       // used when there are no choices
    public List<DialogChoice> choices;  // if populated, nextDialog is ignored

    [Header("Events")]
    public UnityEvent onDialogStart;
    public UnityEvent onDialogContinue; // fired when moving to another dialog
    public UnityEvent onDialogEnd;      // fired when dialog chain ends

    public bool HasChoices => choices != null && choices.Count > 0;
    public bool HasNext    => nextDialog != null;
}

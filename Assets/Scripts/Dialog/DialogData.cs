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
    public GameObject typewriterSound;

    [Header("Progression")]
    public DialogData nextDialog;
    public List<DialogChoice> choices;

    [Header("Events")]
    public UnityEvent onDialogStart;
    public UnityEvent onDialogContinue;
    public UnityEvent onDialogEnd;

    public bool HasChoices => choices != null && choices.Count > 0;
    public bool HasNext    => nextDialog != null;
}

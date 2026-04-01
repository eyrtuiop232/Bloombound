using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogCanvas : MonoBehaviour, IDialogDisplay
{
    [Header("Text")]
    public TMP_Text speakerNameText;
    public TMP_Text dialogText;

    [Header("Choices")]
    public Button choicebtnsample;
    public Transform choiceContainer;

    [Header("Navigation")]
    public Button continueButton; // shown when there are no choices

    [Header("Panel")]
    [Tooltip("The child panel to show/hide. Keep the root GameObject always active.")]
    public GameObject dialogPanel;

    private DialogManager _manager;
    private readonly List<Button> _spawnedButtons = new();

    private void Awake()
    {
        choicebtnsample.gameObject.SetActive(false);
        continueButton.onClick.AddListener(OnContinueClicked);
        dialogPanel.SetActive(false);
    }

    public void ShowDialog(DialogData data, DialogManager manager)
    {
        _manager = manager;
        dialogPanel.SetActive(true);

        if (speakerNameText != null)
            speakerNameText.text = data.speakerName;

        if (dialogText != null)
            dialogText.text = data.dialogText;

        ClearChoices();

        if (data.HasChoices)
        {
            continueButton.gameObject.SetActive(false);
            foreach (var choice in data.choices)
                SpawnChoiceButton(choice);
        }
        else
        {
            continueButton.gameObject.SetActive(true);
        }
    }

    public void HideDialog()
    {
        ClearChoices();
        dialogPanel.SetActive(false);
    }

    private void SpawnChoiceButton(DialogChoice choice)
    {
        Button btn = Instantiate(choicebtnsample, choiceContainer);
        btn.gameObject.SetActive(true);

        TMP_Text label = btn.GetComponentInChildren<TMP_Text>();
        if (label != null)
            label.text = choice.choiceText;

        DialogData next = choice.nextDialog;
        btn.onClick.AddListener(() => _manager.SelectChoice(next));

        _spawnedButtons.Add(btn);
    }

    private void ClearChoices()
    {
        foreach (Button btn in _spawnedButtons)
            Destroy(btn.gameObject);
        _spawnedButtons.Clear();
    }

    private void OnContinueClicked()
    {
        _manager.Continue();
    }
}

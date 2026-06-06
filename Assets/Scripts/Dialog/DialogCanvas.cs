using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogCanvas : MonoBehaviour, IDialogDisplay
{
    [Header("Text")]
    public TMP_Text speakerNameText;
    public TMP_Text dialogText;

    [Header("Typewriter")]
    [Tooltip("Characters revealed per second. 0 = instant.")]
    public float typewriterSpeed = 30f;

    [Header("Choices")]
    public Button choicebtnsample;
    public Transform choiceContainer;

    [Header("Navigation")]
    public Button continueButton;
    [Tooltip("Seconds after text finishes appearing before auto-continuing. 0 = wait for button press.")]
    public float autoFinishDialog = 0f;

    [Header("Panel")]
    [Tooltip("The child panel to show/hide. Keep the root GameObject always active.")]
    public GameObject dialogPanel;

    [Header("Player")]
    [Tooltip("Disable the player while this dialog is visible.")]
    public bool disablePlayer = false;

    private Player _player;
    private DialogManager _manager;
    private readonly List<Button> _spawnedButtons = new();
    private Coroutine _typewriterCoroutine;
    private bool _typingDone;

    private void Awake()
    {
        if (choicebtnsample != null)
            choicebtnsample.gameObject.SetActive(false);
        if (continueButton != null)
            continueButton.onClick.AddListener(OnContinueClicked);
        dialogPanel.SetActive(false);

        if (disablePlayer)
            _player = FindObjectOfType<Player>();
    }

    public void ShowDialog(DialogData data, DialogManager manager)
    {
        _manager = manager;
        dialogPanel.SetActive(true);

        if (disablePlayer && _player != null)
            _player.DisablePlayer();

        if (speakerNameText != null)
            speakerNameText.text = data.speakerName;

        StopAllCoroutines();
        _typewriterCoroutine = null;

        ClearChoices();

        if (data.HasChoices)
        {
            if (continueButton != null)
                continueButton.gameObject.SetActive(false);
            foreach (var choice in data.choices)
                SpawnChoiceButton(choice);

            StartTypewriter(data.dialogText, onDone: null);
        }
        else
        {
            bool autoFinish = autoFinishDialog > 0f;

            if (continueButton != null)
                continueButton.gameObject.SetActive(!autoFinish);

            System.Action onDone = autoFinish ? () => StartCoroutine(AutoFinish(autoFinishDialog)) : null;
            StartTypewriter(data.dialogText, onDone);
        }
    }

    public void HideDialog()
    {
        StopAllCoroutines();
        _typewriterCoroutine = null;

        ClearChoices();
        dialogPanel.SetActive(false);

        if (disablePlayer && _player != null)
            _player.EnablePlayer();
    }

    private void StartTypewriter(string text, System.Action onDone)
    {
        if (dialogText == null) return;

        dialogText.text = text;

        if (typewriterSpeed <= 0f)
        {
            dialogText.maxVisibleCharacters = int.MaxValue;
            _typingDone = true;
            onDone?.Invoke();
        }
        else
        {
            _typingDone = false;
            dialogText.maxVisibleCharacters = 0;
            _typewriterCoroutine = StartCoroutine(TypewriterRoutine(onDone));
        }
    }

    private IEnumerator TypewriterRoutine(System.Action onDone)
    {
        dialogText.ForceMeshUpdate();
        int total = dialogText.textInfo.characterCount;
        float delay = 1f / typewriterSpeed;

        for (int i = 1; i <= total; i++)
        {
            dialogText.maxVisibleCharacters = i;
            yield return new WaitForSeconds(delay);
        }

        _typingDone = true;
        _typewriterCoroutine = null;
        onDone?.Invoke();
    }

    private void SkipTypewriter()
    {
        if (_typewriterCoroutine != null)
        {
            StopCoroutine(_typewriterCoroutine);
            _typewriterCoroutine = null;
        }

        if (dialogText != null)
            dialogText.maxVisibleCharacters = int.MaxValue;

        _typingDone = true;
    }

    private IEnumerator AutoFinish(float delay)
    {
        yield return new WaitForSeconds(delay);
        _manager.Continue(this);
    }

    private void SpawnChoiceButton(DialogChoice choice)
    {
        if (choicebtnsample == null) return;

        Button btn = Instantiate(choicebtnsample, choiceContainer);
        btn.gameObject.SetActive(true);

        TMP_Text label = btn.GetComponentInChildren<TMP_Text>();
        if (label != null)
            label.text = choice.choiceText;

        DialogData next = choice.nextDialog;
        btn.onClick.AddListener(() => _manager.SelectChoice(next, this));

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
        if (!_typingDone)
        {
            SkipTypewriter();

            if (autoFinishDialog > 0f)
                StartCoroutine(AutoFinish(autoFinishDialog));

            return;
        }

        _manager.Continue(this);
    }
}

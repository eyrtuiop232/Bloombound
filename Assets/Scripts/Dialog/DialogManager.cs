using System.Collections.Generic;
using UnityEngine;

public class DialogManager : MonoBehaviour
{
    public static DialogManager Instance { get; private set; }

    private class Session
    {
        public DialogData current;
        public List<DialogCanvas> displays;
        public int displayIndex;
        public IDialogDisplay activeDisplay;
    }

    private readonly Dictionary<DialogCanvas, Session> _sessions = new();

    private void Awake()
    {
        if (Instance != null) { Destroy(gameObject); return; }
        Instance = this;
    }

    public bool IsActive => _sessions.Count > 0;
    public bool IsActiveFor(DialogCanvas canvas) => _sessions.ContainsKey(canvas);

    public void StartDialog(DialogData data, List<DialogCanvas> displays)
    {
        if (data == null || displays == null || displays.Count == 0) return;

        DialogCanvas key = displays[0];
        if (_sessions.ContainsKey(key)) return;

        var session = new Session { displays = displays, displayIndex = 0 };
        _sessions[key] = session;
        ShowOnDisplay(key, session, data);
    }

    public void Continue(DialogCanvas caller)
    {
        if (!TryFindSession(caller, out var key, out var session)) return;
        if (session.current.HasChoices) return;

        if (session.current.HasNext)
        {
            session.current.onDialogContinue.Invoke();
            AdvanceDisplay(session);
            ShowOnDisplay(key, session, session.current.nextDialog);
        }
        else
        {
            EndDialog(key, session);
        }
    }

    public void SelectChoice(DialogData nextDialog, DialogCanvas caller)
    {
        if (!TryFindSession(caller, out var key, out var session)) return;

        session.current.onDialogContinue.Invoke();

        if (nextDialog != null)
        {
            AdvanceDisplay(session);
            ShowOnDisplay(key, session, nextDialog);
        }
        else
        {
            EndDialog(key, session);
        }
    }

    private void ShowOnDisplay(DialogCanvas key, Session session, DialogData data)
    {
        IDialogDisplay next = session.displays[session.displayIndex];

        if (session.activeDisplay != null && session.activeDisplay != next)
            session.activeDisplay.HideDialog();

        session.activeDisplay = next;
        session.current = data;
        session.current.onDialogStart.Invoke();
        session.activeDisplay.ShowDialog(data, this);
    }

    private void AdvanceDisplay(Session session)
    {
        if (session.displays.Count > 1)
            session.displayIndex = (session.displayIndex + 1) % session.displays.Count;
    }

    private void EndDialog(DialogCanvas key, Session session)
    {
        var ended = session.current;
        session.current = null;
        session.activeDisplay.HideDialog();
        session.activeDisplay = null;
        _sessions.Remove(key);
        ended.onDialogEnd.Invoke();
    }

    private bool TryFindSession(DialogCanvas caller, out DialogCanvas key, out Session session)
    {
        foreach (var kvp in _sessions)
        {
            if (kvp.Value.displays.Contains(caller))
            {
                key = kvp.Key;
                session = kvp.Value;
                return true;
            }
        }
        key = null;
        session = null;
        return false;
    }
}

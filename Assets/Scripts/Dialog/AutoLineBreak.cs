using System.Text;
using TMPro;
using UnityEngine;

public class AutoLineBreak : MonoBehaviour
{
    [SerializeField] private TMP_Text target;
    public int charsPerLine = 30;

    private string _lastFormatted;

    private void Awake()
    {
        if (target == null)
            target = GetComponent<TMP_Text>();
    }

    private void Update()
    {
        if (target == null || target.text == _lastFormatted) return;

        _lastFormatted = Format(target.text, charsPerLine);
        target.text = _lastFormatted;
    }

    public void Apply(string text)
    {
        if (target == null) return;

        _lastFormatted = Format(text, charsPerLine);
        target.text = _lastFormatted;
    }

    public static string Format(string text, int maxChars)
    {
        if (string.IsNullOrEmpty(text) || text.Length <= maxChars)
            return text;

        StringBuilder sb = new();
        int lineStart = 0;

        while (lineStart < text.Length)
        {
            int remaining = text.Length - lineStart;
            if (remaining <= maxChars)
            {
                sb.Append(text, lineStart, remaining);
                break;
            }

            int breakAt = -1;
            for (int i = lineStart + maxChars; i > lineStart; i--)
            {
                if (text[i] == ' ')
                {
                    breakAt = i;
                    break;
                }
            }

            if (breakAt == -1)
            {

                int next = text.IndexOf(' ', lineStart + maxChars);
                if (next == -1)
                {
                    sb.Append(text, lineStart, remaining);
                    break;
                }
                breakAt = next;
            }

            sb.Append(text, lineStart, breakAt - lineStart);
            sb.Append('\n');
            lineStart = breakAt + 1;
        }

        return sb.ToString();
    }
}

using UnityEngine;
using UnityEngine.Events;

public class WinCondition : MonoBehaviour
{
    public int clearedMinigame = 0;
    public int winAt = 0;

    public UnityEvent onWin;
    public void Increase()
    {
        clearedMinigame += 1;
        if (clearedMinigame >= winAt)
        {
            Win();
        }
    }

    void Win()
    {
        onWin.Invoke();
    }
}

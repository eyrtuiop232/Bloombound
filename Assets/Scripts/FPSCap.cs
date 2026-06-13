using UnityEngine;

public class FPSCap : MonoBehaviour
{
    [SerializeField] private int targetFPS = 60;

    void Awake()
    {
        QualitySettings.vSyncCount = 0; 
        Application.targetFrameRate = targetFPS;
    }
}

using UnityEngine;

public class TransparencySortFix : MonoBehaviour
{
    void Awake()
    {
        Camera.main.transparencySortMode = TransparencySortMode.CustomAxis;
        Camera.main.transparencySortAxis = new Vector3(0f, 1f, 0f);
    }
}
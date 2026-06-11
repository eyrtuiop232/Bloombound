using UnityEngine;

public class CameraFloat : MonoBehaviour
{
    public float MoveAmount = 0.1f;
    public float MoveSpeed = 0.5f;

    private Vector3 _originPos;

    private void Awake()
    {
        _originPos = transform.localPosition;
    }

    private void Update()
    {
        float x = Mathf.Cos(Time.time * MoveSpeed) * MoveAmount;
        float y = Mathf.Sin(Time.time * MoveSpeed) * MoveAmount;
        transform.localPosition = _originPos + new Vector3(x, y, 0f);
    }
}

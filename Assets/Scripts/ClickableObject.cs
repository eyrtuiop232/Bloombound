using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Collider2D))]
public class ClickableObject : MonoBehaviour
{
    public UnityEvent OnClick;
    public UnityEvent OnMouseEnterEvent;
    public UnityEvent OnMouseLeaveEvent;

    private Collider2D _collider;
    private bool _isHovered;

    private void Awake()
    {
        _collider = GetComponent<Collider2D>();
    }

    private void Update()
    {
        Vector2 mouseWorldPos = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        bool isOver = _collider.OverlapPoint(mouseWorldPos);

        if (isOver && !_isHovered)
        {
            _isHovered = true;
            OnMouseEnterEvent?.Invoke();
        }
        else if (!isOver && _isHovered)
        {
            _isHovered = false;
            OnMouseLeaveEvent?.Invoke();
        }

        if (isOver && Mouse.current.leftButton.wasPressedThisFrame)
        {
            OnClick?.Invoke();
        }
    }
}

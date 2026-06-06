using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(BoxCollider2D))]
public class PlayerZone : MonoBehaviour
{
    public UnityEvent onEnter;
    public UnityEvent onLeave;

    private bool _playerInside;

    private void Awake()
    {
        GetComponent<BoxCollider2D>().isTrigger = true;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (_playerInside || other.GetComponent<Player>() == null) return;
        _playerInside = true;
        onEnter.Invoke();
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!_playerInside || other.GetComponent<Player>() == null) return;
        _playerInside = false;
        onLeave.Invoke();
    }
}

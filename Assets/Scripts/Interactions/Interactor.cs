using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Player))]
public class Interactor : MonoBehaviour
{
    private Interaction _current;
    private Player _player;

    private void Awake()
    {
        _player = GetComponent<Player>();
    }

    private void Update()
    {
        if (_current == null) return;
        if (_player != null && _player.State == PlayerState.Disabled) return;

        if (Keyboard.current.eKey.wasPressedThisFrame)
            _current.Interact(gameObject);
    }

    public void SetInteractable(Interaction interaction)
    {
        _current = interaction;
    }

    public void ClearInteractable(Interaction interaction)
    {
        if (_current == interaction)
            _current = null;
    }
}

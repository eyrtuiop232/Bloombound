using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Player))]
public class Interactor : MonoBehaviour
{
    public GameObject interactIndicator;

    private Interaction _current;
    private Player _player;

    private void Awake()
    {
        _player = GetComponent<Player>();
        if (interactIndicator != null)
            interactIndicator.SetActive(false);
    }

    private void Update()
    {
        if (_current != null && !_current.isEnabled)
            _current = null;

        bool playerDisabled = _player != null && _player.State == PlayerState.Disabled;
        bool canInteract = _current != null && !playerDisabled;

        if (interactIndicator != null)
            interactIndicator.SetActive(canInteract);

        if (!canInteract) return;

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

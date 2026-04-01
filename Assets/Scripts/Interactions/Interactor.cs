using UnityEngine;
using UnityEngine.InputSystem;

public class Interactor : MonoBehaviour
{
    private Interaction _current;

    private void Update()
    {
        if (_current == null) return;

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

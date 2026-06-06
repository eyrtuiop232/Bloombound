using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Player))]
public class Interactor : MonoBehaviour
{
    public GameObject interactIndicator;

    private readonly List<Interaction> _inRange = new();
    private Interaction _nearest;
    private Player _player;

    private void Awake()
    {
        _player = GetComponent<Player>();
        if (interactIndicator != null)
            interactIndicator.SetActive(false);
    }

    private void Update()
    {
        _inRange.RemoveAll(i => i == null || !i.isEnabled || !i.enabled);
        _nearest = GetNearest();

        bool playerDisabled = _player != null && _player.State == PlayerState.Disabled;
        bool canInteract = _nearest != null && !playerDisabled;

        if (interactIndicator != null)
            interactIndicator.SetActive(canInteract);

        if (!canInteract) return;

        if (Keyboard.current.eKey.wasPressedThisFrame)
            InteractWithNearest();
    }

    private Interaction GetNearest()
    {
        if (_inRange.Count == 0) return null;

        Interaction nearest = null;
        float nearestDist = float.MaxValue;

        foreach (Interaction interaction in _inRange)
        {
            float dist = Vector2.Distance(transform.position, interaction.transform.position);
            if (dist < nearestDist)
            {
                nearestDist = dist;
                nearest = interaction;
            }
        }

        return nearest;
    }

    private void InteractWithNearest()
    {
        GameObject target = _nearest.gameObject;
        foreach (Interaction interaction in _inRange)
        {
            if (interaction.gameObject == target)
                interaction.Interact(gameObject);
        }
    }

    public void SetInteractable(Interaction interaction)
    {
        if (!_inRange.Contains(interaction))
            _inRange.Add(interaction);
    }

    public void ClearInteractable(Interaction interaction)
    {
        _inRange.Remove(interaction);
    }
}

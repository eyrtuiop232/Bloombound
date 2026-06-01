using UnityEngine;
using UnityEngine.Events;

public class RantingInteraction : Interaction
{
    public UnityEvent interactStuffs;
    public bool isEnabled = true;

    [Header("Fly Follow")]
    public Vector3 followOffset = new Vector3(0f, 1.5f, 0f);
    public float followSpeed = 5f;

    private Transform _followTarget;

    public override void Interact(GameObject interactor)
    {
        if (!isEnabled) return;

        interactStuffs.Invoke();
        _followTarget = interactor.transform;
    }

    private void Update()
    {
        if (_followTarget == null) return;

        Vector3 targetPos = _followTarget.position + followOffset;
        transform.position = Vector3.Lerp(transform.position, targetPos, followSpeed * Time.deltaTime);
    }

    public void disable() => isEnabled = false;
    public void enable()  => isEnabled = true;
}

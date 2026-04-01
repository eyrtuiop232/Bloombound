using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class Interaction : MonoBehaviour
{
    private Collider2D m_collider;

    protected virtual void Start()
    {
        m_collider = GetComponent<Collider2D>();
        m_collider.isTrigger = true;
    }

    // Called by Interactor when the player presses the interact key
    public virtual void Interact(GameObject interactor)
    {
        print(interactor.gameObject.name+" is interacting with "+gameObject.name);
    }

    // Called when an interactor enters range
    public virtual void OnEnterRange(GameObject interactor)
    {
    }

    // Called when an interactor exits range
    public virtual void OnExitRange(GameObject interactor)
    {
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log($"[Interaction] OnTriggerEnter2D fired. Other: {other.gameObject.name}");

        Interactor interactor = other.GetComponent<Interactor>();
        if (interactor == null)
        {
            Debug.Log($"[Interaction] No Interactor component found on {other.gameObject.name}");
            return;
        }

        interactor.SetInteractable(this);
        OnEnterRange(other.gameObject);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        Interactor interactor = other.GetComponent<Interactor>();
        if (interactor == null) return;

        interactor.ClearInteractable(this);
        OnExitRange(other.gameObject);
    }
}

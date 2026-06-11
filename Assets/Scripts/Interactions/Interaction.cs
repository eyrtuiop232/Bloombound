using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class Interaction : MonoBehaviour
{
    public bool isEnabled = true;

    private Collider2D m_collider;

    protected virtual void Start()
    {
        m_collider = GetComponent<Collider2D>();
        m_collider.isTrigger = true;
    }

    public void enable()  => isEnabled = true;
    public void disable() => isEnabled = false;

    public virtual void Interact(GameObject interactor)
    {
        print(interactor.gameObject.name+" is interacting with "+gameObject.name);
    }

    public virtual void OnEnterRange(GameObject interactor)
    {
    }

    public virtual void OnExitRange(GameObject interactor)
    {
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log($"[Interaction] OnTriggerEnter2D fired. Other: {other.gameObject.name}");

        if (!isEnabled || !enabled) return;

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

using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class MovementSystem : MonoBehaviour
{
    public float movespeed = 10f;
    public float movespeed_mod = 0f;
    public Vector2 movedir = new Vector2(0, 0);

    protected Rigidbody2D rb;

    protected virtual void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    protected virtual void FixedUpdate()
    {
        Move();
    }

    protected virtual void Move()
    {
        if (rb == null) return;
        rb.linearVelocity = movedir.normalized * movespeed;
    }

    public void SetMoveDirection(Vector2 direction)
    {
        movedir = direction;
    }

    public void Stop()
    {
        movedir = Vector2.zero;
        if (rb != null)
            rb.linearVelocity = Vector2.zero;
    }

    public void moveSpeedModifier(float n)
    {
        movespeed_mod = n;
    }
}

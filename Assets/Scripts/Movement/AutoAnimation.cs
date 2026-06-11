using UnityEngine;
using UnityEngine.AI;

public class AutoAnimation : MonoBehaviour
{
    public Animator targetAnimator;
    public MovementSystem movement;
    public NavMeshAgent agent;

    private int _hashX;
    private int _hashY;
    private int _hashIsMoving;

    void Start()
    {
        _hashX = Animator.StringToHash("X");
        _hashY = Animator.StringToHash("Y");
        _hashIsMoving = Animator.StringToHash("IsMoving");
    }

    void Update()
    {
        Vector2 dir;

        if (movement != null)
        {
            dir = movement.movedir;
        }
        else if (agent != null)
        {
            dir = new Vector2(agent.velocity.x, agent.velocity.y);
        }
        else return;

        if (dir == Vector2.zero)
        {
            targetAnimator.SetBool(_hashIsMoving, false);
        }
        else
        {
            targetAnimator.SetBool(_hashIsMoving, true);
            dir = dir.normalized;
            targetAnimator.SetFloat(_hashX, dir.x);
            targetAnimator.SetFloat(_hashY, dir.y);
        }
    }
}

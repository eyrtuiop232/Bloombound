using UnityEngine;
using UnityEngine.AI;

public class CompanionFSM : MonoBehaviour
{
    NavMeshAgent agent;
    public Transform player;
    [SerializeField] private string CurrentState = "Idle";
    public float StopDistance = 10f;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
    }

    void StopImmediately()
    {
        agent.isStopped = true;
        agent.SetDestination(Vector2.zero);
        agent.velocity = Vector3.zero;
    }

    void Update()
    {
        float Distance = Vector2.Distance(transform.position, player.position);
        switch (CurrentState)
        {
            case "Idle":
                if (Distance > StopDistance)
                {
                    CurrentState = "Follow";
                }
                StopImmediately();
                break;

            case "Follow":
                if (Distance <= StopDistance)
                {
                    CurrentState = "Idle";
                }
                agent.SetDestination(player.position);
                Vector3 nextWaypointPosition = agent.steeringTarget;
                Debug.DrawLine(transform.position, nextWaypointPosition, Color.red);
                break;

            default:
                break;
        }
    }
}
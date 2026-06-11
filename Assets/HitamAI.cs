using UnityEngine;
using UnityEngine.AI;

public class HitamAI : MonoBehaviour
{
    NavMeshAgent agent;
    public Transform player;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
    }

    void Update()
    {
        agent.SetDestination(player.position);

        Vector3 nextWaypointPosition = agent.steeringTarget;

        Debug.DrawLine(transform.position, nextWaypointPosition, Color.red);

        print("Next Waypoint Position: " + agent.nextPosition);
    }
}

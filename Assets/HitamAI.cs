using UnityEngine;
using UnityEngine.AI;

public class HitamAI : MonoBehaviour
{
    NavMeshAgent agent;
    public Transform player;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
    }

    void Update()
    {
        agent.SetDestination(player.position);
        // Get the position of the next waypoint (steering target)
        Vector3 nextWaypointPosition = agent.steeringTarget;

        // You can use this position for various purposes, e.g., drawing a debug line
        Debug.DrawLine(transform.position, nextWaypointPosition, Color.red);

        print("Next Waypoint Position: " + agent.nextPosition);
    }
}

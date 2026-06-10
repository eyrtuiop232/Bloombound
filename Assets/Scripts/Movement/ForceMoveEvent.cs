using UnityEngine;

public class ForceMoveEvent : MonoBehaviour
{
    public Vector2 TargetPosition;
    Player player;

    void Start()
    {
        player = FindFirstObjectByType<Player>();
    }
    public void TriggerForceMove()
    {
        player.ForceMoveTo(TargetPosition);
    }
}

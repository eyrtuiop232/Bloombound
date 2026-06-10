using UnityEngine;
using UnityEngine.InputSystem;

public enum PlayerState { Enabled, Disabled }

public class Player : MovementSystem
{
    public float RunningSpeed = 20f;
    public PlayerState State = PlayerState.Enabled;
    public Vector2 forceMove = Vector2.zero;

    private float _baseSpeed;
    private const float _forceMoveArrivalThreshold = 0.1f;

    public void EnablePlayer()  => State = PlayerState.Enabled;
    public void DisablePlayer() => State = PlayerState.Disabled;

    protected override void Start()
    {
        base.Start();
        _baseSpeed = movespeed;
    }

    protected override void FixedUpdate()
    {
        ReadInput();
        base.FixedUpdate();
    }

    public void ForceMoveTo(Vector2 targetPosition)
    {
        forceMove = targetPosition;
    }

    private void ReadInput()
    {
        if (forceMove != Vector2.zero)
        {
            Vector2 toTarget = forceMove - (Vector2)transform.position;
            if (toTarget.magnitude <= _forceMoveArrivalThreshold)
            {
                forceMove = Vector2.zero;
                Stop();
                return;
            }
            movespeed = _baseSpeed + movespeed_mod;
            SetMoveDirection(toTarget);
            return;
        }

        if (State == PlayerState.Disabled)
        {
            SetMoveDirection(Vector2.zero);
            return;
        }

        Vector2 input = Vector2.zero;

        if (Keyboard.current.wKey.isPressed) input.y += 1f;
        if (Keyboard.current.sKey.isPressed) input.y -= 1f;
        if (Keyboard.current.dKey.isPressed) input.x += 1f;
        if (Keyboard.current.aKey.isPressed) input.x -= 1f;

        movespeed = Keyboard.current.leftShiftKey.isPressed ? RunningSpeed : _baseSpeed;
        movespeed += movespeed_mod;
        SetMoveDirection(input);
    }
}

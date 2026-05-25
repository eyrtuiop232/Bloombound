using UnityEngine;
using UnityEngine.InputSystem;

public enum PlayerState { Enabled, Disabled }

public class Player : MovementSystem
{
    public float RunningSpeed = 20f;
    public PlayerState State = PlayerState.Enabled;

    private float _baseSpeed;

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

    private void ReadInput()
    {
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

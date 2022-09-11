using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
public class CC_ProcessInputByState : MonoBehaviour
{
    [SerializeField] CC_InputControl inputControl;
    [SerializeField] CC_CapsuleOverlapFlags2D colliderInfo;

    [Header("Stats")]
    [SerializeField] private float moveSpeed = 4;
    [SerializeField] private float jumpSpeed = 10;

    [Header("JumpSpecial")]
    [SerializeField] private float jumpForgiveness = 0.25f;
    [SerializeField] private float jumpForgiveCounter = 0.25f;
    [SerializeField] private float normalGravity = 30;
    [SerializeField] private float fallGravity = 60;
    [SerializeField] private float jumpGravity = 20;

    [Header("Restrictions")]
    [SerializeField] private bool canMove;
    [SerializeField] private bool canWallJump;
    [SerializeField] private bool canJump;
    [SerializeField] private bool canClimb;
    [SerializeField] private bool applyGravity;

    [Header("States")]
    [SerializeField] private bool grounded;
    [SerializeField] private bool isDucking;
    [SerializeField] private bool isClimbing;
    [SerializeField] private Vector2 velocityAxis;

    public enum States { Idle, Walking, JumpUp, Falling, WallJump, Climbing }
    public States CharacterState = States.Idle;

    private void Start()
    {
        inputControl = GetComponent<CC_InputControl>();
        colliderInfo = GetComponent<CC_CapsuleOverlapFlags2D>();
    }

    public void SetRefrences(CC_InputControl inputControl, CC_CapsuleOverlapFlags2D colliderInfo)
    {
        //  this.inputControl = inputControl;
        //  this.colliderInfo = colliderInfo;
    }

    private void FixedUpdate()
    {
        MovementState();
    }

    public void MovementState()
    {
        jumpForgiveCounter -= Time.fixedDeltaTime;

        colliderInfo.CheckColliderFlags();

        CharacterState = States.Idle;

        if (canMove)
        {
            if (colliderInfo.grounded && velocityAxis.y < 0)
            {
                jumpForgiveCounter = jumpForgiveness;
                velocityAxis = GroundMovement();
                CharacterState = States.Walking;
            }

            else
            {
                if (canWallJump && inputControl.inputHoldJumpButton && colliderInfo.hittingWall)
                {
                    velocityAxis = WallJump(velocityAxis, colliderInfo.hittingWallRight);
                    CharacterState = States.WallJump;
                }
                velocityAxis = AirMovement(velocityAxis);
            }
        }

        if (canJump && inputControl.CheckJumpAndReset() && jumpForgiveCounter > 0)
        {
            velocityAxis = JumpMovement(velocityAxis);
            CharacterState = States.JumpUp;
        }

        if (canClimb)
        {
            velocityAxis = ClimbMovement(velocityAxis);
            if (inputControl.inputAxis.y != 0)
                CharacterState = States.Climbing;
        }

        if (applyGravity)
            velocityAxis = ApplyGravity(velocityAxis);


        Debug.DrawRay(transform.position, velocityAxis);

        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        rb.velocity = velocityAxis;
    }

    Vector2 GroundMovement()
    {
        if (inputControl.inputDuckButton)
            isDucking = true;
        else
            isDucking = false;

        if (isDucking)
            return Vector2.zero;

        Vector2 moveDir = inputControl.inputAxis * moveSpeed;
        moveDir.y = 0;

        return moveDir;
    }

    Vector2 WallJump(Vector2 axis, bool right)
    {
        Vector2 wallJump = axis;
        wallJump = Vector2.one * jumpSpeed;
        if (right)
        {
            wallJump.x *= -1;
        }
        return wallJump;
    }

    Vector2 JumpMovement(Vector2 axis)
    {
        print("Jump");
        jumpForgiveCounter = 0;
        axis += Vector2.up * jumpSpeed;
        return axis;
    }

    Vector2 ClimbMovement(Vector2 axis)
    {
        if (inputControl.inputClimbButton)
        {
            isClimbing = true;
        }

        if (isClimbing)
        {
            axis = inputControl.inputAxis * moveSpeed;
        }

        return axis;
    }

    Vector2 ApplyGravity(Vector2 axis)
    {
        float grav = normalGravity;

        if (inputControl.inputJumpButton)
            grav = jumpGravity;

        axis -= Vector2.up * grav * Time.fixedDeltaTime;
        return axis;
    }

    Vector2 AirMovement(Vector2 axis)
    {
        return axis;
    }
}
*/
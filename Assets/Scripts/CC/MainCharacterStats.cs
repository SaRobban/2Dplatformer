using UnityEngine;

public class MainCharacterStats : MonoBehaviour
{
    [Header("Grounded")]
    [SerializeField] private float walkSpeed = 4;
    [SerializeField] private float runSpeed = 8;
    [SerializeField] private float walkDeadZone = 0.1f;
    [SerializeField] private float killMomentum = 8;
    [Header("AirBorne")]
    [SerializeField] private float airControl = 1;
    [Header("Jump")]
    [SerializeField] private float jumpStr = 10;
    [SerializeField] private int dubbleJumps = 0;
    [SerializeField] private float jumpForgiveness = 0.25f;
    [Header("Gravity")]
    [SerializeField] private float normalGravity = 20;
    [SerializeField] private float jumpHoldGravity = 15;
    [SerializeField] private float jumpFallGravity = 40;
    [Header("Wall")]
    [SerializeField] private float wallSlideAcc = 2f;
    [SerializeField] private float maxWallSlideSpeed = 2f;
    [SerializeField] private float wallJumpYStr = 10f;
    [SerializeField] private float wallJumpXStr = 5f;
    [Header("Physics")]
    [SerializeField] private float halfWidth = 0.25f;
    [SerializeField] private float waistHight = 0.75f;
    [SerializeField] private float hangHight = 0.98f;
    [SerializeField] private float skinWidth = 0.02f;
    [SerializeField] private float alignUpSpeed = 4f;

    //Public pointers to accsess private variables form exernal script without screving with unity inpector.
    public float WalkSpeed => walkSpeed;
    public float RunSpeed => runSpeed;
    public float AirControl => airControl;
    public float JumpStr => jumpStr;
    public float NormalGravity => normalGravity;
    public float JumpHoldGravity => jumpHoldGravity;
    public float JumpFallGravity => jumpFallGravity;
    public float JumpForgiveness => jumpForgiveness;
    public int DubbleJumps => dubbleJumps;
    public float WallSlideAcc => wallSlideAcc;
    public float MaxWallSlideSpeed => maxWallSlideSpeed;
    public float WallJumpYStr => wallJumpYStr;
    public float WallJumpXStr => wallJumpXStr;

    public float HalfWidth => halfWidth;
    public float WaistHight => waistHight;
    public float HangHight => hangHight;
    public float SkinWidth => skinWidth;

    public float WalkDeadZone => walkDeadZone;
    public float KillMomentum => killMomentum;

    public float AlignUpSpeed => alignUpSpeed;
    public void SetDubbleJumps(int num)
    {
        dubbleJumps = num;
    }
}

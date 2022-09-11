using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(MainCharacterStats))]

public class MainCharacter : MonoBehaviour
{
    [Header("Stats")]
    [SerializeField] public MainCharacterStats stats;
    [HideInInspector] private int dubbleJumpAvailable;
    [HideInInspector] public CC_ColliderFlags flags { get; private set; }
    [HideInInspector] public Rigidbody2D rb;
    [HideInInspector] public CC_InputControl input { get; private set; }
    [HideInInspector] private StateMachine stateMachine;
    [HideInInspector] public CommonStateFunctions stateFunc;

    [HideInInspector] public CC_Aim aim;

    [Header("Dependencies")]
    [SerializeField] public SpriteRenderer sprite;
    [SerializeField] public Animator anim;
    [SerializeField] public CC_Souds sounds;
//    [HideInInspector] public bool useJumpFrogiveness;
    public bool enabledPixelPerfect;

    private string lastAnimationState;

    [Header("LightnignMoveFX")]
    [SerializeField] public GameObject lightningFX;

    private void Awake()
    {
        //SetUp
        stats = GetComponent<MainCharacterStats>();
        rb = GetComponent<Rigidbody2D>();
        sprite = GetComponentInChildren<SpriteRenderer>();
        anim = GetComponentInChildren<Animator>();
        sounds = GetComponentInChildren<CC_Souds>();

        input = new CC_InputControl(stats.JumpForgiveness);
        flags = new CC_ColliderFlags(gameObject.GetComponent<PolygonCollider2D>(), this.transform);
        stateMachine = new StateMachine(this);
        aim = new CC_Aim(rb, this);

        //Init
        stateMachine.ChangeState<CC_Walk>();

        lightningFX.SetActive(false);
    }

    private void Start()
    {
        CameraFunction.instance.TargetOne(transform);
    }

    private void Update()
    {
        input.UpdateController();
        AlignUp();
    }

    private void FixedUpdate()
    {
        flags.CheckColliderFlags();
        Crush();
        aim.UpdateAim();
        stateMachine.Execute();
        rb.velocity += addVelocity;
        addVelocity = Vector2.zero;

        input.ResetController();
    }

    private void LateUpdate()
    {
        if (!enabledPixelPerfect)
            return;

        PixelPerfectSpritePosition();
    }

    /// <summary>
    /// New
    /// </summary>
    private void AlignUp()
    {
        if (transform.up == Vector3.up)
            return;

        float angle = Vector2.Angle(transform.up, Vector3.up);
        angle = Mathf.Clamp(angle, 0, angle* stats.AlignUpSpeed * Time.deltaTime);
        transform.Rotate(Vector3.forward, angle);
    }

    public void ChangeStateTo<newState>() where newState : ICharacterState
    {
        stateMachine.ChangeState<newState>();
    }

    public Vector2 GetPosition()
    {
        return transform.position;
    }

    public Vector2 GetVelocity()
    {
        return rb.velocity;
    }

    private Vector2 addVelocity = Vector2.zero;


    public void SetVelocityTo(Vector2 to)
    {
        rb.velocity = to;
    }

    public void TeleportTo(Vector2 pos)
    {
        rb.MovePosition(pos);
    }

    public void Crush()
    {
        if (flags.Grounded && flags.HittingRoof)
            print("Crush");

        if (flags.HittingWallLeft && flags.HittingWallRight)
            print("Crush");
    }

    public bool GetDrop()
    {
        return input.GetIfDrop();
    }

    public void SetAnimationTo(string animation)
    {
        lastAnimationState = animation;
        anim.Play(animation);
    }

    public string GetAnimationState()
    {
        return lastAnimationState;
    }

    void PixelPerfectSpritePosition()
    {
        Vector3 pos = transform.position;
        pos *= 32;
        pos.x = Mathf.Round(pos.x);
        pos.y = Mathf.Round(pos.y);
        pos.z = Mathf.Round(pos.z);
        pos /= 32;
        sprite.transform.position = pos + Vector3.up * stats.WaistHight;

    }

    public bool UseDubbleJump()
    {
        if (dubbleJumpAvailable > 0)
        {
            dubbleJumpAvailable--;
            return true;
        }
        return false;
    }
    public void ResetDubbleJumps()
    {
        dubbleJumpAvailable = stats.DubbleJumps;
    }
}
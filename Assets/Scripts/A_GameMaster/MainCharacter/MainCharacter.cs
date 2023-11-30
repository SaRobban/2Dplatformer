using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(MainCharacterStats))]

public class MainCharacter : MonoBehaviour, GameMasterUpdate
{
    [Header("Stats")]
    [SerializeField] public MainCharacterStats stats;
    [HideInInspector] private int dubbleJumpAvailable;
    [HideInInspector] public CC_ColliderFlags flags { get; private set; }
    [HideInInspector] public CC_InputControl input { get; private set; }
    [HideInInspector] private StateMachine stateMachine;
    [HideInInspector] public CommonStateFunctions stateFunc;

    [HideInInspector] public CC_Aim aim;
    [HideInInspector] public CC_Health healthSystem;

    [Header("Dependencies")]
    [SerializeField] public SpriteRenderer sprite;
    [SerializeField] public Animator anim;
    [SerializeField] public CC_Souds sounds;
    [HideInInspector] public Rigidbody2D rb;
    [SerializeField] public Tail tail;

    //TODO : When is this used???
    //Used  : by teleporters
    public string lastAnimationName { get; private set; }
    public System.Action<string> AnimationSwitch;
    public bool enabledPixelPerfect;

    private string lastAnimationState;

    public event System.Action A_OnInteract;
    public event System.Action<Item> A_OnUseSpecialItem;

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

        healthSystem = new CC_Health(this);

        

        //Init
        stateMachine.ChangeState<CC_Walk>();

        if (stats.LastSpawnPoint == null)
        {
            Debug.LogError("<color=red>No spawnPoint</color>");
            stats.SetSpawnPoint(
                new SpawnLocation(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name,
                transform.position
            ));
        }
    }

    private void Start()
    {
       

        GameMaster.A_OnFreezeScene += GM_FreezeGame;
        GameMaster.A_OnUnFreezeScene += GM_UnFreezeGame;
    }

    private void OnDestroy()
    {
        GameMaster.A_OnFreezeScene -= GM_FreezeGame;
        GameMaster.A_OnUnFreezeScene -= GM_UnFreezeGame;
    }

    public void Update()
    {
        input.UpdateController();
        AlignUp();
    }

    public void FixedUpdate()
    {
        flags.CheckColliderFlags();
        Crush();
        aim.UpdateAim();
        stateMachine.Execute();
        rb.velocity += addVelocity;
        addVelocity = Vector2.zero;

        input.ResetController();
    }

    public void LateUpdate()
    {
        if (!enabledPixelPerfect)
            return;

        PixelPerfectSpritePosition();
    }

  
    public void GM_OnPause()
    {
        ChangeStateTo<CC_FreezePlayer>();
    }

    public void GM_OnUnPause()
    {
        ChangeStateTo<CC_Walk>();
    }

    public void GM_FreezeGame()
    {
        ChangeStateTo<CC_FreezePlayer>();
    }

    public void GM_UnFreezeGame()
    {
        ChangeStateTo<CC_Walk>();
    }
    /// <summary>
    /// New
    /// </summary>
    private void AlignUp()
    {
        if (transform.up == Vector3.up)
            return;

        float angle = Vector2.Angle(transform.up, Vector3.up);
        angle = Mathf.Clamp(angle, 0, angle * stats.AlignUpSpeed * Time.deltaTime);
        transform.Rotate(Vector3.forward, angle);
    }

    public void ChangeStateTo<newState>() where newState : ICharacterState
    {
        stateMachine.ChangeState<newState>();
    }
    public void ChangeStateTo(ICharacterState newState)
    {
        stateMachine.ChangeState(newState);
    }
    public ICharacterState GetCurrentState()
    {
        return stateMachine.currentState;
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

    /// <summary>
    /// TODO : ADD AS PLAYER IS TOUTCHING COLLIDER??
    /// Checks if player is pushing down
    /// </summary>
    public bool GetDrop()
    {
        return input.GetIfDrop();
    }

    public void SetAnimationTo(string animation)
    {
        lastAnimationState = animation;
        anim.Play(animation);
        AnimationSwitch?.Invoke(animation);
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
        sprite.transform.position = pos + Vector3.up * stats.WaistHeight;

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


    //Item Actions
    public void InvokeInteract()
    {
        A_OnInteract?.Invoke();
    }

    public void OnUseSpecialItem(Item item)
    {
        A_OnUseSpecialItem?.Invoke(item);
    }

    public void RevivePlayerAtLastCheckPoint()
    {
        if (stats.LastSpawnPoint == null)
        {
            Debug.LogError("No spawnPoint");
            return;
        }

        if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name == stats.LastSpawnPoint.sceneName)
        {
            Debug.Log("Last Location : " + stats.LastSpawnPoint.sceneName + "  " + stats.LastSpawnPoint.location);
            Debug.Log("my Location : " + transform.position);
            TeleportTo(stats.LastSpawnPoint.location);

            //Warning : Telepot does not work until nextframe;
            transform.position = stats.LastSpawnPoint.location;
            healthSystem.SetHPToFull();
        }
    }

    public void AddDamage(int dmg)
    {
        healthSystem.AddHP(dmg * -1);
        ChangeStateTo<CC_Hurt>();
    }

    public void DisableCollision()
    {
        flags.GetCollider().enabled = false;
    }
    public void EnableCollision()
    {
        flags.GetCollider().enabled = true;
    }

  
}
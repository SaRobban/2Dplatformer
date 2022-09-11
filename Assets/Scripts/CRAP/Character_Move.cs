using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character_Move : MonoBehaviour
{
    [Header("Controls")]
    private Vector2 axis;
    private bool jump;
    [SerializeField] private float jumpForgiveness = 0.25f;
    private float jumpForgCounter = 0.25f;

    [Header("Physics")]
    private Rigidbody2D rb;
    [Tooltip("Sphare for circlecast")]
    public GameObject gRayCenter;
    private float gRayRadius = 0.5f;
    private float gRayInnerRadius = 0.5f;
    private Renderer gRenderer;

    [Tooltip("Layer mask gound")]
    public LayerMask groundLayer;

    [Header("States")]
    public bool grounded;
    public bool airControl;
    public bool hurt;

    public bool canClimbX;
    public bool canClimbY;
    public bool climbing;

    public Transform climbCenter;

    public Vector2 movement;
    public Vector2 motion; //movement and motion is seperated for animator

    [Header("Movement tweek")]
    [Tooltip("How fast should player center to climbeble object")]
    public float climbCenterSpeed = 0.5f;

    public float runSpeed = 2;
    public float jumpStrength = 10;
    
    private float gravityCurrent;
    public float gravityNormal = 30;
    public float gravityLow = 20;
    public float gravityHi = 50;

    [Header("External Forces")]
    public Vector2 addForce;
    public Vector2 overrideMovement;

    public bool teleport;
    public Vector2 telepotDestination;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        gravityCurrent = gravityNormal;

        //Ground Ray stuff
        gRenderer = gRayCenter.GetComponent<Renderer>();
        gRayRadius = gRenderer.bounds.extents.y;
        float ccBottom = transform.position.y - gameObject.GetComponent<CapsuleCollider2D>().bounds.extents.y;
        gRayInnerRadius = gRayCenter.transform.position.y - ccBottom;

    }

    // Update is called once per frame
    void Update()
    {
        axis.x = Input.GetAxis("Horizontal");
        axis.y = Input.GetAxis("Vertical");

        //Adv jump
        jumpForgCounter -= Time.deltaTime;

        if (Input.GetButtonDown("Jump"))
        {
            jump = true;
            airControl = true;
            jumpForgCounter = jumpForgiveness;
        }

        if (jumpForgCounter < 0)
            jump = false;
    }

    private void FixedUpdate()
    {
        Vector2 pos = transform.position;
        grounded = Groundcheck();

        movement = motion;

        if (canClimbX || canClimbY)
        {
            if (axis.y != 0)
                climbing = true;
        }

        if (climbing)
        {
            Climbing();
        }
        else if (grounded)
        {
            hurt = false;
            GroundControl();
        }
        else if(airControl)
        {
            AirControl();
        }
        else
        {
            gravityCurrent = gravityNormal;
        }

        //Apply gravity
        movement.y -= gravityCurrent * Time.fixedDeltaTime;

        //Split movement and motion for animator
        motion = movement;
        motion += addForce;
        addForce = Vector2.zero;

        //Overide
        if(overrideMovement != Vector2.zero)
        {
            airControl = false;
            hurt = true;
            motion = overrideMovement;
            overrideMovement = Vector2.zero;
        }

        //Move player/ rigidbody
        rb.MovePosition(pos + (motion * Time.fixedDeltaTime));

        if (teleport)
        {
            rb.isKinematic = true;
            rb.transform.position = (telepotDestination);
            rb.isKinematic = false;
            teleport = false;
        }

        Debug.DrawRay(pos, movement, Color.green);
        Debug.DrawRay(pos, motion, Color.green);
        Debug.DrawRay(pos + Vector2.right, Vector2.down * gravityCurrent * 0.1f, Color.yellow);
    }

    private void Climbing()
    {
        print("Player is climbing");
        int platformMask = LayerMask.NameToLayer("Platform");
        int playerMask = LayerMask.NameToLayer("Player");

        if (!canClimbX && !canClimbY)
        {
            //Not Climbing
            Physics2D.IgnoreLayerCollision(playerMask, platformMask, false);

            gravityCurrent = gravityNormal;
            climbing = false;
            return;
        }

        if(grounded && Mathf.Abs(axis.x) >= Mathf.Abs(axis.y))
        {
            //Not Climbing
            Physics2D.IgnoreLayerCollision(playerMask, platformMask, false);

            gravityCurrent = gravityNormal;
            GroundControl();
            climbing = false;
            return;
        }

        if (jump)
        {
            //Not Climbing
            Physics2D.IgnoreLayerCollision(playerMask, platformMask, false);

            gravityCurrent = gravityNormal;
            GroundControl();
            climbing = false;
            return;
        }

        gravityCurrent = 0;

        //Climbing
        Physics2D.IgnoreLayerCollision(playerMask, platformMask, true);


        if (canClimbX)
            movement.x = axis.x * runSpeed;
        else
        {
            movement.x = Mathf.MoveTowards(0, climbCenter.position.x - rb.position.x, climbCenterSpeed) /Time.fixedDeltaTime;
        }

        if (canClimbY)
            movement.y = axis.y * runSpeed;
        else
            movement.y = Mathf.MoveTowards(0, climbCenter.position.y - rb.position.y, climbCenterSpeed) / Time.fixedDeltaTime;
    }

    private void GroundControl()
    {
        //Movement X
        movement.x = axis.x * runSpeed;

        //Jump
        if (jump)
            movement.y = jumpStrength;

        if (movement.y < 0)
        {
            movement.y = 0f;
            airControl = false;
        }
        jump = false;
    }

    private void AirControl()
    {
        if (Input.GetButton("Jump"))
        {
            if (movement.y > 0)
                gravityCurrent = gravityLow;
            else
                gravityCurrent = gravityNormal;
        }
        else
        {
            gravityCurrent = gravityHi;
        }
    }

    private bool Groundcheck()
    {
        Collider2D[] hit = Physics2D.OverlapCircleAll(gRayCenter.transform.position, gRayRadius, groundLayer);

        if (hit.Length > 0)
        {
            for (int i = 0; i < hit.Length; i++)
            {
                //Only hit if above
                float platformY = hit[i].transform.position.y + hit[i].bounds.extents.y - 0.05f;
                if (platformY < gRayCenter.transform.position.y - gRayInnerRadius && rb.velocity.y <= 0)
                {
                    //If standing on ground
                    gRenderer.material.color = Color.green;
                    if(hit[i].tag == "MovingPlatform")
                    {
                       // addForce += hit[i].GetComponent<MoveingPlatform>().move;
                    }
                    else
                    {
                        transform.parent = null;
                    }

                    //Check if crushed
                    HitHead();
                    return true;
                }
            }
        }
        else
        {
            transform.parent = null;
        }
        gRenderer.material.color = Color.red;
        return false;
    }

    private void HitHead()
    {
        float headLength = transform.GetComponent<Collider2D>().bounds.extents.y;
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.up, headLength, groundLayer);

        Debug.DrawRay(transform.position, Vector3.up * headLength, Color.red);
        if (hit.collider != null)
        {
            Debug.DrawRay(transform.position, Vector3.up * headLength, Color.green);
            print("CRuSh");
        }
    }
}

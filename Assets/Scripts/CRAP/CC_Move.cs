using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//FIX
//Slope slide
//Ladder V
//Door 
//Attack
//Platform
public class CC_Move : MonoBehaviour
{
    [Header("Build")]
    [Tooltip("Sprite thats displayed")]
    public Transform sprite;
    [Tooltip("Start Raycast from this point")]
    public Transform groundRayS;
    [Tooltip("Stop Raycast at this point")]
    public Transform groundRayT;
    private float gRayDist;
    [Tooltip("Mask whats ground")]
    public LayerMask groundMask;
    /*
    public LayerMask platformMask;
    //public LayerMask moveingPlatformsMask;
    [Tooltip("Mask whats player")]
    public LayerMask playerMask;*/



    [Header("Settings")]
    public float runSpeed = 2.0f;
    public float airSpeed = 0.5f;
    public float jumpStrength = 5.0f;
    [Tooltip("Gravity while holding jump and moveing up")]
    public float gravityJump = 0.5f; //Gravity while holding jump and moveing up
    [Tooltip("Gravity while holding jump and moveing down")]
    public float gravityfall = 1f; //Gravity while holding jump and moveing down
    [Tooltip("Gravity while not holding jump")]
    public float gravityNormal = 2.0f; //Gravity while not holding jump
    [Tooltip("Jump forgiveness in seconds")]
    public float jumpForgiveness = 0.25f; //Jump forgiveness
    private float jFTimeOut = 0.25f;

    

    [Header("Climb")]
    public bool canClimb;
    public bool climb;
    public float climbSpeed = 4;
    private float ladderCenter;


    [Header("Other")]
    public Vector2 inAxis;
    private Vector2 movement;
    public Vector2 addForce;
    public Vector2 overideMovement;
    public Vector3 teleport;
    private bool jump;
    private Rigidbody2D rb;
    private Animator anim;

    public bool grounded;


    [Header("FX")]
    public float impForce;

    public bool hit;

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "Ladder")
        {
            print("ladder");
            canClimb = true;
            ladderCenter = collision.transform.position.x;
        }

        if (collision.tag == "MovingPlatform")
        {
            print("VBlfkrogp");
            //MoveingPlatform mPlatform = collision.GetComponentInParent<MoveingPlatform>();
          /*
            if (mPlatform != null)
                teleport = mPlatform.moveDir;
            else
                print("missint");
          */
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.tag == "Ladder")
            canClimb = false;
    }

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = sprite.GetComponent<Animator>();
        Vector2 gDir = groundRayS.position - groundRayT.position;
        gRayDist = gDir.magnitude;
        jFTimeOut = jumpForgiveness;
    }

    // Update is called once per frame
    void Update()
    {
        sprite.position = transform.position;

        //Input actions
        inAxis.x = Input.GetAxis("Horizontal");
        inAxis.y = Input.GetAxis("Vertical");

        if (canClimb && inAxis.y != 0)
        {
            climb = true; //Start climb
        }
        else if(inAxis.y < 0)
        {
            StartCoroutine(DropFromPlatform()); //Drop down
        }


        //Jump input
        if (Input.GetButtonDown("Jump"))
        {
            jump = true;
            jFTimeOut = jumpForgiveness;
        }
        if (jump == true)
        {
            jFTimeOut -= Time.deltaTime;
            if (jFTimeOut <= 0)
                jump = false;
        }


        //Gravity Scale
            if (Input.GetButton("Jump") && !hit)
            {
                if (rb.velocity.y > 0)
                    rb.gravityScale = gravityJump;
                else
                    rb.gravityScale = gravityfall;
            }
            else
            {
                rb.gravityScale = gravityNormal;
            }
    }



    private void FixedUpdate()
    {
        inAxis.x = Input.GetAxis("Horizontal");
        inAxis.y = Input.GetAxis("Vertical");

        grounded = Groundcheck();
        movement = rb.velocity;


        if (climb)
        {
            //Movement while climbing
            Climbing();
        }
        else if (grounded)
        {
            //Movement while grounded
            GroundMovement();
            hit = false;
        }
        else
        {
            //Airial Movement
            AirialMovement();
        }


        movement += addForce;
        addForce = Vector2.zero;

        if (overideMovement != Vector2.zero)
        {
            hit = true;
            movement = overideMovement;
        }
        overideMovement = Vector2.zero;

        if (teleport != Vector3.zero)
        {
            rb.MovePosition(transform.position + teleport * Time.fixedDeltaTime);
            print("teleport");
            teleport = Vector3.zero;
        }

        Animations(movement, grounded);
        rb.velocity = movement;
    }


    private void Climbing()
    {
        int platformMask = LayerMask.NameToLayer("Platform");
        int playerMask = LayerMask.NameToLayer("Player");

        if (canClimb)
        {
            if (Mathf.Abs(inAxis.x) > Mathf.Abs(inAxis.y))
            {
                //Not Climbing
                Physics2D.IgnoreLayerCollision(playerMask, platformMask, false);

                rb.gravityScale = gravityNormal;
                climb = false;
            }
            else
            {
                //is Climbing
                Physics2D.IgnoreLayerCollision(playerMask, platformMask, true);

                rb.gravityScale = 0;
                rb.MovePosition(Vector2.MoveTowards(transform.position, new Vector2(ladderCenter, transform.position.y + inAxis.y), climbSpeed * Time.fixedDeltaTime));
                movement.y = inAxis.y;
            }
        }
        else
        {
            //exit ladder
            movement.y = jumpStrength;
            movement.x = runSpeed * inAxis.x;
            //Not Climbing
            Physics2D.IgnoreLayerCollision(playerMask, platformMask, false);

            rb.gravityScale = gravityNormal;
            climb = false;
        }
    }

    private void GroundMovement()
    {
        movement.x = inAxis.x * runSpeed;

        if (jump)
        {
            movement.y = jumpStrength;
            jump = false;
        }

        //FX
        if (impForce > 120f)
        {
            Camera.main.GetComponent<CamControll>().impForce = 0.5f;
        }
        impForce = 0;
    }

    private void AirialMovement()
    {
        if (movement.x < runSpeed && movement.x > -runSpeed)
            movement.x += inAxis.x * airSpeed;

        impForce = rb.velocity.y * rb.velocity.y;
    }

    private bool Groundcheck()
    {
        Collider2D[] hit = Physics2D.OverlapCircleAll(groundRayS.position, 0.4f, groundMask);

        if (hit.Length > 0)
        {
            for (int i = 0; i < hit.Length; i++)
            {
                float platformY = hit[i].transform.position.y + hit[i].bounds.extents.y - 0.05f;
                if (platformY < groundRayT.position.y && rb.velocity.y <= 0)
                {
                    //If standing on moveing platform
                    return true;
                }
            }
        }
        return false;
    }
   
    private void Animations(Vector2 dir, bool ground)
    {
        if (climb)
        {
            anim.SetBool("Run", false);
            anim.SetBool("Climb", true);
            anim.SetBool("Jump", false);
            anim.SetBool("Fall", false);
            anim.SetBool("Idle", false);

            anim.speed = Mathf.Abs(dir.y);
            return;
        }
        else if (ground)
        {
            if (dir.x < 0)
            {
                sprite.GetComponent<SpriteRenderer>().flipX = true;
                anim.SetBool("Run", true);
                anim.SetBool("Climb", false);
                anim.SetBool("Jump", false);
                anim.SetBool("Fall", false);
                anim.SetBool("Idle", false);

                anim.speed = 1;//dir.x * 0.25f;
                return;
            }
            else if (dir.x > 0)
            {
                sprite.GetComponent<SpriteRenderer>().flipX = false;
                anim.SetBool("Run", true);
                anim.SetBool("Climb", false);
                anim.SetBool("Jump", false);
                anim.SetBool("Fall", false);
                anim.SetBool("Idle", false);

                anim.speed = 1;// dir.x * 0.25f;
                return;
            }
            else
            {
                anim.SetBool("Run", false);
                anim.SetBool("Climb", false);
                anim.SetBool("Jump", false);
                anim.SetBool("Fall", false);
                anim.SetBool("Idle", true);

                anim.speed = 1;
                return;

            }
        }
        else
        {
            if (dir.y > 0)
            {
                anim.SetBool("Run", false);
                anim.SetBool("Climb", false);
                anim.SetBool("Jump", true);
                anim.SetBool("Fall", false);
                anim.SetBool("Idle", false);

                anim.speed = 1;
                return;
            }
            else
            {
                anim.SetBool("Run", false);
                anim.SetBool("Climb", false);
                anim.SetBool("Jump", false);
                anim.SetBool("Fall", true);
                anim.SetBool("Idle", false);

                anim.speed = 1;
                return;
            }
        }
    }

    IEnumerator DropFromPlatform()
    {
        int platformMask = LayerMask.NameToLayer("Platform");
        int playerMask = LayerMask.NameToLayer("Player");
        Physics2D.IgnoreLayerCollision(playerMask, platformMask, true);
        yield return new WaitForSeconds(0.5f);
        Physics2D.IgnoreLayerCollision(playerMask, platformMask, false);
        print("Drop");
    }
}
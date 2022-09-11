using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character_Animator : MonoBehaviour
{
    //[Header("Build, Sets automatic if child " + "\n" +" object to script transform")]
    [Header ("is child to script transform")]
    [Header("Build, Sets automatic if sprite ")]
    public Character_Move cMove;
    public Transform sprite;
    public Animator anim;

    private bool run;
    private bool climb;
    private bool jump;
    private bool fall;
    private bool idle;
    private bool hurt;

    private float speed;

    private bool flip;

        Rigidbody treD;
        Rigidbody2D tvaD;
    private void Start()
    {
        if (cMove == null)
            cMove = GetComponent<Character_Move>();

        if(sprite == null)
        {
            for(int i = 0; i < transform.childCount; i++)
            {
                anim = transform.GetChild(i).GetComponent<Animator>();
                if (anim != null)
                {
                    sprite = transform.GetChild(i);
                }
            }
        }
    }

    private void LateUpdate()
    {
        GetAnimationState(cMove.movement, cMove.grounded, cMove.climbing, cMove.hurt);
        SetAnimationState();
    }

    private void SetAnimationState()
    {
        sprite.GetComponent<SpriteRenderer>().flipX = flip;

        anim.speed = speed;
        anim.SetBool("Run", run);
        anim.SetBool("Climb", climb);
        anim.SetBool("Jump", jump);
        anim.SetBool("Fall", fall);
        anim.SetBool("Idle", idle);
        anim.SetBool("Hurt", hurt);
    }

    private void GetAnimationState(Vector2 dir, bool ground, bool climbing, bool hurting)
    {
        climb = false;
        run = false;
        jump = false;
        fall = false;
        idle = false;
        hurt = false;

        speed = 1;

        if (dir.x < 0)
        {
            flip = true;
        }
        else if (dir.x > 0)
        {
            flip = false;
        }

        if (climbing)
        {
            climb = true;
            speed = Mathf.Abs(dir.y);
            return;
        }
        else if (ground)
        {
            if (dir.x < 0 || dir.x > 0)
            {
                run = true;
                speed = Mathf.Abs(dir.x) * 0.25f;
                return;
            }
            else
            {
                idle = true;
                return;
            }
        }
        else if(!hurting)
        {
            if (dir.y > 0)
            {
                jump = true;
                return;
            }
            else
            {
                fall = true;
                return;
            }
        }
        else
        {
            hurt = true;
        }
    }
}

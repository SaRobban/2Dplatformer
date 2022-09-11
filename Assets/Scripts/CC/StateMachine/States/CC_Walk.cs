using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CC_Walk : ICharacterState
{
    MainCharacter owner;
    private bool hasBraked;
    //Constructor
    public CC_Walk(MainCharacter owner)
    {
        this.owner = owner;
    }

    public void OnEnter()
    {
        owner.ResetDubbleJumps();
        owner.input.ConsumeJumpForgiveness();

        if (owner.input.JumpWithForgiveness)
        {
            Debug.Log("JumpedWithForgivness");
            Vector2 velocity = owner.GetVelocity();

            if (velocity.x > 0)
                owner.sprite.flipX = false;
            else
                owner.sprite.flipX = true;

            owner.ChangeStateTo<CC_Jump>();
            return;
        }
        owner.SetAnimationTo("Run");
    }

    float dubbleClickTimeOut = 0.2f;
    float dubbleClickTime = 0.0f;
    int lastAxis = 0;
    bool wasZero = true;
    private bool DubbleKlick()
    {
        dubbleClickTime -= Time.deltaTime;

        if ((int)Input.GetAxisRaw("Horizontal") == lastAxis && wasZero && dubbleClickTime > 0)
        {
            dubbleClickTime = 0;
            return true;
        }
        else if (Input.GetAxisRaw("Horizontal") != 0)
        {
            lastAxis = (int)Input.GetAxisRaw("Horizontal");
            dubbleClickTime = dubbleClickTimeOut;
            wasZero = false;
        }
        else
        {
            wasZero = true;
        }

        return false;
    }
    public void Execute(float deltaT)
    {
        if (ShouldExitState())
            return;

        Vector2 velocity = WalkFunction(deltaT);

        Animations(velocity);
        FootStepsSounds(velocity);
    }

    bool ShouldExitState()
    {
        if(owner.input.FixedLightnignDash)
        {
            owner.ChangeStateTo<CC_LightningDash>();
            return true;
        }

        //Exit State?
        if (owner.input.FixedJump == true)
        {
            owner.ChangeStateTo<CC_Jump>();
            return true;
        }
        if (!owner.flags.Grounded)
        {
            owner.ChangeStateTo<CC_Fall>();
            return true;
        }
        return false;
    }

    Vector2 WalkFunction(float deltaT)
    {

        Vector2 velocity = owner.GetVelocity();
        float inputVelocity = owner.input.Axis.x * owner.stats.WalkSpeed;

        //Momentum
        velocity.x = Mathf.Lerp(velocity.x, inputVelocity, owner.stats.KillMomentum * deltaT);

        if (owner.flags.Grounded)
        {
            velocity.y = 0;
        }
        velocity.y -= owner.stats.NormalGravity * deltaT;

        owner.SetVelocityTo(velocity);
        return velocity;
        /*
        if (owner.input.Dash)
        {
            velocity.x = owner.input.Axis.x * owner.stats.RunSpeed;
        }
        */

        /*
        if (DubbleKlick())
        {
            velocity.x = Input.GetAxisRaw("Horizontal") * 10;
            Debug.Log("dodge");
        }
        */
    }

    void Animations(Vector2 velocity)
    {
        //Animation
        if (velocity.x > owner.stats.WalkDeadZone)
        {
            owner.anim.speed = 1 / owner.stats.WalkSpeed * velocity.x;
            owner.sprite.flipX = false;
            hasBraked = false;
            owner.SetAnimationTo("Run");
        }
        else if (velocity.x < -owner.stats.WalkDeadZone)
        {
            owner.anim.speed = 1 / owner.stats.WalkSpeed * -velocity.x;
            owner.sprite.flipX = true;
            hasBraked = false;
            owner.SetAnimationTo("Run");
        }
        else
        {
            if (!hasBraked)
            {
                if (owner.sprite.flipX)
                    SPECIALFX.Command.FireFxUmphLeft(owner.transform.position - Vector3.right * owner.stats.HalfWidth, Vector2.up);
                else
                    SPECIALFX.Command.FireFxUmphRight(owner.transform.position + Vector3.right * owner.stats.HalfWidth, Vector2.up);
                hasBraked = true;
            }

            owner.anim.speed = 1;
            owner.SetAnimationTo("Idle");
        }
    }

    float stepTime = 1;
    float time = 1;
    void FootStepsSounds(Vector2 velocity)
    {
        if (velocity.x == 0)
           time = stepTime;

        time -= owner.anim.speed * Time.deltaTime;
        if (time > 0)
            return;

        owner.sounds.PlaySteps();
        time = stepTime;
    }
    public void OnExit()
    {
        owner.anim.speed = 1;
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CC_Walk : ICharacterState
{
    MainCharacter owner;
    private bool hasBraked;
    float stepTime = 1f;
    float time = 1;


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

    public void Execute(float deltaT)
    {
        if (ShouldExitState())
            return;

        Vector2 velocity = WalkFunction(deltaT);

        if (owner.input.Interact)
        {
            owner.InvokeInteract();
        }

        if(owner.input.Axis.y > 0)
        {
            owner.ChangeStateTo<CC_Howl>();
            return;
        }

        Animations(velocity);
        FootStepsSounds(velocity);

        
    }

    bool ShouldExitState()
    {
        //Exit State?
        if(owner.input.Axis.y < 0)
        {
            owner.ChangeStateTo<CC_Duck>();
        }

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
                    SPECIALFX.Command.Fire("FX_UmphLeft", owner.transform.position - Vector3.right * owner.stats.HalfWidth, Vector2.up);
                else
                    SPECIALFX.Command.Fire("FX_UmphRight", owner.transform.position + Vector3.right * owner.stats.HalfWidth, Vector2.up);
                hasBraked = true;
            }

           

            owner.anim.speed = 1;
            owner.SetAnimationTo("Idle");
        }
    }

   
    void FootStepsSounds(Vector2 velocity)
    {
        if (velocity.x == 0)
           time = stepTime;

        time -= Mathf.Abs(velocity.x) * Time.deltaTime;
        if (time > 0)
            return;

        owner.sounds.PlaySteps();
        //owner.sounds.PlayHowl();
        time = stepTime;
    }
    public void OnExit()
    {
        owner.anim.speed = 1;
    }
}
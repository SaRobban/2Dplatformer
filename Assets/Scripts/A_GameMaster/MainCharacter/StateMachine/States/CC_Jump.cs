using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CC_Jump : ICharacterState
{
    MainCharacter owner;
    //Constructor
    public CC_Jump(MainCharacter owner)
    {
        this.owner = owner;
    }

    public void OnEnter()
    {
        owner.input.ConsumeJumpForgiveness();//Not to dubble jump imedetly
        //owner.useJumpFrogiveness = false;
        owner.input.ConsumeJumpForgiveness();
        //Jump
        owner.SetVelocityTo(new Vector2(owner.GetVelocity().x, owner.stats.JumpStr));
        owner.SetAnimationTo("Jump");

        CommonStateFunctions.SmokeFx(owner, true);
    }

   

    public void Execute(float deltaT)
    {
        Vector2 velocity = owner.GetVelocity();
        if (owner.input.HoldJump)
        {
            velocity = CommonStateFunctions.ApplyJumpHoldGravity(owner,velocity, deltaT);
        }
        else
        {
            velocity = CommonStateFunctions.ApplyFallGravity(owner,velocity, deltaT);
        }
        velocity = CommonStateFunctions.AirControll(owner,velocity, deltaT);
        owner.SetVelocityTo(velocity);

        CheckExitConditions();
    }

    public void OnExit()
    {

    }

    void CheckExitConditions()
    {
        //Exit State?
        CommonStateFunctions.ExitOnDubbleJump(owner);
        /*
        if (owner.input.FixedJump && owner.dubbleJumpAvailable > 0)
        {
            Debug.Log("DubbleJumped");
            owner.dubbleJumpAvailable--;
            owner.ChangeStateTo<CC_Jump>();
            
            return;
        }
        */
        if (owner.GetVelocity().y < 0)
        {
            if (owner.flags.Grounded)
                owner.ChangeStateTo<CC_Walk>();
            else
                owner.ChangeStateTo<CC_Fall>();
        }
        if (owner.input.FixedLightnignDash)
            owner.ChangeStateTo<CC_LightningDash>();
    }
}
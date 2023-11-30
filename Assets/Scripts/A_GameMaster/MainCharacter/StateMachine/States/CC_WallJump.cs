using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CC_WallJump : ICharacterState
{
    MainCharacter owner;
    //Constructor
    public CC_WallJump(MainCharacter owner)
    {
        this.owner = owner;
    }

    public void OnEnter()
    {
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
            velocity = CommonStateFunctions.ApplyJumpHoldGravity(owner, velocity, deltaT);
        }
        else
        {
            velocity = CommonStateFunctions.ApplyFallGravity(owner,velocity, deltaT);
        }
        owner.SetVelocityTo(velocity);

        CheckExitConditions();
    }

    public void OnExit()
    {
        owner.input.ConsumeJumpForgiveness();
    }

    void CheckExitConditions()
    {
        //Exit State?
      
        if (owner.GetVelocity().y < 0)
        {
            if (owner.flags.Grounded)
                owner.ChangeStateTo<CC_Walk>();
            else
                owner.ChangeStateTo<CC_Fall>();
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CC_Fall : ICharacterState
{
    MainCharacter owner;

    //Constructor
    public CC_Fall(MainCharacter owner)
    {
        this.owner = owner;
    }

    public void OnEnter()
    {
        //Debug.Log("Falling");
        owner.SetAnimationTo("Fall");
    }

    public void Execute(float deltaT)
    {
        //WallJump
        Vector2 velocity = owner.GetVelocity();
        if (owner.input.HoldJump)
        {
            velocity = CommonStateFunctions.ApplyNormalGravity(owner,velocity, deltaT);
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
        CommonStateFunctions.SmokeFx(owner, false);
        owner.sounds.PlayThud();
    }

    void CheckExitConditions()
    {
        CommonStateFunctions.ExitOnDubbleJump(owner);

        if (owner.input.JumpWithForgiveness && owner.UseDubbleJump())//Only if last was grounded
        {
            owner.ChangeStateTo<CC_Jump>();
        }

        //Exit State?
        if (owner.GetVelocity().y > 0)
            owner.ChangeStateTo<CC_Jump>();

        if (owner.flags.Grounded )
            owner.ChangeStateTo<CC_Walk>();

        
        if (owner.flags.HitLeftWallHi)
            owner.ChangeStateTo<CC_WallSlide>();
        
        if (owner.flags.HitRightWallHi)
            owner.ChangeStateTo<CC_WallSlide>();

        if (owner.input.FixedLightnignDash)
            owner.ChangeStateTo<CC_LightningDash>();

    }
}

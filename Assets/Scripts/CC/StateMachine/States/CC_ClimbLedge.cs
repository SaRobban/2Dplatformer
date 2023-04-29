using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CC_ClimbLedge : ICharacterState
{
    MainCharacter owner;
    float clipLength;

    float moveTimeDelta;
    Vector2 tPos;
    Vector3 spriteLocalPos;

    public CC_ClimbLedge(MainCharacter owner)
    {
        this.owner = owner;
    }

    public void OnEnter()
    {
        owner.enabledPixelPerfect = false;

        owner.SetVelocityTo(Vector2.zero);
        owner.SetAnimationTo("ClimbLedge");

        owner.rb.isKinematic = true;
        if (owner.sprite.flipX)
            tPos = owner.transform.position + Vector3.up * owner.stats.HangHeight  - Vector3.right * owner.stats.HalfWidth * 2;
        else
            tPos = owner.transform.position + Vector3.up * owner.stats.HangHeight  + Vector3.right * owner.stats.HalfWidth * 2;

        //Unlink Sprite//
      //  spriteLocalPos = owner.sprite.transform.position - owner.transform.position;
        owner.sprite.transform.parent = null;

        foreach (AnimationClip clip in owner.anim.runtimeAnimatorController.animationClips)
        {
            if (clip.name == "ClimbLedge")
            {
                clipLength = clip.length;
                moveTimeDelta = 1 / clipLength;
                return;
            }
        }
    }

    public void Execute(float deltaT)
    {
        clipLength -= deltaT;
        owner.transform.position = Vector2.MoveTowards(owner.transform.position, tPos, moveTimeDelta * deltaT);
        if (clipLength <= 0)
            ExitClimb();
        //        CheckExitConditions();
    }

    public void OnExit()
    {
        owner.enabledPixelPerfect = true;
        return;
    }

    void ExitClimb()
    {
        owner.sprite.transform.position = owner.transform.position + spriteLocalPos;
        owner.sprite.transform.parent = owner.transform;
        owner.rb.isKinematic = false;
        //   owner.transform.position += Vector3.up * 1f + Vector3.right * 0.40f;
        owner.ChangeStateTo<CC_Walk>();
    }

    void CheckExitConditions()
    {
        ExitGrounded();
        ExitAxis();
        ExitWallJump();
        ExitNoWall();
    }

    void ExitWallJump()
    {
        //WallJump
        if (owner.input.FixedJump)
        {
            WallJump();
        }
    }

    void WallJump()
    {
        Vector2 jumpForce = (Vector2.up * owner.stats.WallJumpYStr + Vector2.right * owner.stats.WallJumpXStr);
        if (owner.flags.HittingWallRight)
        {
            jumpForce.x *= -1;
        }
        owner.SetVelocityTo(jumpForce);
        owner.sprite.flipX = !owner.sprite.flipX;
        owner.ChangeStateTo<CC_WallJump>();
    }

    void ExitAxis()
    {
        //Drop by input
        if (owner.flags.HittingWallRight && owner.input.Axis.x < -0.5f)
        {
            owner.TeleportTo((Vector2)owner.transform.position - Vector2.right * owner.flags.GetSkinWitdh() * 2);
            owner.ChangeStateTo<CC_Fall>();
        }
        else if (owner.flags.HittingWallLeft && owner.input.Axis.x > 0.5f)
        {
            owner.TeleportTo((Vector2)owner.transform.position + Vector2.right * owner.flags.GetSkinWitdh() * 2);
            owner.ChangeStateTo<CC_Fall>();
        }
    }

    void ExitNoWall()
    {
        //Drop if no wall
        if (!owner.flags.HittingWall)
        {
            owner.ChangeStateTo<CC_Fall>();
        }
    }

    void ExitGrounded()
    {
        //If hitting ground
        if (owner.flags.Grounded)
            owner.ChangeStateTo<CC_Walk>();
    }
}
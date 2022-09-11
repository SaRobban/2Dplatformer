using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CC_WallSlide : ICharacterState
{
    MainCharacter owner;

    private bool ownerHasBeenAbove;
    public CC_WallSlide(MainCharacter owner)
    {
        this.owner = owner;
        ownerHasBeenAbove = false;
    }

    public void OnEnter()
    {
        ownerHasBeenAbove = false;
        if (owner.input.JumpWithForgiveness)
        {
            Debug.Log("Enterd while Jump");
            WallJump();
            return;
        }

        owner.SetVelocityTo(Vector2.zero);

        if (owner.flags.HittingWallRight)
            owner.sprite.flipX = false;
        else
            owner.sprite.flipX = true;

        owner.SetAnimationTo("Slide");
    }

    public void Execute(float deltaT)
    {
        Vector2 velocity = owner.GetVelocity();
        velocity = Acc(velocity, deltaT);

        owner.SetVelocityTo(velocity);

        CheckExitConditions();
    }

    Vector2 Acc(Vector2 velocity, float deltaT)
    {
        velocity -= Vector2.up * owner.stats.WallSlideAcc * deltaT;

        //Gravity
        if (velocity.y < -owner.stats.MaxWallSlideSpeed)
        {
            velocity = -Vector2.up * owner.stats.MaxWallSlideSpeed;
        }
        return velocity;
    }
    public void OnExit()
    {
        ownerHasBeenAbove = false;
        return;
    }

    void CheckExitConditions()
    {
        ExitGrounded();
        ExitAxis();
        ExitWallJump();
        ExitNoWall();
        ExitToHanging();
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

    void ExitToHanging()
    {
        float segment = 0.25f;
        Vector2 checkPoint = new Vector2(
            owner.stats.HalfWidth + owner.stats.SkinWidth,
            owner.stats.HangHight + segment
            );

        if (owner.flags.HitLeftWallHi)
        {
            checkPoint.x *= -1;
        }

        //TODO : NON alockate?
        RaycastHit2D hit = Physics2D.Raycast(owner.GetPosition() + checkPoint, Vector2.down, segment);

        if (hit.collider != null)
        {
            //If hit point is not insidesomething and top is flat
            if (hit.point != owner.GetPosition() + checkPoint && hit.normal == Vector2.up)
            {
                owner.TeleportTo(hit.point - checkPoint + Vector2.up * segment);
                owner.ChangeStateTo<CC_LedgeHang>();
            }
        }
    }
}



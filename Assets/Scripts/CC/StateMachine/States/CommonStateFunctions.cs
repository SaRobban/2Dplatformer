using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct CommonStateFunctions
{
    //Common Exit conditions
    public static void ExitOnDubbleJump(MainCharacter owner)
    {
        if (owner.input.FixedJump && owner.UseDubbleJump())
        {
            Debug.Log("DubbleJumped");
            owner.ChangeStateTo<CC_Jump>();
            return;
        }
    }
    

    //Common Forces
    public static Vector2 AddGravityByInput(MainCharacter owner, Vector2 velocity, float deltaT)
    {
        //Gravity
        if (owner.input.HoldJump)
        {
            if (velocity.y > 0)
                return velocity - Vector2.up * owner.stats.JumpHoldGravity * deltaT;
            else
                return velocity - Vector2.up * owner.stats.NormalGravity * deltaT;
        }
        else
        {
            return velocity - Vector2.up * owner.stats.JumpFallGravity * deltaT;
        }
    }

    public static Vector2 ApplyJumpHoldGravity(MainCharacter owner, Vector2 velocity, float deltaT)
    {
        return velocity - Vector2.up * owner.stats.JumpHoldGravity * deltaT;
    }

    public static Vector2 ApplyFallGravity(MainCharacter owner, Vector2 velocity, float deltaT)
    {
        return velocity - Vector2.up * owner.stats.JumpFallGravity * deltaT;
    }

    public static Vector2 ApplyNormalGravity(MainCharacter owner, Vector2 velocity, float deltaT)
    {
        return velocity - Vector2.up * owner.stats.NormalGravity * deltaT;
    }

    public static Vector2 AirControll(MainCharacter owner, Vector2 velocity, float deltaT)
    {
        if (velocity.x >= owner.stats.WalkSpeed && owner.input.Axis.x >= 0)
            return velocity;

        if (velocity.x <= -owner.stats.WalkSpeed && owner.input.Axis.x <= 0)
            return velocity;


            return velocity + Vector2.right * owner.input.Axis.x * owner.stats.AirControl * deltaT;
    }

    public static void SmokeFx(MainCharacter owner, bool inclWalls)
    {
        //FX
        Vector2 up = Vector2.up;
        Vector2 pos = (Vector2)owner.transform.position;

        if (!owner.flags.Grounded)
        {
            if (inclWalls)
            {
                if (owner.flags.HittingWallLeft)
                {
                    up = Vector2.right;
                    SPECIALFX.Command.FireFxUmphRight(pos, up);
                    return;
                }

                if (owner.flags.HittingWallRight)
                {
                    up = Vector2.left;
                    SPECIALFX.Command.FireFxUmphLeft(pos, up);
                    return;
                }
            }
            return;
        }

        if (owner.GetVelocity().x > 0.25f)
            SPECIALFX.Command.FireFxUmphLeft(pos, up);
        else if (owner.GetVelocity().x < -0.25f)
            SPECIALFX.Command.FireFxUmphRight(pos, up);
        else
        {
            SPECIALFX.Command.FireFxUmphLeft(pos, up);
            SPECIALFX.Command.FireFxUmphRight(pos, up);
        }
    }
}

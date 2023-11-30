using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CC_LightningDash : ICharacterState
{
    MainCharacter owner;
    public CC_LightningDash(MainCharacter owner)
    {
        this.owner = owner;
    }

    bool fistFrame = true;
    Vector2 dir;
    public void OnEnter()
    {
        Debug.Log("<color:red>FlashMove</color>");
        owner.input.ConsumeJumpForgiveness();
        owner.sprite.gameObject.SetActive(false);
        //owner.lightningFX.SetActive(true);
        owner.TeleportTo(owner.GetPosition() + Vector2.up * owner.flags.GetSkinWitdh());
        fistFrame = true;
        dir = LockDir();
    }
    Vector2 LockDir()
    {
        Vector2 axis = owner.input.Axis;
        if(axis.sqrMagnitude < 0.25f)
        {
            if (owner.sprite.flipX)
                return Vector2.left;
            else
                return Vector2.right;
        }

        if(Mathf.Abs(axis.x)> Mathf.Abs(axis.y))
        {
            if (axis.x > 0)
                return Vector2.right;
            else
                return Vector2.left;
        }
        else
        {
            if (axis.y > 0)
                return Vector2.up;
            else
                return Vector2.down;
        }
    }

    public void Execute(float deltaT)
    {
        if (ShouldExitState())
            return;

        owner.SetVelocityTo(dir * 30);
    }

    bool ShouldExitState()
    {
        if (owner.input.FixedJump)
        {
            owner.ChangeStateTo<CC_Fall>();
            return true;
        }

        if (fistFrame)
        {
            fistFrame = false;
            return false;
        }
        if (owner.flags.Hit)
        {
            owner.ChangeStateTo<CC_Fall>();
            return true;
        }
        return false;
    }

    public void OnExit()
    {
        owner.input.ConsumeJumpForgiveness();
        owner.sprite.gameObject.SetActive(true);
        //owner.lightningFX.SetActive(false);
        owner.anim.speed = 1;
    }
}

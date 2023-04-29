using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CC_FreezePlayer : ICharacterState
{
    MainCharacter owner;

    Vector2 inVelociity;
    public CC_FreezePlayer(MainCharacter owner)
    {
        this.owner = owner;
    }
    public void Execute(float deltaT)
    {
        owner.SetVelocityTo(Vector2.zero);
        owner.SetAnimationTo("Idle");
    }

    public void OnEnter()
    {
        inVelociity = owner.rb.velocity;
        owner.SetVelocityTo(Vector2.zero);
    }

    public void OnExit()
    {
        owner.SetVelocityTo(inVelociity);
    }
}

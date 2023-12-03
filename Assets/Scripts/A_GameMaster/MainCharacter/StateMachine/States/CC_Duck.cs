using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class CC_Duck : ICharacterState
{
    MainCharacter owner;

    Vector2 inVelociity;
    public CC_Duck(MainCharacter owner)
    {
        this.owner = owner;
    }
    public void Execute(float deltaT)
    {
        if (owner.input.Axis.y >= 0)
        {
            owner.ChangeStateTo<CC_Walk>();
        }
        owner.SetVelocityTo(Vector2.zero);
        owner.SetAnimationTo("Duck");
    }

    public void OnEnter()
    {
        inVelociity = owner.rb.velocity;
        owner.SetVelocityTo(Vector2.zero);
    }

    public void OnExit()
    {
    }
}

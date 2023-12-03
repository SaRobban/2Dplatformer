using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CC_DialogEyesClosed : ICharacterState
{
    MainCharacter owner;
    //Constructor
    public CC_DialogEyesClosed(MainCharacter owner)
    {
        this.owner = owner;
    }

    public void Execute(float deltaT)
    {
    }

    public void OnEnter()
    {
        owner.SetVelocityTo(Vector2.zero);
        owner.SetAnimationTo("DialogEyesClosed");
    }

    public void OnExit()
    {
    }
}

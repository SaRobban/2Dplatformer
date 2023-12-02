using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CC_Howl : ICharacterState
{
    MainCharacter owner;
    float audioLength;
    //Constructor
    public CC_Howl(MainCharacter owner)
    {
        this.owner = owner;
    }

    public void Execute(float deltaT)
    {
        audioLength -= deltaT;

        if (audioLength < 0)
            owner.ChangeStateTo<CC_Walk>();
    }

    public void OnEnter()
    {
        owner.SetVelocityTo(Vector2.zero);
        owner.SetAnimationTo("Cat_Howl");
        audioLength = owner.sounds.PlayHowl();
    }

    public void OnExit()
    {
       
    }
}

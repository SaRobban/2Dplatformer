using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CC_Dialog : ICharacterState
{
    MainCharacter owner;

    //Constructor
    public CC_Dialog(MainCharacter owner)
    {
        this.owner = owner;
    }
    public void Execute(float deltaT)
    {
       
    }

    public void OnEnter()
    {
        owner.SetVelocityTo(Vector2.zero);
        owner.SetAnimationTo("Dialog");
        owner.sounds.PlayDialog();
    }

    public void OnExit()
    {
        
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CC_Respawn : ICharacterState
{
    MainCharacter owner;
    public CC_Respawn(MainCharacter owner)
    {
        this.owner = owner;
    }

    public void Execute(float deltaT)
    {
        Debug.Log("State : CC_Respawn");
    }

    public void OnEnter()
    {
        owner.EnableCollision();
        owner.SetVelocityTo(Vector2.zero);
        CameraFunction.instance.follow = true;
        owner.RevivePlayerAtLastCheckPoint();
        owner.ChangeStateTo<CC_Walk>();
    }

    public void OnExit()
    {
       // throw new System.NotImplementedException();
    }

}

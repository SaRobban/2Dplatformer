using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CC_PortalMagic : ICharacterState
{
    MainCharacter owner;
    public CC_PortalMagic(MainCharacter owner)
    {
        this.owner = owner;
    }
    public void Execute(float deltaT)
    {
        throw new System.NotImplementedException();
    }

    public void OnEnter()
    {
        throw new System.NotImplementedException();
    }

    public void OnExit()
    {
        throw new System.NotImplementedException();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CC_Death : ICharacterState
{
    private float hurtTime = 2;
    private MainCharacter owner;

    private Vector2 move;
    private Vector3 orgPos;

    public CC_Death(MainCharacter owner)
    {
        this.owner = owner;
    }
    public void Execute(float deltaT)
    {
        hurtTime -= deltaT;
        move = CommonStateFunctions.ApplyNormalGravity(owner, move, deltaT);
        owner.sprite.transform.position += (Vector3)(move * deltaT);
        if (hurtTime < 0)
        {
            owner.ChangeStateTo<CC_Respawn>();
        }
    }

    public void OnEnter()
    {
        CanvasManager.deathOverlay.FadeIn();
        CameraManager.controll.SetStatic();

        hurtTime = 4;
        owner.DisableCollision();
        owner.SetVelocityTo(Vector2.zero);
        owner.SetAnimationTo("Death");
        move = Vector2.up * 2;
        orgPos = owner.transform.InverseTransformPoint(owner.sprite.transform.position);
    }

    public void OnExit()
    {
        CanvasManager.deathOverlay.Close();
        owner.sprite.transform.position = owner.transform.position + orgPos;
        owner.EnableCollision();
    }

}

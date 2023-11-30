using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CC_Aim
{
    [SerializeField] private MainCharacter owner;
    [SerializeField] private Vector2 aim;
    [SerializeField] private Rigidbody2D rb;
    public CC_Aim(Rigidbody2D rb, MainCharacter owner)
    {
        this.rb = rb;
        this.owner = owner;
    }

    public void UpdateAim()
    {
        Debug.DrawRay(owner.transform.position, aim, Color.cyan);
       // aim = owner.input.Axis;
        //return;
        if (owner.GetAnimationState() == "Idle")
            return;

        if (owner.GetAnimationState() == "Run")
        {
            if (owner.input.Axis != Vector2.zero)
                aim = owner.input.Axis;
            return;
        }


        if (rb.velocity == Vector2.zero)
            return;

        aim = rb.velocity.normalized;
    }

    public Vector2 GetAim()
    {
        return aim;
    }
}

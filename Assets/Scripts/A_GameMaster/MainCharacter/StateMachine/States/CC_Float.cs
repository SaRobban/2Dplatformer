using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CC_Float : ICharacterState
{
    MainCharacter owner;

    public float speed = 0.5f;
    public float damping = 0.2f;
    private int hits;
    private float minDistToG = 2;
    private RaycastHit2D[] hitData;

    public CC_Float(MainCharacter owner)
    {
        this.owner = owner;
        this.speed = 1f;
        this.damping = 2;
        this.hits = 0;
        this.minDistToG = 2;
        this.hitData = new RaycastHit2D[8];
    }

    public void OnEnter()
    {
        owner.SetAnimationTo("Slide");
    }

    public void Execute(float deltaT)
    {
        Vector2 velocity = owner.GetVelocity();
        velocity = FloatG(velocity, deltaT);

        owner.SetVelocityTo(velocity);

        CheckExitConditions();
    }


    public void OnExit()
    {
        return;
    }

    void CheckExitConditions()
    {
        
        if (!owner.input.HoldJump && !owner.input.FixedJump)
            owner.ChangeStateTo<CC_Fall>();
        
        if (owner.flags.Grounded)
            owner.ChangeStateTo<CC_Walk>();
    }

    Vector2 FloatG(Vector2 velocity, float deltaT)
    {
        float distToG = 0;
        Debug.DrawRay(owner.transform.position, Vector2.down * minDistToG);

        hits = Physics2D.CircleCastNonAlloc((Vector2)owner.GetPosition(), 0.25f, Vector2.down, hitData, minDistToG);
        if (hits > 0)
        {
            float closest = Mathf.Infinity;
            for (int i = 0; i < hits; i++)
            {
                if (hitData[i].collider.transform != owner.transform)
                {
                    if (hitData[i].distance < closest)
                        closest = hitData[i].distance;
                }
            }
            distToG = closest - minDistToG;
        }
        if (distToG != Mathf.Infinity)
            velocity.y -= (minDistToG - distToG * distToG * 4) * deltaT * speed;
        else
            velocity.y -= minDistToG * deltaT;

            velocity.y *= 1 - damping * deltaT;

        return velocity;
    }

}

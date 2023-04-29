using System.Collections;
using UnityEngine;
public class CC_Hurt : ICharacterState
{
    public float resetHurtTime = 0.5f;
    private float hurtTime = 0.5f;
    private MainCharacter owner;

    public CC_Hurt(MainCharacter owner)
    {
        this.owner = owner;
    }

    public void OnEnter()
    {
        hurtTime = resetHurtTime;

        ApplyBackLach();

        owner.SetAnimationTo("Hurt");

        //WARNING : 
        CommonStateFunctions.OnTakeDamage(owner, 1);
        if (owner.healthSystem.IsDead())
        {
            owner.ChangeStateTo<CC_Death>();
        }
    }
    public void Execute(float deltaT)
    {
        hurtTime -= deltaT;
        owner.SetVelocityTo(CommonStateFunctions.ApplyNormalGravity(owner, owner.GetVelocity(), deltaT));

        if (owner.flags.Grounded && hurtTime <= 0)
        {
            owner.ChangeStateTo<CC_Walk>();
        }
    }
    public void OnExit()
    {
        CommonStateFunctions.SmokeFx(owner, true);
    }

    private void ApplyBackLach()
    {
        Vector2 velocity = owner.GetVelocity();
        float x = owner.stats.WalkSpeed;
        if (velocity.x > 0)
        {
            x = -x;
        }
        velocity.x = -owner.input.Axis.x * owner.stats.WalkSpeed;
        velocity.y = owner.stats.JumpStr;
        owner.SetVelocityTo(velocity);
    }
}

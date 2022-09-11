using System.Collections;
using UnityEngine;
public class CC_Hurt : ICharacterState
{
//TODO : Clean up
    private int hurtTime = 2;
    private MainCharacter owner;

    public CC_Hurt(MainCharacter owner)
    {
        this.owner = owner;
    }

    public void OnEnter()
    {
        hurtTime = 4;

        Vector2 velocity = owner.GetVelocity();
        float x = owner.stats.WalkSpeed;
        if (velocity.x > 0)
        {
            x = -x;
        }
        velocity.x = -owner.input.Axis.x * owner.stats.WalkSpeed;
        velocity.y = owner.stats.JumpStr;
        owner.SetVelocityTo(velocity);

        owner.SetAnimationTo("Hurt");
    }
    public void Execute(float deltaT)
    {
        hurtTime--;
        owner.SetVelocityTo(CommonStateFunctions.ApplyNormalGravity(owner, owner.GetVelocity(), deltaT));

        if (owner.flags.Grounded && hurtTime <= 0 )
        {
            owner.ChangeStateTo<CC_Walk>();
        }
    }
    public void OnExit()
    {
        CommonStateFunctions.SmokeFx(owner, true);
    }

    IEnumerator Hurt()
    {
        yield return new WaitForSeconds(hurtTime);
        owner.ChangeStateTo<CC_Fall>();
    }
}

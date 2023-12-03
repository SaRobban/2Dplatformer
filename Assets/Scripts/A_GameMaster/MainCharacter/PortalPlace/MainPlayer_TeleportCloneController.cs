using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainPlayer_TeleportCloneController : TeleportCloneController
{
    [SerializeField] MainCharacter mainCharacter => PlayerManager.mainCharacter;
    [SerializeField] Animator animator;
    [SerializeField] SpriteRenderer sprite;
    public override void Init()
    {
        base.Init();

        if (animator == null)
            animator = cloneInstance.GetComponentInChildren<Animator>();

        if (sprite == null)
            sprite = cloneInstance.GetComponentInChildren<SpriteRenderer>();

        sprite.flipX = mainCharacter.sprite.flipX;
        
        animator.Play(mainCharacter.lastAnimationName);
        Hide();
    }

    public void SwapAnimation(string animationName)
    {
        animator.Play(animationName);
        sprite.flipX = mainCharacter.sprite.flipX;
    }
    public override void Show(Transform from, Transform to)
    {
        base.Show(from,to);
        mainCharacter.AnimationSwitch += SwapAnimation;
    }

    public override void Move()
    {
        base.Move();
        sprite.flipX = mainCharacter.sprite.flipX;
    }
    public override void Hide()
    {
        base.Hide();
        mainCharacter.AnimationSwitch -= SwapAnimation;
    }
}

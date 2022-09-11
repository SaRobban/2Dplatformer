using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Note changes are permanent
/// </summary>
[CreateAssetMenu(fileName = "CharacterStats", menuName = "Character")]
public class CharacterStats : ScriptableObject
{
    [SerializeField] private float walkSpeed = 4;
    [SerializeField] private float jumpStr = 10;
    [SerializeField] private float normalGravity = 20;
    [SerializeField] private float jumpHoldGravity = 15;
    [SerializeField] private float jumpFallGravity = 40;
    [SerializeField] private float jumpForgiveness = 0.25f;
    [SerializeField] private int dubbleJumps = 0;
    [SerializeField] private float maxWallSlideSpeed = 2f;

    public float WalkSpeed => walkSpeed;
    public float JumpStr => jumpStr;
    public float NormalGravity => normalGravity;
    public float JumpHoldGravity => jumpHoldGravity;
    public float JumpFallGravity => jumpFallGravity;
    public float JumpForgiveness => jumpForgiveness;
    public int DubbleJumps => dubbleJumps;
    public float MaxWallSlideSpeed => maxWallSlideSpeed;

    public void SetDubbleJumps(int num)
    {
        dubbleJumps = num;
    }
}

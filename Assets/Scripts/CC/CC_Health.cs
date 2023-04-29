using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class CC_Health
{
    MainCharacter owner;
    public int health;
    public Action<int> healthChange;
    public CC_Health(MainCharacter owner)
    {
        this.owner = owner;
        SetHPToFull();
    }

    public void SetHPToFull()
    {
        health = owner.stats.FullHP;
        healthChange?.Invoke(health);
    }

    public void SetHPTo(int hp)
    {
        health = hp;
        healthChange?.Invoke(health);
    }

    public void AddHP(int extraHealth)
    {
        health += extraHealth;
        if(owner.stats.FullHP < health)
            health = owner.stats.FullHP;
        
        healthChange?.Invoke(health);
    }

    public void SubstractHP(int dmg)
    {
        health -= dmg;
        healthChange?.Invoke(health);
    }

    public bool IsDead()
    {
        if(health < 1)
        {
            return true;
        }
        return false;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HPTest : Item
{
    internal override void InitializeItem()
    {
        base.InitializeItem();
       
    }
    public override void OnOptionCancel_World()
    {
        base.OnOptionCancel_World();
    }


    public override void OnOptionOne_World()
    {
        AddThisToInventory();
        base.OnOptionOne_World();
    }

    public override void OnOptionTwo_World()
    {
        base.OnOptionTwo_World();
    }

    public override void OnInteract_World()
    {
        base.OnInteract_World();
    }


    public override void OnOptionCancel_Inventory()
    {
        base.OnOptionCancel_Inventory();
    }

    public override void OnOptionOne_Inventory()
    {
        PlayerMain.mainCharacter.healthSystem.AddHP(5);
        base.OnOptionOne_Inventory();
    }

    public override void OnOptionTwo_Inventory()
    {
        base.OnOptionTwo_Inventory();
    }
}

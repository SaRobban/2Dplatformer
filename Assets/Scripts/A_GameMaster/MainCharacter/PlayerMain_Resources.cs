using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// TODO : Make spawner?
/// </summary>
public class PlayerMain_Resources : MonoBehaviour
{
    public MainCharacter mainCharacter;
    public InventoryRoot inventory;

    public void Awake()
    {
        PlayerMain.SetupPlayerMain(this);
    }
}
public static class PlayerMain
{
    public static bool pauseGame { get; private set; }
    public static MainCharacter mainCharacter { get; private set; }
    public static InventoryRoot inventory { get; private set; }
    public static void SetupPlayerMain(PlayerMain_Resources playerMain_Resources)
    {
        mainCharacter = playerMain_Resources.mainCharacter;
        inventory = playerMain_Resources.inventory;
    }
}
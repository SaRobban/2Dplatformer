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
    public DialogSystem dialogSystem;
    public UI_InfoBox infoBox;
    public void Awake()
    {
        /*
        PlayerMain.mainCharacter = mainCharacter;
        PlayerMain.inventory = inventory;
        PlayerMain.dialogSystem = dialogSystem;
        PlayerMain.infoBox = infoBox;
        */
        PlayerMain.SetupPlayerMain(this);
    }
}
public static class PlayerMain
{
    public static bool pauseGame { get; private set; }
    public static MainCharacter mainCharacter { get; private set; }
    public static InventoryRoot inventory { get; private set; }
    public static DialogSystem dialogSystem { get; private set; }
    public static UI_InfoBox infoBox { get; private set; }


    public static void SetupPlayerMain(PlayerMain_Resources playerMain_Resources)
    {
        mainCharacter = playerMain_Resources.mainCharacter;
        inventory = playerMain_Resources.inventory;
        dialogSystem = playerMain_Resources.dialogSystem;
        infoBox = playerMain_Resources.infoBox;
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// TODO : Move all canvas stuff here!
/// </summary>
public class CanvasMagager_Recorces : MonoBehaviour
{

    public UI_DeathOverlay deathOverlay;
    public UI_CurtainScript fade;
    public UI_DialogSystem dialogSystem;
    public UI_InfoBox infoBox;
    void Awake()
    {
        CanvasManager.SetupCanvas(this);
    }
}


public static class CanvasManager
{
    public static UI_DeathOverlay deathOverlay { get; private set; }
    public static UI_CurtainScript curtain { get; private set; }
    public static UI_DialogSystem dialogSystem { get; private set; }
    public static UI_InfoBox infoBox { get; private set; }

    public static void SetupCanvas(CanvasMagager_Recorces recorces)
    {
        deathOverlay = recorces.deathOverlay;
        curtain = recorces.fade;
        dialogSystem = recorces.dialogSystem;
        infoBox = recorces.infoBox;

        Debug.Log("<color:yellow>Canvas was Setuped!!!</color>");
    }
}
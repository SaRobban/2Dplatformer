using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager_Recorce : MonoBehaviour
{
    public Transform cameraTruck;
    public CameraFunction controll;

    void Awake()
    {
        CameraManager.SetupSceneCamera(this);
    }
}

public static class CameraManager
{
    public static Transform cameraTruck { get; private set; }
    public static CameraFunction controll { get; private set; }
    public static void SetupSceneCamera(CameraManager_Recorce recorce)
    {
        cameraTruck = recorce.cameraTruck;
        controll = recorce.controll;
    }
}


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneMaster : MonoBehaviour
{
    public Transform startPos;


    public class SceneScript
    {
        public Vector3 cameraStartPos;
        public Vector3 cameraTarget;

    }

    Camera playerCamera;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Start LevelScript");
        CanvasManager.curtain.FadeIn(4);
        CameraManager.controll.aTargetReached += OnStep2;
        CameraManager.controll.MoveFromTo(startPos.position + Vector3.up * 5, startPos, 1);

        PlayerManager.mainCharacter.ChangeStateTo<CC_DialogEyesClosed>();
    }

    void OnStep2()
    {
        CameraManager.controll.aTargetReached -= OnStep2;

        CameraManager.controll.SetStatic();

        PlayerManager.mainCharacter.ChangeStateTo<CC_Dialog>();

        CanvasManager.dialogSystem.a_OnDialogFinished += OnStep3;
        CanvasManager.dialogSystem.EnterDialogFor("Intro");

    }

    void OnStep3() { 
        CameraManager.controll.TargetOne(PlayerManager.mainCharacter.transform);
        PlayerManager.mainCharacter.ChangeStateTo<CC_Walk>();


    }
}



using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GameMaster_ExecuteAction : MonoBehaviour
{
    public Action UpdateEveryFrame;
    public Action UpdateInGameTime;
    public Action OnInteractionKey;

    private void Update()
    {
        UpdateEveryFrame?.Invoke();

        if (!GameMaster.Paused)
            UpdateInGameTime?.Invoke();

        if (Input.GetButtonDown("Interact"))
        {
            OnInteractionKey?.Invoke();
        }
    }
}

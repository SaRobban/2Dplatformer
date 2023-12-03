using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMaster_Resource : MonoBehaviour
{
    void Awake()
    {
        GameMaster.SetResources(this);
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            GameMaster.ToggleFreeze();
        }
    }

    /*
    private GameMaster_ExecuteAction FindExecuteAction()
    {
        if (executeAction != null)
            return executeAction;

        if (TryGetComponent(out GameMaster_ExecuteAction ea))
        {
            return ea;
        }

        return gameObject.AddComponent<GameMaster_ExecuteAction>();
    }*/
}


public static class GameMaster
{
    public static float GameSpeed => gameSpeed;
    private static float gameSpeed = 1;
    public static bool freeze = false;
    public static System.Action a_OnFreeze;
    public static System.Action a_OnUnFreeze;
    public static void ToggleFreeze()
    {
        if (freeze)
        {
            UnFreeze();
            return;
        }
        Freeze();
    }

    public static void SetResources(GameMaster_Resource resource)
    {
    }

    public static void Freeze()
    {
        freeze = true;
        gameSpeed = 0;
        a_OnFreeze?.Invoke();
    }
    public static void UnFreeze()
    {
        freeze = false;
        gameSpeed = 1;
        a_OnUnFreeze?.Invoke();
    }

}
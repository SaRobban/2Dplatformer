using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMaster_Resource : MonoBehaviour
{
    public GameMaster_ExecuteAction executeAction;
    public UpdateGameObjects updateGameObjects;


    void Awake()
    {
        executeAction = FindExecuteAction();
        updateGameObjects = gameObject.AddComponent<UpdateGameObjects>();
        GameMaster.SetResources(this);
    }
    
    private GameMaster_ExecuteAction FindExecuteAction()
    {
        if (executeAction != null)
            return executeAction;

        if (TryGetComponent(out GameMaster_ExecuteAction ea))
        {
            return ea;
        }

        return gameObject.AddComponent<GameMaster_ExecuteAction>();
    }
}


public static class GameMaster
{
    public static bool Paused => pause;
    private static bool pause;

    public static event System.Action A_OnPause;
    public static event System.Action A_OnUnPause;
    
    public static event System.Action A_OnFreezeScene;
    public static event System.Action A_OnUnFreezeScene;

    public static void PauseGame()
    {
        pause = true;
        A_OnPause?.Invoke();
    }
    public static void UnPause()
    {
        pause = false;
        A_OnUnPause?.Invoke();
    }

    public static void FreezeScene()
    {
        Debug.Log("Gamemaster Invoke freeze");
        A_OnFreezeScene?.Invoke();
    }

    public static void UnFreezeScene()
    {
        A_OnUnFreezeScene?.Invoke();
    }

    public static void TogglePause()
    {
        if(pause)
        {
            UnPause();
            return;
        }
        PauseGame();
    }

    public static void SetResources(GameMaster_Resource resource)
    {
    }
}

public interface GameMasterUpdate
{
    void GM_OnPause();
    void GM_OnUnPause();

    void GM_FreezeGame();
    void GM_UnFreezeGame();
}
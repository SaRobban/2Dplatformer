using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpdateGameObjects : MonoBehaviour
{
    List<GameMasterUpdate> updateSceneObjectsList = new List<GameMasterUpdate>();

    // Start is called before the first frame update
    void Awake()
    {

    }
    public void AddObjectToUpdateList(GameMasterUpdate obj)
    {
        if(ListExsist())
            updateSceneObjectsList[0].GM_OnPause();

        updateSceneObjectsList.Insert(0,obj);
        updateSceneObjectsList[0].GM_OnUnPause();
    }
    public void RemoveObjectFromUpdateList(GameMasterUpdate obj)
    {
        if (!ListExsist())
            return;

        updateSceneObjectsList.Remove(obj);

        if(ListExsist())
            updateSceneObjectsList[0].GM_OnUnPause();
    }

    private bool ListExsist()
    {
        if (updateSceneObjectsList.Count <= 0)
            return false;
        return true;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            GameMaster.TogglePause();
        }
    }
   

    public void OnPause()
    {
        foreach (GameMasterUpdate upt in updateSceneObjectsList)
            upt.GM_OnPause();
    }

    public void OnUnPause()
    {
        foreach (GameMasterUpdate upt in updateSceneObjectsList)
            upt.GM_OnUnPause();
    }
}


using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public static class CanvasFocus
{

    static GameObject lastFocus = null;


    public static void SetFocus(GameObject toThis)
    {
        lastFocus = EventSystem.current.currentSelectedGameObject;
        EventSystem.current.SetSelectedGameObject(toThis);
    }
   
    public static void Unfocus()
    {
        if (lastFocus != null && lastFocus.activeSelf )
            EventSystem.current.SetSelectedGameObject(lastFocus);
    }

   public static void UpdateModules()
    {
        EventSystem.current.UpdateModules();
    }
}
/*
public interface CanvasPriority
{
    int canvasPrio { get; set; }
}
*/
using UnityEngine;
using UnityEngine.Events;
using System;
using System.Collections;

using System.Collections.Generic;


//public delegate void DoStuff(); //Declaring a function that returns void and takes in no Parameters
public class GameEvents : MonoBehaviour
{
    public static GameEvents _instance;

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Debug.LogError("More than one Instance of Sigleton : " + name);
            Destroy(this.gameObject);
            return;
        }
        else
        {
            _instance = this;
        }
    }

    public event Action<Vector3> onEvent;

    public void DoStuff(Vector3 pos)
    {
        if (onEvent != null)
        {
            Debug.Log("Do Stuff");
            onEvent(pos);
        }
    }
}
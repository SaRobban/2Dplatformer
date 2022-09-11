using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameDoSomthingOnEvent : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        //Add as listner
        GameEvents._instance.onEvent += DoSomething;
    }

    void DoSomething(Vector3 pos)
    {
        if ((transform.position - pos).sqrMagnitude < 10)
            Debug.Log("Action Here", gameObject);
        else
            Debug.Log("Action Out of range", gameObject);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateEvent : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Jump"))
        {
            Debug.Log("Pressed Jump");
            GameEvents._instance.DoStuff(transform.position);
        }
    }
}

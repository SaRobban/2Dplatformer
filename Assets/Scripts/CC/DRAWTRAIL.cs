using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DRAWTRAIL : MonoBehaviour
{
    Vector2 lastPos = Vector2.zero;
    float length = 5;
    Color color = Color.red;
    MainCharacter stateM;
    // Start is called before the first frame update
    void Start()
    {
        stateM = GetComponent<MainCharacter>();
        lastPos = transform.position;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(stateM.input.JumpWithForgiveness)
        Debug.DrawLine(lastPos, transform.position, Color.yellow, length);
        else
        Debug.DrawLine(lastPos, transform.position, color, length);
        lastPos = transform.position;
    }
}

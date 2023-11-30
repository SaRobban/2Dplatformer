using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClimbebleVolume : MonoBehaviour
{

    public bool Y;
    public bool X;

    private void OnTriggerStay2D(Collider2D collision)
    {
        Character_Move cMove = collision.GetComponent<Character_Move>();
        if (cMove != null)
        {
            if (X)
            {
                cMove.canClimbX = true;
            }
            if (Y)
            {
                cMove.canClimbY = true;
            }
            cMove.climbCenter = transform;
            Debug.DrawRay(transform.position, Vector3.up, Color.yellow);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        Character_Move cMove = collision.GetComponent<Character_Move>();
        if (cMove != null)
        {
            cMove.canClimbX = false;
            cMove.canClimbY = false;
        }
    }
}
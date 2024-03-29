using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogTrigger : MonoBehaviour
{
    bool start = true;
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (start == false)
            return;

        if (collision.tag == "Player")
        {
            Debug.Log("<color=yellow>Player entered dialog trigger</color>");
            if (PlayerManager.mainCharacter.flags.Grounded)
            {
                CanvasManager.dialogSystem.EnterDialogFor(transform.name);
                start = false;
            }
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            CanvasManager.dialogSystem.HaltDialog();
            start = true;
        }
    }
}

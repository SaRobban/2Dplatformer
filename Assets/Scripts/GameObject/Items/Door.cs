using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField] private Key_Item itemReq;
    private CC_Inventory inventory;

    [SerializeField] private Animator anim;
    [SerializeField] private bool open;
    [SerializeField] private bool plAtDoor;

    public Door exit;

    private void Start()
    {
        if(exit == null)
        {
            print("Door " + transform.name + " leads to noware");
        }
        if (exit.exit != this)
        {
            if (exit.exit == null)
            {
                exit.exit = this;
            }
            else
            {
                print("Warning Door " + exit.transform.name + " dont share exit");
            }
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            if (Input.GetAxis("Vertical") > 0) //action button????
            {
                if (!open)
                {
                    bool hasKey = collision.GetComponent<CC_Inventory>().RemoveItem(itemReq);

                    if (hasKey)
                        DoorOpen();
                }
                else
                {
                    print("player walked trought door " + transform.name + collision.transform.name);
                    Character_Move cm = collision.GetComponent<Character_Move>();
                    cm.telepotDestination = exit.transform.position;
                    cm.teleport = true;
                }
            }
        }
    }

    public void DoorOpen()
    {
        open = true;
        anim.SetBool("Open", open);
    }
}

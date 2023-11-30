using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal_connector : MonoBehaviour
{
    [Header("playerScript")]
    public GameObject pl;
    public bool inPn;
    public bool one;

    [Header("Potals (place destination as child")]
    private Portal_Node pnOne;
    private Portal_Node pnTwo;

    public bool open;
    public bool locked;
    public Key_Item key;

    private Animator animOne;
    private Animator animTwo;


    // Start is called before the first frame update
    void Start()
    {
        Portal_Node[] nodes = transform.GetComponentsInChildren<Portal_Node>();
        if(nodes.Length >= 2)
        {
            nodes[0].pc = this;
            nodes[1].pc = this;

            nodes[0].id = 0;
            nodes[1].id = 1;

            pnOne = nodes[0];
            pnTwo = nodes[1];

            animOne = nodes[0].transform.GetComponent<Animator>();
            animTwo = nodes[1].transform.GetComponent<Animator>();
        }
        else
        {
            print("Portal missing destinations/ nodes. " + transform.name);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Jump"))
        {
            if (inPn)
            {
                if (open)
                {
                    PassThrough();
                }

                if (locked == true)
                {
                    bool hasKey = pl.GetComponent<CC_Inventory>().RemoveItem(key);
                    if (hasKey)
                        DoorOpen();
                }
                else
                {
                    DoorOpen();
                }
            }
        }
    }
    void PassThrough()
    {
        print("pass here!!");
        if (one)
        {
            pl.GetComponent<Character_Move>().telepotDestination = pnTwo.transform.position;
            pl.GetComponent<Character_Move>().teleport = true;
        }
        else
        {
            pl.GetComponent<Character_Move>().telepotDestination = pnOne.transform.position;
            pl.GetComponent<Character_Move>().teleport = true;
        }
    }
    public void DoorOpen()
    {
        open = true;
        animOne.SetBool("Open", open);
        animTwo.SetBool("Open", open);
    }
}

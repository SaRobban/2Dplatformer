using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TakeItem : MonoBehaviour
{
    [SerializeField] private Key_Item itemReq;
    private CC_Inventory inventory;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        collision.GetComponent<CC_Inventory>().RemoveItem(itemReq);
    }
}

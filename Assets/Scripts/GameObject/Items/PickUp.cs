using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUp : MonoBehaviour
{
    [SerializeField] private Key_Item item;
    public int contains = 1;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (contains > 0)
        {
            Key_Item newKey = Instantiate(item);
            collision.GetComponent<CC_Inventory>().AddItem(newKey);
            contains--;
        }
    }
}

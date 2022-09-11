using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CC_Inventory : MonoBehaviour
{
    public Transform listStart;

    public Key_Item[] items;
    
    public void AddItem(Key_Item item)
    {
        //Set new Legnth
        int l = items.Length;
        Key_Item[] newItems = new Key_Item[l+1];
        
        //Add Items
        for (int i = 0; i < items.Length; i++)
        {
            newItems[i] = items[i];
        }

        newItems[l] = item;

        //Set new Array
        items = newItems;

        DisplayList();
    }

    public bool HasItem(Key_Item item)
    {
        bool hasItem = false;

        if (items.Length > 0)
        {
            //Find item in array to remove
            int removeThis = 1000;
            for (int r = 0; r < items.Length; r++)
            {
                if (items[r].keyName == item.keyName)
                {
                    hasItem = true;
                    removeThis = r;
                }
            }

        }
        return hasItem;
    }

    public bool RemoveItem(Key_Item item)
    {
        bool hasItem = false;
        if (items.Length > 0)
        {
            //Find item in array to remove
            int removeThis = 1000;
            for (int r = 0; r < items.Length; r++)
            {
                if (items[r].keyName == item.keyName)
                {
                    hasItem = true;
                    removeThis = r;
                }
            }

            //Set new Length
            int l = items.Length;
            l--;
            Key_Item[] newItems = new Key_Item[l];

            //Create new array of items excluding removeditem
            int n = 0;
            for (int i = 0; i < items.Length; i++)
            {
                if (i != removeThis)
                {
                    newItems[n] = items[i];
                    n++;
                }
                else
                {
                    Destroy(items[i].gameObject);
                }
            }

            //Set new Array
            items = newItems;

            DisplayList();
        }
        return hasItem;
    }

    public void DisplayList()
    {
        for(int i = 0; i < items.Length; i++)
        {
            items[i].transform.position = listStart.position + Vector3.down * i;
            items[i].transform.parent = listStart;
        }
    }
}

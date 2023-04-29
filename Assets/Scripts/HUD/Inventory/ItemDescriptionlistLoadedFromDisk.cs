using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// This is a file manager class for loading item description from excel
/// </summary>
public static class ItemDescriptionlistLoadedFromDisk
{
    private static Dictionary<string, Item.ItemDescription> itemDescriptionDictionary = null;
    public static Item.ItemDescription GetItemDescriptionFromList(string className)
    {
        if (itemDescriptionDictionary == null)
        {
            string data = IO_FileManager.LoadFromHDD(Application.dataPath + "/Files/Items/items.tsv");
            itemDescriptionDictionary = ProcessData(data);
            Debug.Log("Items in ItemLoadedList : " + itemDescriptionDictionary.Count);
        }


        foreach (string key in itemDescriptionDictionary.Keys)
        {
            Debug.Log("Keys : " + key);
        }


        if (itemDescriptionDictionary.ContainsKey(className))
        {
            return itemDescriptionDictionary[className];
        }
        return null;
    }


    private static Dictionary<string, Item.ItemDescription> ProcessData(string rawData)
    {
        string[] enterSeparated = SeparateByEnter(rawData);
        Dictionary<string, Item.ItemDescription> dictionaryItems = new Dictionary<string, Item.ItemDescription>();


        for (int i = 0; i < enterSeparated.Length; i++)
        {
            if (string.IsNullOrEmpty(enterSeparated[i]))
                continue;

            string[] lineContent = SeperateByTab(enterSeparated[i], 9);
            string classType = lineContent[0];

            if (string.IsNullOrEmpty(classType) || classType.StartsWith("0 "))
                continue;

            Item.ItemDescription newListItem = new Item.ItemDescription();
            newListItem.header = lineContent[1];
            newListItem.descriptionText = lineContent[2];
            newListItem.worldActionOne = lineContent[3];
            newListItem.worldActionTwo = lineContent[4];
            newListItem.worldCancelOption = lineContent[5];
            newListItem.inventoryActionOne = lineContent[6];
            newListItem.inventoryActionTwo = lineContent[7];
            newListItem.inventoryCancelOption = lineContent[8];

            dictionaryItems.Add(classType, newListItem);
        }
        return dictionaryItems;
    }
    //TODO : Move to IO_Filemaneger.support????
    private static string[] SeparateByEnter(string data)
    {
        string[] result = data.Split(new[] { '\r', '\n' });
        return result;
    }

    private static string[] SeperateByTab(string data, int separations)
    {

        string[] tabSeperated = data.Split(new[] { '\t' });

        string[] result = new string[separations];

        //        int x = 0;
        for (int y = 0; y < tabSeperated.Length; y++)
        {
            //  if (!string.IsNullOrEmpty(tabSeperated[y]))
            //  {
            result[y] = tabSeperated[y];
            //  x++;
            // }
        }
        return result;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SavePoint : MonoBehaviour
{
    [SerializeField] private bool save;
    [SerializeField] private bool load;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (save)
            FileManager.Command.SaveData(collision.transform);

        if (load)
            FileManager.Command.LoadData(collision.transform);
    }
}

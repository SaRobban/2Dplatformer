using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RequireKeySensor : MonoBehaviour
{
    [SerializeField] private Item requeredKey;
    MainCharacter player;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out MainCharacter getPlayer))
        {
            this.player = getPlayer;
            player.A_OnInteract += OpenInventory;
            player.A_OnUseSpecialItem += TryKey;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        UnSubscibe();
    }
    private void OnDisable()
    {
        UnSubscibe();
    }
    private void OnDestroy()
    {
        UnSubscibe();
    }

    void TryKey(Item key)
    {
        if (key == requeredKey)
        {
            if (key.ConsumeOnUse)
                key.RemoveThisFromInventory();

            PlayerMain.dialogSystem.EnterDialogFor("LockRightKey");
            PlayerMain.inventory.m_ui.CloseInventory();

            return;
        }

        Debug.Log("Wrong Key");
        OnWrongKey();
    }

    void OnWrongKey()
    {
        PlayerMain.dialogSystem.EnterDialogFor("LockWrongKey");
    }


    void OpenInventory()
    {
        Debug.Log("Force Open Inventory");
        PlayerMain.inventory.m_ui.OpenInventory();
    }

    void UnSubscibe()
    {
        if (player != null)
        {
            player.A_OnInteract -= OpenInventory;
            player.A_OnUseSpecialItem -= TryKey;
            player = null;
        }
    }
}

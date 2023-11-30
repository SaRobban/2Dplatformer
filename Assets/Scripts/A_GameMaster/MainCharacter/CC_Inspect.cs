using UnityEngine;

//TODO : BYPASS THIS SCRIPT !?
public class CC_Inspect : MonoBehaviour
{
    System.Action A_OnInspect;
    Item activeItem;

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (activeItem != null)
            return;

        if (collision.TryGetComponent(out Item item))
        {
            activeItem = item;
            PlayerMain.mainCharacter.A_OnInteract += OnInspect;
        }
    }

    public void OnInspect()
    {
        activeItem.OnInteract_World();
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        //TODO : Find better way to dump and identefy item
        if (collision.TryGetComponent(out Item item))
        {
            PlayerMain.mainCharacter.A_OnInteract -= OnInspect;
            activeItem = null;
        }
    }
}
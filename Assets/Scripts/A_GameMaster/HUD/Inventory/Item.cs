using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class Item : MonoBehaviour
{
    [System.Serializable]
    public class ItemDescription
    {
        [Space(30)]
        [TextAreaAttribute(1, 1)]
        public string header = null;
        [TextAreaAttribute]
        public string descriptionText = null;
        public string worldActionOne = null;
        public string worldActionTwo = null;
        public string worldCancelOption = null;
        public string inventoryActionOne = null;
        public string inventoryActionTwo = null;
        public string inventoryCancelOption = null;

    }

    [SerializeField] private bool consumeOnUse;
    public bool ConsumeOnUse => consumeOnUse;

    [SerializeField] private SpriteRenderer sprite;

    [Header("DescriptionOptions")]
    public ItemDescription description;


    private void Awake()
    {
        //TODO : Tryget!?
        sprite = GetComponent<SpriteRenderer>();
        InitializeItem();
    }



    internal virtual void InitializeItem()
    {
        description = ItemDescriptionlistLoadedFromDisk.GetItemDescriptionFromList(this.GetType().Name);

        if (description == null)
        {
            gameObject.SetActive(false);
            Debug.LogError("<color=red>Missing item description : </color>" + this.GetType().Name);
        }
    }


    public Color GetColor()
    {
        return sprite.color;
    }
    public Sprite GetSprite()
    {
        return sprite.sprite;
    }

    //Interaction from World
    public virtual void OnInteract_World()
    {
        ShowOnInfoBox();
    }

    public virtual void OnOptionCancel_World()
    {
        HideInfoBox();
    }
    public virtual void OnOptionOne_World()
    {
        HideInfoBox();
    }
    public virtual void OnOptionTwo_World()
    {
        HideInfoBox();
    }

    //Interaction from Inventory
    public virtual void OnInveroryClick()
    {
        Debug.Log("Clicked : " + transform.name);
    }
    public virtual void OnOptionCancel_Inventory()
    {
        PlayerManager.inventory.m_ui.HideInteractOptions();
    }
    public virtual void OnOptionOne_Inventory()
    {
        CloseInventorty();
    }
    public virtual void OnOptionTwo_Inventory()
    {
        CloseInventorty();
    }

    //Support functions
    internal void AddThisToInventory()
    {
        InventoryRoot.instance.AddItem(this);
        gameObject.SetActive(false);
    }
    internal void RemoveThisFromInventory()
    {
        InventoryRoot.instance.RemoveItem(this);
        gameObject.SetActive(false);
    }
    internal void ShowOnInfoBox()
    {
        CanvasManager.infoBox.Show(this);
    }
    internal void HideInfoBox()
    {
        CanvasManager.infoBox.Hide();
    }
    internal void CloseInventorty()
    {
        PlayerManager.inventory.m_ui.CloseInventory();
    }

    internal void CloseInventoryInteractOptions()
    {
        PlayerManager.inventory.m_ui.HideInteractOptions();
    }

    internal void OnCosumeItem()
    {
        // IfExistRemoveThisFromInventory();
        PlayerManager.inventory.RemoveItem(this);
        Destroy(this.gameObject);
    }
}

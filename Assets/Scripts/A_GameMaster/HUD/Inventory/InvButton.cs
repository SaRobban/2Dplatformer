using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;
public class InvButton : Button//, ISelectHandler, IDeselectHandler
{
    [SerializeField]
    private Image contentImage;

    public static Action Selected;
    public static Action DeSelected;
    private Item item;

    protected override void Awake()
    {
        base.Awake();
        if (contentImage == null)
        {
            foreach (Transform child in transform)
            {
                if (child.TryGetComponent(out Image img))
                {
                    contentImage = img;
                }
            }
        }
    }
    public void Activate(Item item)
    {
        gameObject.SetActive(true);
        this.item = item;
        Sprite sprite = item.GetSprite();
        contentImage.sprite = sprite;

        Navigation aMode = new Navigation();
        aMode.mode = Navigation.Mode.Automatic;
        navigation = aMode;
        onClick.AddListener(item.OnInveroryClick);
    }
    public void DeActivate()
    {
        onClick.RemoveAllListeners();
        this.item = null;
        gameObject.SetActive(false);
    }


    public override void OnDeselect(BaseEventData eventData)
    {
        base.OnDeselect(eventData);
        DeSelected?.Invoke();
    }

    public override void OnSelect(BaseEventData eventData)
    {
        base.OnSelect(eventData);
        PlayerManager.inventory.m_ui.ShowInfoFor(item);
        Selected?.Invoke();
    }
    public override void OnSubmit(BaseEventData eventData)
    {
        PlayerManager.inventory.m_ui.ShowInteractOptions(item, this);
        base.OnSubmit(eventData);
    }
}

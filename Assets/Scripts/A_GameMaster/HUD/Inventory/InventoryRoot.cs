using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class InventoryRoot : MonoBehaviour
{
    private static class InventoryList
    {
        private static List<Item> inventory = new List<Item>();

        public static event Action OnInventoryChange;

        public static void AddItem(Item item)
        {
            inventory.Add(item);
            OnInventoryChange?.Invoke();
        }
        public static void RemoveItem(Item item)
        {
            inventory.Remove(item);
            OnInventoryChange?.Invoke();
        }

        public static List<Item> GetList()
        {
            return inventory;
        }

        public static Sprite[] GetSprites()
        {
            List<Sprite> sprites = new List<Sprite>();
            foreach (Item item in inventory)
            {
                sprites.Add(item.GetComponent<SpriteRenderer>().sprite);
            }
            return sprites.ToArray();
        }
    }
    internal class UI_Inventory
    {
        private InventoryUIResources m_resources;
        private InvButton[] m_frames;
        private GameObject m_lastSelectedItem;


        public UI_Inventory(Canvas canvas, InventoryUIResources uiInventory)
        {
            m_resources = uiInventory;
            CC_InputControl.OnInventoryKey += ToggleVisibility;
            InventoryList.OnInventoryChange += UpdateInventory;

            //List inventory slots
            List<InvButton> framesList = new List<InvButton>();
            foreach (Transform child in m_resources.itemGrid)
            {
                if (child.gameObject.TryGetComponent(out InvButton frame))
                {
                    framesList.Add(frame);
                    frame.DeActivate();
                }
            }
            m_frames = framesList.ToArray();


            UpdateInventory();
            m_resources.inventoryRoot.gameObject.SetActive(false);
        }

        public void OnDestroy()
        {
            CC_InputControl.OnInventoryKey -= ToggleVisibility;
            InventoryList.OnInventoryChange -= UpdateInventory;
        }

        public void OpenInventory()
        {
            m_resources.inventoryRoot.gameObject.SetActive(true);
            m_resources.interactOptions.gameObject.SetActive(false);
            UpdateInventory();
            GameMaster.FreezeScene();
        }

        public void CloseInventory()
        {
            m_resources.inventoryRoot.gameObject.SetActive(false);
            GameMaster.UnFreezeScene();
        }

        public void ToggleVisibility()
        {
            if (m_resources.inventoryRoot.gameObject.activeSelf == false)
            {
                OpenInventory();
            }
            else
            {
                CloseInventory();
            }
        }
        public void UpdateInventory()
        {
            List<Item> items = InventoryList.GetList();

            int numberOfActiveItems = -1;
            for (int i = 0; i < m_frames.Length; i++)
            {
                m_frames[i].DeActivate();

                if (i < items.Count)
                {
                    numberOfActiveItems++;
                    m_frames[i].Activate(items[i]);
                }
            }

            //Set Selected to last listed item
            if (numberOfActiveItems > -1)
            {
                m_lastSelectedItem = m_frames[numberOfActiveItems].gameObject;
                CanvasFocus.SetFocus(m_frames[numberOfActiveItems].gameObject);
                m_frames[numberOfActiveItems].OnSelect(null);
            }
            else
            {
                NoPreviewAvalible();
            }
        }

        public void ShowInfoFor(Item item)
        {
            m_resources.itemPreview.color = item.GetColor();
            m_resources.itemPreview.sprite = item.GetSprite();
            m_resources.itemDescrition.SetText(item.description.header + "\n" + item.description.descriptionText);
        }


        private Selectable ActiveByContent(Button button, TMP_Text bText, string content)
        {
            if (string.IsNullOrEmpty(content))
            {
                button.gameObject.SetActive(false);
                return null;
            }

            bText.SetText(content);
            button.gameObject.SetActive(true);
            return button;
        }
        public void ShowInteractOptions(Item item, InvButton button)
        {
            m_resources.interactOptions.gameObject.SetActive(true);
            m_resources.interactOptions.transform.position = button.transform.position;
            Selectable focus;
            List<Selectable> selections = new List<Selectable>();
            focus = ActiveByContent(m_resources.optionTre, m_resources.optionTreText, item.description.inventoryCancelOption);
            if (focus != null && focus.gameObject.activeSelf)
                selections.Add(focus);

            focus = ActiveByContent(m_resources.optionTwo, m_resources.optionTwoText, item.description.inventoryActionTwo);
            if (focus != null && focus.gameObject.activeSelf)
                selections.Add(focus);

            focus = ActiveByContent(m_resources.optionOne, m_resources.optionOneText, item.description.inventoryActionOne);
            if (focus != null && focus.gameObject.activeSelf)
                selections.Add(focus);


            if (selections.Count > 0)
            {
                CanvasFocus.SetFocus(focus.gameObject);
                Debug.Log("count : " + selections.Count);
                for (int i = 0; i < selections.Count; i++)
                {
                    Navigation nav = new Navigation();
                    nav.mode = Navigation.Mode.Explicit;

                    int down = i - 1;
                    if (down < 0)
                        down = selections.Count - 1;

                    int up = i + 1;
                    if (up > selections.Count-1)
                        up = 0;

                    nav.selectOnDown = selections[down];
                    nav.selectOnUp = selections[up];
                    nav.wrapAround = true;
                    selections[i].navigation = nav;
                }
            }
            m_resources.optionOne.onClick.AddListener(item.OnOptionOne_Inventory);
            m_resources.optionTwo.onClick.AddListener(item.OnOptionTwo_Inventory);
            m_resources.optionTre.onClick.AddListener(item.OnOptionCancel_Inventory);
        }

        public void HideInteractOptions()
        {
            m_resources.optionOne.onClick.RemoveAllListeners();
            m_resources.optionTwo.onClick.RemoveAllListeners();
            m_resources.optionTre.onClick.RemoveAllListeners();
            m_resources.interactOptions.gameObject.SetActive(false);

            CanvasFocus.SetFocus(m_lastSelectedItem);
        }

        private void NoPreviewAvalible()
        {
            m_resources.itemPreview.color = Color.clear;
            m_resources.itemDescrition.SetText("Bag is empty!");
        }
    }

    public static InventoryRoot instance;

    [Header("UI")]
    [SerializeField] private Canvas canvas;
    [SerializeField] private InventoryUIResources uiPreFab;
    internal UI_Inventory m_ui;


    public void AddItem(Item item)
    {
        InventoryList.AddItem(item);
    }
    public void RemoveItem(Item item)
    {
        InventoryList.RemoveItem(item);
    }


    private void Awake()
    {
        MakeSingelton();
        canvas = GetOrCreateCanvas();
        InventoryUIResources uiInventoryObj = GetOrInstantiateUI();
        m_ui = new UI_Inventory(canvas, uiInventoryObj);
    }
    private void OnDestroy()
    {
        m_ui.OnDestroy();
    }
    private void MakeSingelton()
    {
        if (instance != null && instance != this)
        {
            Destroy(instance);
        }
        else
        {
            instance = this;
        }
    }
    private Canvas GetOrCreateCanvas()
    {
        if (canvas)
            return canvas;

        canvas = FindObjectOfType<Canvas>();
        if (canvas)
            return canvas;

        Debug.Log("No CanvasRenderer object could be found");

        GameObject newUI = new GameObject();
        newUI.transform.position = Vector3.zero;
        newUI.transform.rotation = Quaternion.identity;
        newUI.transform.localScale = Vector3.one;

        canvas = newUI.AddComponent<Canvas>();
        return canvas;

    }
    private InventoryUIResources GetOrInstantiateUI()
    {
        InventoryUIResources bag = FindObjectOfType(typeof(InventoryUIResources)) as InventoryUIResources;
        if (bag != null)
        {
            bag.transform.SetParent(canvas.transform);
            return bag;
        }

        if (uiPreFab == null)
            Debug.LogError("Unable to crate UI");

        bag = Instantiate(uiPreFab, canvas.transform);
        return bag;
    }
}
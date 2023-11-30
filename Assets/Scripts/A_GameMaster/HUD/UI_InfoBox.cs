using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class UI_InfoBox : MonoBehaviour
{
    [SerializeField] private GameObject inspectBox;
    [SerializeField] private TMP_Text header;
    [SerializeField] private TMP_Text descriptionText;
    [SerializeField] private TMP_Text buttonCancel_Text;
    [SerializeField] private TMP_Text buttonOne_Text;
    [SerializeField] private TMP_Text buttonTwo_Text;

    [SerializeField] private Button buttonCancel;
    [SerializeField] private Button buttonOne;
    [SerializeField] private Button buttonTwo;
   
    private Item currentItem;

    private void Start()
    {
        buttonCancel.onClick.AddListener(OnButtonCancel);
        buttonOne.onClick.AddListener(OnButtonOne);
        buttonTwo.onClick.AddListener(OnButtonTwo);
    }
    private void OnDestroy()
    {
        buttonCancel.onClick.RemoveListener(OnButtonCancel);
        buttonOne.onClick.RemoveListener(OnButtonOne);
        buttonTwo.onClick.RemoveListener(OnButtonTwo);
    }
    public void Show(Item item)
    {
        currentItem = item;
        SetInfo(item);
    }
    private void SetInfo(Item item)
    {
        if (item.description == null)
            Debug.Log("EMPTY");

        header.SetText(item.description.header);
        descriptionText.SetText(item.description.descriptionText);

        buttonCancel_Text.SetText(item.description.inventoryCancelOption);
        inspectBox.SetActive(true);

        GameObject focus = null;

        if (string.IsNullOrEmpty(item.description.worldCancelOption))
            buttonCancel.gameObject.SetActive(false);
        else
        {
            buttonCancel.gameObject.SetActive(true);
            buttonCancel_Text.SetText(item.description.worldCancelOption);
            focus = buttonCancel.gameObject;
        }

        if (string.IsNullOrEmpty(item.description.worldActionTwo))
            buttonTwo.gameObject.SetActive(false);
        else
        {
            buttonTwo.gameObject.SetActive(true);
            buttonTwo_Text.SetText(item.description.worldActionTwo);
            focus = buttonTwo.gameObject;
        }

        if (string.IsNullOrEmpty(item.description.worldActionOne))
            buttonOne.gameObject.SetActive(false);
        else
        {
            buttonOne.gameObject.SetActive(true);
            buttonOne_Text.SetText(item.description.worldActionOne);
            focus = buttonOne.gameObject;
        }

        if(focus != null)
            CanvasFocus.SetFocus(focus);
    }

    public void Hide()
    {
        inspectBox.SetActive(false);
    }
    public void OnButtonCancel()
    {
        currentItem.OnOptionCancel_World();
    }
    public void OnButtonOne()
    {
        currentItem.OnOptionOne_World();
    }
    public void OnButtonTwo()
    {
        currentItem.OnOptionTwo_World();
    }
}

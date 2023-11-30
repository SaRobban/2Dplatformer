using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class InventoryUIResources : MonoBehaviour
{
    [SerializeField]
    public GameObject backDrop;
    [SerializeField]
    public RectTransform inventoryRoot;
    [SerializeField]
    public RectTransform itemGrid;
    [SerializeField]
    public Image itemPreview;
    [SerializeField]
    public TMP_Text itemDescrition;

    [Header("Interact menu")]
    [SerializeField]
    public RectTransform interactOptions;
    [SerializeField]
    public Button optionOne;
    [SerializeField]
    public Button optionTwo;
    [SerializeField]
    public Button optionTre;
    [SerializeField]
    public TMP_Text optionOneText;
    [SerializeField]
    public TMP_Text optionTwoText;
    [SerializeField]
    public TMP_Text optionTreText;
}

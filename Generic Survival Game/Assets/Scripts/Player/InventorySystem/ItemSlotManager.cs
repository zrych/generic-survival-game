using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemSlotManager : MonoBehaviour, IPointerClickHandler
{
    public string itemName;
    public int quantity;
    private Sprite itemIcon;
    public bool isFull;
    private string itemDesc;
    public bool isEmpty = true;

    private int maxItems = 10;


    [SerializeField] private TMP_Text quantityText;
    [SerializeField] public Image itemImage;
    [SerializeField] private Image itemDescImage;
    [SerializeField] private TMP_Text itemDescText;
    [SerializeField] private TMP_Text itemNameDesc;
    [SerializeField] private Button swap1Button;
    [SerializeField] private Button swap2Button;
    [SerializeField] private Button swap3Button;

    [SerializeField] private Sprite emptySprite;

    public GameObject currentItem; // The visual representation of the item
    public GameObject selectedShade;

    public bool isSlotSelected;

    [HideInInspector] public Item item;

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            SelectSlot();
        }
    }

    public int StoreItem(Item item, int qty)
    {
        if (isFull) return qty;

        this.item = item;

        isEmpty = false;

        itemName = item.itemName;
        itemIcon = item.itemIcon;
        itemImage.sprite = itemIcon;

        itemDesc = item.itemDescription;
        quantity += qty;
        if (quantity >= maxItems)
        {
            quantityText.text = maxItems.ToString();
            quantityText.gameObject.SetActive(true);
            isFull = true;

            //return leftovers
            int extraItems = quantity - maxItems;
            quantity = maxItems;
            return extraItems;
        }

        quantityText.text = quantity.ToString();
        quantityText.gameObject.SetActive(true);

        return 0;
    }

    public void SelectSlot()
    {
        InventoryUIManager.Instance.DeselectAllSlots();
        if (selectedShade != null)
            selectedShade.SetActive(true);
        isSlotSelected = true;

        itemNameDesc.text = itemName;
        itemDescText.text = itemDesc;
        itemDescImage.sprite = itemIcon != null ? itemIcon : emptySprite;

        if (itemDescImage.sprite == null)
        {
            itemDescImage.sprite = emptySprite;
        }

        if (isEmpty == true)
        {
            SetSwapButtons(false);
        } else
        {
            SetSwapButtons(true);
        }
    }

    private void SetSwapButtons(bool decision)
    {
        swap1Button.gameObject.SetActive(decision);
        swap2Button.gameObject.SetActive(decision);
        swap3Button.gameObject.SetActive(decision);
    }

    public void EmptySlot()
    {
        item = null;
        itemName = null;
        quantity = 0;
        itemIcon = null;
        isFull = false;
        itemDesc = null;

        isEmpty = true;

        if (itemImage != null)
            itemImage.sprite = emptySprite;

        if (quantityText != null)
            quantityText.gameObject.SetActive(false);
    }
}
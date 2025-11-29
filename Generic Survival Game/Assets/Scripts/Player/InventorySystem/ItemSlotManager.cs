using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static UnityEditor.Progress;

public class ItemSlotManager : MonoBehaviour, IPointerClickHandler
{
    public string itemName;
    public int quantity;
    private Sprite itemIcon;
    public bool isFull;
    private string itemDesc;

    private int maxItems = 10;

    [SerializeField] private TMP_Text quantityText;
    [SerializeField] private Image itemImage;
    [SerializeField] private Image itemDescImage;
    [SerializeField] private TMP_Text itemDescText;
    [SerializeField] private TMP_Text itemNameDesc;

    [SerializeField] private Sprite emptySprite;


    public GameObject selectedShade;
    private bool isSlotSelected;

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

    private void SelectSlot()
    {
        InventoryUIManager.Instance.DeselectAllSlots();
        selectedShade.SetActive(true);
        isSlotSelected = true;
        itemNameDesc.text = itemName;
        itemDescText.text = itemDesc;
        itemDescImage.sprite = itemIcon;

        if (itemDescImage.sprite == null)
        {
            itemDescImage.sprite = emptySprite;
        }
    }
}

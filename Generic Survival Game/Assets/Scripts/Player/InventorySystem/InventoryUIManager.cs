using UnityEngine;

public class InventoryUIManager : MonoBehaviour
{
    public GameObject inventoryUI;
    private bool isToggled = false;
    [SerializeField] private GameObject crosshair;
    [SerializeField] private ItemSlotManager[] itemSlot;

    public static InventoryUIManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        inventoryUI.SetActive(false); // make sure it's off at start
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            isToggled = !isToggled;

            inventoryUI.SetActive(isToggled);

            if (isToggled == true) crosshair.SetActive(false);
            else crosshair.SetActive(true);
        }
    }

    public int AddItem(Item item, int quantity)
    {
        for (int i = 0; i < itemSlot.Length; i++)
        {
            if (itemSlot[i].isFull == false && itemSlot[i].itemName == item.itemName || itemSlot[i].quantity == 0)
            {
                int leftOverItems = itemSlot[i].StoreItem(item, quantity);
                if (leftOverItems > 0) leftOverItems = AddItem(item, leftOverItems);
                return leftOverItems;
            }
        }
        return quantity;
    }

    public void DeselectAllSlots()
    {
        for (int i = 0; i < itemSlot.Length; ++i)
        {
            itemSlot[i].selectedShade.SetActive(false);
        }
    }
}
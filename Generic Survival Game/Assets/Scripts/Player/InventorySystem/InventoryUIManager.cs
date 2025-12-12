using UnityEngine;

public class InventoryUIManager : MonoBehaviour
{
    public GameObject inventoryUI;
    private bool isToggled = false;
    [SerializeField] public ItemSlotManager[] itemSlot = new ItemSlotManager[15];

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
        }
    }

    public void DeleteItem(Item item)
    {
        for (int i = 0; i < itemSlot.Length; i++)
        {
            if (itemSlot[i].item == item)
            {
                itemSlot[i].EmptySlot();
                return;
            }
        }
    }

    public int AddItem(Item item, int quantity)
    {
        for (int i = 0; i < itemSlot.Length; i++)
        {
            if (!itemSlot[i].isFull && itemSlot[i].itemName == item.itemName)
            {
                int leftOverItems = itemSlot[i].StoreItem(item, quantity);
                if (leftOverItems > 0) return AddItem(item, leftOverItems);
                return leftOverItems;
            }
        }

        for (int i = 0; i < itemSlot.Length; i++)
        {
            if (itemSlot[i].quantity == 0)
            {
                int leftOverItems = itemSlot[i].StoreItem(item, quantity);
                if (leftOverItems > 0) return AddItem(item, leftOverItems);
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
            itemSlot[i].isSlotSelected = false;
        }
    }
}
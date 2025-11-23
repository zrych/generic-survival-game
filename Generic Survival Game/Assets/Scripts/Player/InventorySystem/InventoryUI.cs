using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    public GameObject inventoryPanel;
    public InventorySlot[] slots;
    void Start()
    {
        inventoryPanel.SetActive(false);
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            ToggleInventory();
        }
    }

    void OnEnable()
    {
        RefreshUI();
    }

    void RefreshUI()
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (InventoryManager.instance.items[i] != null)
            {
                slots[i].SetItem(InventoryManager.instance.items[i]);
            } else
            {
                slots[i].ClearSlot();
            }
        }
    }
    void ToggleInventory()
    {
        bool isOpen = inventoryPanel.gameObject.activeSelf;
        inventoryPanel.SetActive(!isOpen);
        Time.timeScale = isOpen ? 1 : 0; //pauses movement when inventory opens
    }
}

using UnityEngine;
using System.Collections.Generic;

public class HotbarUI : MonoBehaviour
{
    public InventoryManager inventory;
    public List<InventorySlot> hotbarSlots;
    void Start()
    {

    }
    public void RefreshHotbar()
    {
        for (int i = 0; i < hotbarSlots.Count; i++)
        {
            if (i < inventory.items.Length && inventory.items[i] != null)
            {
                hotbarSlots[i].SetItem(inventory.items[i]);
            } else {
                hotbarSlots[i].ClearSlot();
            }
        }
    }
}

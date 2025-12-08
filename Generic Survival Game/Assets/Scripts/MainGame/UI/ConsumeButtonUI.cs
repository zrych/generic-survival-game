using UnityEngine;

public class ConsumeButtonUI : MonoBehaviour
{
    public void ConsumeItem()
    {
        InventoryUIManager inventory = InventoryUIManager.Instance;
        for (int i = 0; i < inventory.itemSlot.Length; i++)
        {
            if (inventory.itemSlot[i].isSlotSelected)
            {
                inventory.itemSlot[i].ConsumeItem();
                return;
            }
        }
    }
}

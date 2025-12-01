using UnityEngine;

public class CraftButtonUI : MonoBehaviour
{
    [SerializeField] Item[] itemsToGive;
    
    public void CraftPlanks()
    {
        InventoryUIManager inventory = InventoryUIManager.Instance;
        ItemSlotManager slot = null;
        bool hasLog = false;

        Item planks = null;
        for (int i = 0; i < itemsToGive.Length; i++)
        {
            if (itemsToGive[i].itemId == "002")
            {
                planks = itemsToGive[i];
                break;
            }
        }

        for (int i = 0; i < inventory.itemSlot.Length; i++)
        {
            if (inventory.itemSlot[i].item != null && inventory.itemSlot[i].item.itemId == "001") //ID of log
            {
                slot = inventory.itemSlot[i];
                hasLog = true;
                break;
            }
        }

        if (hasLog == true)
        {
            slot.DeductItem(1);
            inventory.AddItem(planks, 2);
        } else
        {
            return;
        }
    }

    public void CraftWoodAxe()
    {
        InventoryUIManager inventory = InventoryUIManager.Instance;
        ItemSlotManager planksSlot = null;
        ItemSlotManager sticksSlot = null;
        bool hasPlanks = false;
        bool hasSticks = false;

        Item woodAxe = null;

        for (int i = 0; i < itemsToGive.Length; i++)
        {
            if (itemsToGive[i].itemId == "004")
            {
                woodAxe = itemsToGive[i];
                break;
            }
        }

        for (int i = 0; i < inventory.itemSlot.Length; i++)
        {
            if (inventory.itemSlot[i].item != null && inventory.itemSlot[i].item.itemId == "002") //ID of planks
            {
                planksSlot = inventory.itemSlot[i];
                int remaining = planksSlot.quantity - 3;
                if (remaining >= 0)
                {
                    hasPlanks = true;
                    break;
                }
                else
                {
                    hasPlanks = false;
                    break;
                }
            }
        }
        for (int i = 0; i < inventory.itemSlot.Length; i++)
        {
            if (inventory.itemSlot[i].item != null && inventory.itemSlot[i].item.itemId == "003") //ID of sticks
            {
                sticksSlot = inventory.itemSlot[i];
                int remaining = sticksSlot.quantity - 2;
                if (remaining >= 0)
                {
                    hasSticks = true;
                    break;
                }
                else
                {
                    hasSticks = false;
                    break;
                }
            }
        }

        if (hasPlanks == true && hasSticks == true)
        {
            planksSlot.DeductItem(3);
            sticksSlot.DeductItem(2);
            inventory.AddItem(woodAxe, 1);
        }
        else
        {
            return;
        }
    }

}

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
                int remaining = planksSlot.quantity - 4;
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
            woodAxe.currentDurability = woodAxe.maxDurability;
            planksSlot.DeductItem(4);
            sticksSlot.DeductItem(2);
            inventory.AddItem(woodAxe, 1);
        }
        else
        {
            return;
        }
    }

    public void CraftWoodPickaxe()
    {
        InventoryUIManager inventory = InventoryUIManager.Instance;
        ItemSlotManager planksSlot = null;
        ItemSlotManager sticksSlot = null;
        bool hasPlanks = false;
        bool hasSticks = false;

        Item woodPickaxe = null;

        for (int i = 0; i < itemsToGive.Length; i++)
        {
            if (itemsToGive[i].itemId == "009")
            {
                woodPickaxe = itemsToGive[i];
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
            woodPickaxe.currentDurability = woodPickaxe.maxDurability;
            planksSlot.DeductItem(3);
            sticksSlot.DeductItem(2);
            inventory.AddItem(woodPickaxe, 1);
        }
        else
        {
            return;
        }
    }

    public void CraftWoodSword()
    {
        InventoryUIManager inventory = InventoryUIManager.Instance;
        ItemSlotManager planksSlot = null;
        ItemSlotManager sticksSlot = null;
        bool hasPlanks = false;
        bool hasSticks = false;

        Item woodSword = null;

        for (int i = 0; i < itemsToGive.Length; i++)
        {
            if (itemsToGive[i].itemId == "010")
            {
                woodSword = itemsToGive[i];
                break;
            }
        }

        for (int i = 0; i < inventory.itemSlot.Length; i++)
        {
            if (inventory.itemSlot[i].item != null && inventory.itemSlot[i].item.itemId == "002") //ID of planks
            {
                planksSlot = inventory.itemSlot[i];
                int remaining = planksSlot.quantity - 2;
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
            woodSword.currentDurability = woodSword.maxDurability;
            planksSlot.DeductItem(3);
            sticksSlot.DeductItem(2);
            inventory.AddItem(woodSword, 1);
        }
        else
        {
            return;
        }
    }

    public void CraftStoneAxe()
    {
        InventoryUIManager inventory = InventoryUIManager.Instance;
        ItemSlotManager cobbledSlot = null;
        ItemSlotManager sticksSlot = null;
        bool hasCobble = false;
        bool hasSticks = false;

        Item stoneAxe = null;

        for (int i = 0; i < itemsToGive.Length; i++)
        {
            if (itemsToGive[i].itemId == "012")
            {
                stoneAxe = itemsToGive[i];
                break;
            }
        }

        for (int i = 0; i < inventory.itemSlot.Length; i++)
        {
            if (inventory.itemSlot[i].item != null && inventory.itemSlot[i].item.itemId == "011")
            {
                cobbledSlot = inventory.itemSlot[i];
                int remaining = cobbledSlot.quantity - 4;
                if (remaining >= 0)
                {
                    hasCobble = true;
                    break;
                }
                else
                {
                    hasCobble = false;
                    break;
                }
            }
        }
        for (int i = 0; i < inventory.itemSlot.Length; i++)
        {
            if (inventory.itemSlot[i].item != null && inventory.itemSlot[i].item.itemId == "003")
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

        if (hasCobble == true && hasSticks == true)
        {
            stoneAxe.currentDurability = stoneAxe.maxDurability;
            cobbledSlot.DeductItem(4);
            sticksSlot.DeductItem(2);
            inventory.AddItem(stoneAxe, 1);
        }
        else
        {
            return;
        }
    }
    public void CraftStonePickaxe()
    {
        InventoryUIManager inventory = InventoryUIManager.Instance;
        ItemSlotManager cobbledSlot = null;
        ItemSlotManager sticksSlot = null;
        bool hasCobble = false;
        bool hasSticks = false;

        Item stonePickaxe = null;

        for (int i = 0; i < itemsToGive.Length; i++)
        {
            if (itemsToGive[i].itemId == "013")
            {
                stonePickaxe = itemsToGive[i];
                break;
            }
        }

        for (int i = 0; i < inventory.itemSlot.Length; i++)
        {
            if (inventory.itemSlot[i].item != null && inventory.itemSlot[i].item.itemId == "011")
            {
                cobbledSlot = inventory.itemSlot[i];
                int remaining = cobbledSlot.quantity - 3;
                if (remaining >= 0)
                {
                    hasCobble = true;
                    break;
                }
                else
                {
                    hasCobble = false;
                    break;
                }
            }
        }
        for (int i = 0; i < inventory.itemSlot.Length; i++)
        {
            if (inventory.itemSlot[i].item != null && inventory.itemSlot[i].item.itemId == "003")
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

        if (hasCobble == true && hasSticks == true)
        {
            stonePickaxe.currentDurability = stonePickaxe.maxDurability;
            cobbledSlot.DeductItem(3);
            sticksSlot.DeductItem(2);
            inventory.AddItem(stonePickaxe, 1);
        }
        else
        {
            return;
        }

    }
    public void CraftStoneSword()
    {
        InventoryUIManager inventory = InventoryUIManager.Instance;
        ItemSlotManager cobbledSlot = null;
        ItemSlotManager sticksSlot = null;
        bool hasCobble = false;
        bool hasSticks = false;

        Item stoneSword = null;

        for (int i = 0; i < itemsToGive.Length; i++)
        {
            if (itemsToGive[i].itemId == "014")
            {
                stoneSword = itemsToGive[i];
                break;
            }
        }

        for (int i = 0; i < inventory.itemSlot.Length; i++)
        {
            if (inventory.itemSlot[i].item != null && inventory.itemSlot[i].item.itemId == "011")
            {
                cobbledSlot = inventory.itemSlot[i];
                int remaining = cobbledSlot.quantity - 2;
                if (remaining >= 0)
                {
                    hasCobble = true;
                    break;
                }
                else
                {
                    hasCobble = false;
                    break;
                }
            }
        }
        for (int i = 0; i < inventory.itemSlot.Length; i++)
        {
            if (inventory.itemSlot[i].item != null && inventory.itemSlot[i].item.itemId == "003")
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

        if (hasCobble == true && hasSticks == true)
        {
            stoneSword.currentDurability = stoneSword.maxDurability;
            cobbledSlot.DeductItem(2);
            sticksSlot.DeductItem(2);
            inventory.AddItem(stoneSword, 1);
        }
        else
        {
            return;
        }

    }
}

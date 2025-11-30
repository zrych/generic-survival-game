using UnityEngine;

public class SwapButtonUI : MonoBehaviour
{
    public void SwapWithFirst()
    {
        for (int i = 0; i < 15; i++)
        {
            if (InventoryUIManager.Instance.itemSlot[i].isSlotSelected == true)
            {
                ItemSlotManager selectedSlot = InventoryUIManager.Instance.itemSlot[i];
                ItemSlotManager firstSlot = InventoryUIManager.Instance.itemSlot[0];

                if (selectedSlot == firstSlot)
                {
                    return;
                }

                if (firstSlot.isEmpty)
                {
                    firstSlot.StoreItem(selectedSlot.item, selectedSlot.quantity);
                    selectedSlot.EmptySlot();  
                } else
                {
                    Item temp = selectedSlot.item;
                    int tempQ = selectedSlot.quantity;
                    selectedSlot.EmptySlot();
                    selectedSlot.StoreItem(firstSlot.item, firstSlot.quantity);
                    firstSlot.EmptySlot();
                    firstSlot.StoreItem(temp, tempQ);
                }
                selectedSlot.SelectSlot();
            }
        }
    }

    public void SwapWithSecond()
    {
        for (int i = 0; i < 15; i++)
        {
            if (InventoryUIManager.Instance.itemSlot[i].isSlotSelected == true)
            {
                ItemSlotManager selectedSlot = InventoryUIManager.Instance.itemSlot[i];
                ItemSlotManager secondSlot = InventoryUIManager.Instance.itemSlot[1];

                if (selectedSlot == secondSlot)
                {
                    return;
                }

                if (secondSlot.isEmpty)
                {
                    secondSlot.StoreItem(selectedSlot.item, selectedSlot.quantity);
                    selectedSlot.EmptySlot();
                }
                else
                {
                    Item temp = selectedSlot.item;
                    int tempQ = selectedSlot.quantity;
                    selectedSlot.EmptySlot();
                    selectedSlot.StoreItem(secondSlot.item, secondSlot.quantity);
                    secondSlot.EmptySlot();
                    secondSlot.StoreItem(temp, tempQ);          
                }
                selectedSlot.SelectSlot();
            }
        }
    }

    public void SwapWithThird()
    {
        for (int i = 0; i < 15; i++)
        {
            if (InventoryUIManager.Instance.itemSlot[i].isSlotSelected == true)
            {
                ItemSlotManager selectedSlot = InventoryUIManager.Instance.itemSlot[i];
                ItemSlotManager thirdSlot = InventoryUIManager.Instance.itemSlot[2];

                if (selectedSlot == thirdSlot)
                {
                    return;
                }

                if (thirdSlot.isEmpty)
                {
                    thirdSlot.StoreItem(selectedSlot.item, selectedSlot.quantity);
                    selectedSlot.EmptySlot();
                }
                else
                {
                    Item temp = selectedSlot.item;
                    int tempQ = selectedSlot.quantity;
                    selectedSlot.EmptySlot();
                    selectedSlot.StoreItem(thirdSlot.item, thirdSlot.quantity);
                    thirdSlot.EmptySlot();
                    thirdSlot.StoreItem(temp, tempQ);
                }
                selectedSlot.SelectSlot();
            }
        }
    }

}

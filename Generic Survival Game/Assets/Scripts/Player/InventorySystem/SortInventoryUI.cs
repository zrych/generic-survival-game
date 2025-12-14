using UnityEngine;

public class SortInventoryUI : MonoBehaviour
{
    public void SortInventory()
    {
        ItemSlotManager[] slots = InventoryUIManager.Instance.itemSlot;

        for (int i = 1; i < slots.Length; i++)
        {
            int j = i;

            while (j > 0 &&
                   !slots[j].isEmpty &&
                   !slots[j - 1].isEmpty &&
                   slots[j - 1].quantity > slots[j].quantity)
            {
                slots[j - 1].SwapWith(slots[j]);
                j--;
            }
        }
    }
}

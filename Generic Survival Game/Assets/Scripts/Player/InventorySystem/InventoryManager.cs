using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public Item[] items = new Item[20];
    public HotbarUI hotbarUI;

    public bool AddItem(Item newItem)
    {
        for (int i = 0; i < items.Length; i++)
        {
            if (items[i] == null)
            {
                items[i] = newItem;
                hotbarUI.RefreshHotbar();
                return true;
            }
        }
        return false;
    }
}

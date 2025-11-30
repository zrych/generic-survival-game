using UnityEngine;

public class ItemObject : MonoBehaviour
{
    [SerializeField] private Item item; //contains the item name, desc, etc.
    [HideInInspector] public int quantity;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            int leftOverItems = InventoryUIManager.Instance.AddItem(item, quantity);
            if (leftOverItems <= 0) Destroy(gameObject);
            else quantity = leftOverItems;
        }
    }
}

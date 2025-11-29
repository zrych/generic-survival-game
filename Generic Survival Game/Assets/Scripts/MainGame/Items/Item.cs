using UnityEngine;

[CreateAssetMenu(fileName = "Item", menuName = "Scriptable Objects/Item")]
public class Item : ScriptableObject
{
    public string itemId;
    public string itemName;
    public Sprite itemIcon;
    public string itemDescription;
}

using UnityEngine;

[CreateAssetMenu(fileName = "Item", menuName = "Scriptable Objects/Item")]
public class Item : ScriptableObject
{
    [Header("General Info")]
    public string itemId;
    public string itemName;
    public Sprite itemIcon;
    [TextArea(3, 10)]
    public string itemDescription;

    [Header("Tool Info")]
    public bool isTool;
    public float resourceDamage;
    public float enemyDamage;
    public int durability;

    [Header("Consumable Info")]
    public bool isConsumable;
    public float hpRestore;
    public float hungerRestore;
}

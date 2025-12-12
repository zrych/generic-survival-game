using Unity.VisualScripting;
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

    public ToolType toolType;
    public int toolLevel; //0 - hand, 1 - wood, 2 - stone, 3 - iron

    public float resourceDamage;
    public float enemyDamage;
    public int maxDurability;
    public int currentDurability = 0;

    [Header("Consumable Info")]
    public bool isConsumable;
    public float hpRestore;
    public float hungerRestore;
    public float saturationRestore;

    [Header("Fuel Info")]
    public bool isFuel;
    public float fuelRestore;
}


public enum ToolType
{
    None,
    Hand,
    Axe,
    Pickaxe,
    Sword
}

using UnityEngine;

public abstract class ResourceNode : MonoBehaviour, IDamageable
{
    [SerializeField] private float hp;
    [SerializeField] private GameObject[] itemYields; //item prefab goes here
    [SerializeField] private int[] minYield;
    [SerializeField] private int[] maxYield;
    private float currentHP;

    private ItemObject[] itemDrops;

    [SerializeField] private ToolType[] requiredTools;
    [SerializeField] private int[] requiredToolLevels;

    protected virtual void Start()
    {
        itemDrops = new ItemObject[itemYields.Length];
        currentHP = hp;
        for (int i = 0; i < itemYields.Length; i++)
        {
            itemDrops[i] = itemYields[i].GetComponent<ItemObject>();
        }
    }

    public void TakeDamage(float amount)
    {
        Debug.Log($"-{amount} hp!");
        currentHP -= amount;
        if (currentHP <= 0)
        {
            BreakNode();
        }
    }
    public bool CanBeDamagedBy(Item tool)
    {
        if (tool == null)
        {
            foreach (ToolType t in requiredTools)
            {
                if (t == ToolType.Hand) return true;
            }
            return false;
        }
        bool hasTool = false;
        bool hasLevel = false;
        foreach (ToolType t in requiredTools)
        {
            if (tool.toolType == t)
            {
                hasTool = true;
                break;
            }
        }
        foreach (int level in requiredToolLevels)
        {
            if (tool.toolLevel >= level)
            {
                hasLevel = true;
                break;
            }
        }
        if (hasTool == true && hasLevel == true) return true;
        return false;
    }
    public bool TryHit(Item tool)
    {
        if (!CanBeDamagedBy(tool)) return false;
        if (tool == null) TakeDamage(1);
        else TakeDamage(tool.resourceDamage);
        return true;
    }

    public void BreakNode()
    {
        

        for (int i = 0; i < itemDrops.Length; i++)
        {
            int yieldQuantity = Random.Range(minYield[i], maxYield[i] + 1);
            itemDrops[i].quantity = yieldQuantity;
            Vector2 scatterOffset = new Vector2(
                Random.Range(-0.7f, 0.7f), // adjust X spread
                Random.Range(-0.2f, 0.2f)  // adjust Y spread (less vertical scatter)
            );
            Instantiate(itemYields[i], (Vector2)transform.position + scatterOffset, Quaternion.identity);
        }
        Destroy(gameObject);
    }
}

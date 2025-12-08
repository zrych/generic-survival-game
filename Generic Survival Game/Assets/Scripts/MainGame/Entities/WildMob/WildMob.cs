using UnityEngine;
using UnityEngine.UI;

public class WildMob : MonoBehaviour, IDamageable
{
    [SerializeField] protected float maxHp;
    [SerializeField] protected float moveSpeed = 1.5f;
    protected Rigidbody2D rb;
    protected Animator animator;

    [SerializeField] protected GameObject[] itemYields; //item prefab goes here
    [SerializeField] protected int[] minYield;
    [SerializeField] protected int[] maxYield;
    private float currentHp;

    private ItemObject[] itemDrops;

    [SerializeField] private ToolType[] requiredTools;
    [SerializeField] private int[] requiredToolLevels;

    [SerializeField] private EnemyHPBar hpBar;
    protected virtual void Start()
    {
        itemDrops = new ItemObject[itemYields.Length];

        currentHp = maxHp;
        hpBar.SetHealth(currentHp, maxHp);

        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        for (int i = 0; i < itemYields.Length; i++)
        {
            itemDrops[i] = itemYields[i].GetComponent<ItemObject>();
        }

    }

    public void TakeDamage(float amount)
    {
        Debug.Log($"-{amount} hp!");
        currentHp -= amount;
        hpBar.SetHealth(currentHp, maxHp);
        if (currentHp <= 0)
        {
            KillSelf();
        }
    }

    public void KillSelf()
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

    public bool TryHit(Item tool)
    {
        if (!CanBeDamagedBy(tool)) return false;
        if (tool == null) TakeDamage(1);
        else TakeDamage(tool.enemyDamage);
        return true;
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
        foreach (ToolType t in requiredTools)
        {
            if (tool.toolType == t) return true;
        }
        foreach (int level in requiredToolLevels)
        {
            if (tool.toolLevel >= level) return true;
        }
        return false;
    }
}

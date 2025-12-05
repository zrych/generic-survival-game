using UnityEngine;

public class WildMob : MonoBehaviour, IDamageable
{
    [SerializeField] protected float maxHp;
    [SerializeField] protected float moveSpeed = 1.5f;
    protected Rigidbody2D rb;
    protected Animator animator;

    [SerializeField] protected GameObject[] itemYields; //item prefab goes here
    [SerializeField] protected int[] minYield;
    [SerializeField] protected int[] maxYield;
    private float currentHP;

    private ItemObject[] itemDrops;

    protected virtual void Start()
    {
        itemDrops = new ItemObject[itemYields.Length];
        currentHP = maxHp;
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
        currentHP -= amount;
        if (currentHP <= 0)
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
}

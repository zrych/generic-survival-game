using UnityEngine;

public abstract class ResourceNode : MonoBehaviour, IDamageable
{
    [SerializeField] private float hp;
    [SerializeField] private GameObject itemYield; //item prefab goes here
    [SerializeField] private int minYield;
    [SerializeField] private int maxYield;
    private float currentHP;

    private ItemObject itemDrop;

    protected virtual void Start()
    {
        currentHP = hp;
        itemDrop = itemYield.GetComponent<ItemObject>();
    }

    public void TakeDamage(float amount)
    {
        Debug.Log($"-{amount} hp!");
        hp -= amount;
        if (hp <= 0)
        {
            BreakNode();
        }
    }

    public void BreakNode()
    {
        int yieldQuantity = Random.Range(minYield, maxYield + 1);
        itemDrop.quantity = yieldQuantity;

        Instantiate(itemYield, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}

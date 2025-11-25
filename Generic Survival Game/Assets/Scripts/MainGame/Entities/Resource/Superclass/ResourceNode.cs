using UnityEngine;

public abstract class ResourceNode : MonoBehaviour, IDamageable
{
    [SerializeField] private float hp;
    [SerializeField] private Item itemYield;
    [SerializeField] private int minYield;
    [SerializeField] private int maxYield;

    private float currentHP;

    public float GetHP() => hp;
    public void SetHP(float value) => hp = value;
    public float GetCurrentHP() => currentHP;
    protected void setCurrentHP(float value) => currentHP = value;
    public Item GetYieldItem() => itemYield;
    public void SetYieldItem(Item item) => itemYield = item;

    public int GetMinYield() => minYield;
    public void SetMinYield(int value) => minYield = value;
    public int GetMaxYield() => maxYield;
    public void SetMaxYield(int value) => maxYield = value;

    protected virtual void Start()
    {
        currentHP = hp;
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

    protected abstract void BreakNode();
}

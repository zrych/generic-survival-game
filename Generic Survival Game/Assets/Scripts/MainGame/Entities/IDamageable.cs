using UnityEngine;

public interface IDamageable
{
    void TakeDamage(float amount);

    bool TryHit(Item tool);

    bool CanBeDamagedBy(Item tool);
}

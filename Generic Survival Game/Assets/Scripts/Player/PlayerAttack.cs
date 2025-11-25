using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public float baseDamage = 0.5f;
    public float attackRange = 1f;
    public LayerMask hittableMask;

    [SerializeField] private Animator armAnim;


    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            armAnim.SetBool("IsAttacking", true);
            Attack();
        } else
        {
            armAnim.SetBool("IsAttacking", false);
        }
    }

    //void Attack()
    //{

    //    RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.right, attackRange, hittableMask);

    //    if (hit.collider != null)
    //    {
    //        IDamageable damageable = hit.collider.GetComponentInParent<IDamageable>();
    //        if (damageable != null)
    //        {
    //            damageable.TakeDamage(baseDamage);
    //        }
    //    }
    //}

    void Attack()
    {
        Vector2 playerPos = transform.position;
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        // Direction from player to mouse pointer (crosshair)
        Vector2 attackDirection = (mousePos - playerPos).normalized;

        // Get all colliders within attackRange on hittableMask
        Collider2D[] hits = Physics2D.OverlapCircleAll(playerPos, attackRange, hittableMask);

        foreach (Collider2D hitCollider in hits)
        {
            Vector2 targetPos = hitCollider.transform.position;
            Vector2 directionToTarget = (targetPos - playerPos).normalized;

            float angle = Vector2.Angle(attackDirection, directionToTarget);

            if (angle <= 22.5f) // half of 45 degrees
            {
                IDamageable damageable = hitCollider.GetComponentInParent<IDamageable>();
                if (damageable != null)
                {
                    damageable.TakeDamage(baseDamage);
                }
            }
        }
    }
}

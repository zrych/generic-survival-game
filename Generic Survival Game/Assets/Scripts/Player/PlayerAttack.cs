using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public float baseDamage = 0.5f;
    public float attackRange = 1f;
    public LayerMask hittableMask;

    [SerializeField] private Animator armAnim;
    [SerializeField] private Transform armTransform;
    [SerializeField] private CursorManager cursorManager;

    void Update()
    {
        Vector2 playerPos = transform.position;
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        // Flip only the Arm, not the whole player
        if (mousePos.x < playerPos.x)
        {
            armTransform.localScale = new Vector3(-1, 1, 1); // face left
        }
        else
        {
            armTransform.localScale = new Vector3(1, 1, 1); // face right
        }

        if (Input.GetMouseButtonDown(0) && cursorManager.isUiOpen == false)
        {
            armAnim.SetBool("IsAttacking", true);
            Attack();
        } else
        {
            armAnim.SetBool("IsAttacking", false);
        }
    }


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

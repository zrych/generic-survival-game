using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public float baseDamage = 0.5f;
    public float attackRange = 1f;
    public LayerMask hittableMask;

    [SerializeField] private Animator armAnim;
    [SerializeField] private Transform armTransform;
    [SerializeField] private CursorManager cursorManager;
    [SerializeField] private float attackCooldown = 0.5f;

    public Item heldTool;
    public Item heldWeapon;
    public bool isHoldingTool = false;
    private bool isAttacking;

    public static PlayerAttack Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void ToolIdleAnimation(bool toggle)
    {
        armAnim.SetBool("IsHoldingAxeWood", toggle);
    }

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

        ToolIdleAnimation(isHoldingTool);

        if (Input.GetMouseButtonDown(0) && cursorManager.isUiOpen == false)
        {
            if (heldTool != null)
            {
                if (heldTool.itemName == "Wooden Axe")
                {
                    armAnim.SetTrigger("AttackAxeWood");
                    Attack(heldTool.resourceDamage, true);
                    Invoke(nameof(ResetAttack), attackCooldown);
                }
            }
            else
            {
                armAnim.SetTrigger("Attack");
                Attack(baseDamage, false);
                Invoke(nameof(ResetAttack), attackCooldown);
            }
        }
    }

    private void ResetAttack()
    {
        isAttacking = false;
    }

    void Attack(float damage, bool isTool)
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
                    damageable.TakeDamage(damage);
                }
            }
        }
    }
}

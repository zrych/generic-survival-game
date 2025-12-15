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
    public bool isHoldingTool = false;
    private float nextAttackTime;

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
        if (heldTool != null)
        {
            if (heldTool.itemName == "Wooden Axe")
            {
                armAnim.SetBool("IsHoldingAxeWood", toggle);
                armAnim.SetBool("IsHoldingPickWood", !toggle);
                armAnim.SetBool("IsHoldingSwordWood", !toggle);
                armAnim.SetBool("IsHoldingAxeStone", !toggle);
                armAnim.SetBool("IsHoldingPickStone", !toggle);
                armAnim.SetBool("IsHoldingSwordStone", !toggle);
            }
            else if (heldTool.itemName == "Wooden Pickaxe")
            {
                armAnim.SetBool("IsHoldingPickWood", toggle);
                armAnim.SetBool("IsHoldingAxeWood", !toggle);
                armAnim.SetBool("IsHoldingSwordWood", !toggle);
                armAnim.SetBool("IsHoldingAxeStone", !toggle);
                armAnim.SetBool("IsHoldingPickStone", !toggle);
                armAnim.SetBool("IsHoldingSwordStone", !toggle);
            }
            else if (heldTool.itemName == "Wooden Sword")
            {
                armAnim.SetBool("IsHoldingSwordWood", toggle);
                armAnim.SetBool("IsHoldingAxeWood", !toggle);
                armAnim.SetBool("IsHoldingPickWood", !toggle);
                armAnim.SetBool("IsHoldingAxeStone", !toggle);
                armAnim.SetBool("IsHoldingPickStone", !toggle);
                armAnim.SetBool("IsHoldingSwordStone", !toggle);
            }
            else if (heldTool.itemName == "Stone Axe")
            {
                armAnim.SetBool("IsHoldingAxeStone", toggle);
                armAnim.SetBool("IsHoldingAxeWood", !toggle);
                armAnim.SetBool("IsHoldingPickWood", !toggle);
                armAnim.SetBool("IsHoldingSwordWood", !toggle);
                armAnim.SetBool("IsHoldingPickStone", !toggle);
                armAnim.SetBool("IsHoldingSwordStone", !toggle);
            }
            else if (heldTool.itemName == "Stone Pickaxe")
            {
                armAnim.SetBool("IsHoldingPickStone", toggle);
                armAnim.SetBool("IsHoldingAxeWood", !toggle);
                armAnim.SetBool("IsHoldingPickWood", !toggle);
                armAnim.SetBool("IsHoldingSwordWood", !toggle);
                armAnim.SetBool("IsHoldingAxeStone", !toggle);
                armAnim.SetBool("IsHoldingSwordStone", !toggle);
            }
            else if (heldTool.itemName == "Stone Sword")
            {
                armAnim.SetBool("IsHoldingSwordStone", toggle);
                armAnim.SetBool("IsHoldingAxeWood", !toggle);
                armAnim.SetBool("IsHoldingPickWood", !toggle);
                armAnim.SetBool("IsHoldingSwordWood", !toggle);
                armAnim.SetBool("IsHoldingAxeStone", !toggle);
                armAnim.SetBool("IsHoldingPickStone", !toggle);
            }
        }
        else
        {
            armAnim.SetBool("IsHoldingAxeWood", toggle);
            armAnim.SetBool("IsHoldingPickWood", toggle);
            armAnim.SetBool("IsHoldingSwordWood", toggle);
            armAnim.SetBool("IsHoldingAxeStone", toggle);
            armAnim.SetBool("IsHoldingPickStone", toggle);
            armAnim.SetBool("IsHoldingSwordStone", toggle);
        }
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

        if (Input.GetMouseButtonDown(0) && Time.time >= nextAttackTime && cursorManager.isUiOpen == false)
        {
            nextAttackTime = Time.time + attackCooldown;
            if (heldTool != null)
            {
                if (heldTool.itemName == "Wooden Axe")
                {
                    armAnim.SetTrigger("AttackAxeWood");
                    Attack(heldTool.resourceDamage, true);
                }
                else if (heldTool.itemName == "Wooden Pickaxe")
                {
                    armAnim.SetTrigger("AttackPickWood");
                    Attack(heldTool.resourceDamage, true);
                }
                else if (heldTool.itemName == "Wooden Sword")
                {
                    armAnim.SetTrigger("AttackSwordWood");
                    Attack(heldTool.enemyDamage, true);
                }
                else if (heldTool.itemName == "Stone Axe")
                {
                    armAnim.SetTrigger("AttackAxeStone");
                    Attack(heldTool.enemyDamage, true);
                }
                else if (heldTool.itemName == "Stone Pickaxe")
                {
                    armAnim.SetTrigger("AttackPickStone");
                    Attack(heldTool.enemyDamage, true);
                }
                else if (heldTool.itemName == "Stone Sword")
                {
                    armAnim.SetTrigger("AttackSwordStone");
                    Attack(heldTool.enemyDamage, true);
                }
            }
            else
            {
                armAnim.SetTrigger("Attack");
                Attack(baseDamage, false);
            }
        }
    }

    private void UseDurability()
    {
        if (!isHoldingTool) return;
        heldTool.currentDurability -= 1;
        if (heldTool.currentDurability <= 0)
        {
            BreakTool();
            isHoldingTool = false;
            heldTool = null;
        }
    }

    private void BreakTool()
    {
        InventoryUIManager.Instance.DeleteItem(heldTool);
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
                if (damageable == null)
                {
                    damageable = hitCollider.GetComponent<IDamageable>();
                }

                if (damageable != null)
                {
                    UseDurability();
                    if (damageable.TryHit(heldTool))
                    {
                        // Play hit sound for player hitting
                        if (SoundManager.Instance != null)
                            SoundManager.Instance.PlaySound2D("Player");

                        Chicken chicken;
                        Boar boar;
                        if (chicken = hitCollider.GetComponent<Chicken>())
                        {
                            chicken.OnHit(transform.position);
                            // Play random Chicken hit sound (2 clips)
                            if (SoundManager.Instance != null)
                                SoundManager.Instance.PlaySound2D("ChickenHit");
                        }
                        if (boar = hitCollider.GetComponent<Boar>())
                        {
                            boar.OnHit(transform.position);
                            // Play Boar hit sound
                            if (SoundManager.Instance != null)
                                SoundManager.Instance.PlaySound2D("BoarHit");
                        }
                    }
                }
            }
        }
    }
}

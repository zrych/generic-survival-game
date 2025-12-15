using UnityEngine;

public class Monster : MonoBehaviour, IDamageable
{
    [Header("Stats")]
    [SerializeField] protected float maxHp;
    [SerializeField] protected float moveSpeed = 1.5f;
    [SerializeField] protected float detectionRange = 6f;
    [Header("Attack")]
    [SerializeField] protected float attackRange = 0.7f;
    [SerializeField] protected float attackDamage = 0.5f;
    [SerializeField] protected float attackCooldown = 1.0f;
    protected Rigidbody2D rb;
    protected Animator animator;
    private float currentHp;

    [SerializeField] private ToolType[] requiredTools;
    [SerializeField] private int[] requiredToolLevels;

    [SerializeField] private EnemyHPBar hpBar;

    // 🧟 AudioSource for hit/death sounds
    private AudioSource audioSource;

    protected virtual void Start()
    {
        currentHp = maxHp;
        hpBar.SetHealth(currentHp, maxHp);

        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        // Setup audio source
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.spatialBlend = 1f; // 3D sound
        audioSource.rolloffMode = AudioRolloffMode.Linear;
        audioSource.minDistance = 1f;
        audioSource.maxDistance = 6f; // adjust as needed
        audioSource.playOnAwake = false;
    }

    public void KillSelf()
    {
        // Play death sound before destroying
        if (SoundManager.Instance != null)
        {
            AudioClip clip = SoundManager.Instance.GetClip("ZombieDead");
            if (clip != null)
            {
                audioSource.PlayOneShot(clip);
            }
        }

        Destroy(gameObject, 0.1f); // short delay to allow sound
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

    public void TakeDamage(float amount)
    {
        currentHp -= amount;
        hpBar.SetHealth(currentHp, maxHp);

        // Play hit sound
        if (SoundManager.Instance != null)
        {
            AudioClip hitClip = SoundManager.Instance.GetClip("ZombieHit");
            if (hitClip != null)
                audioSource.PlayOneShot(hitClip);
        }

        if (currentHp <= 0)
        {
            KillSelf();
        }
    }

    public bool TryHit(Item tool)
    {
        if (!CanBeDamagedBy(tool)) return false;
        if (tool == null) TakeDamage(1);
        else TakeDamage(tool.enemyDamage);
        return true;
    }
}

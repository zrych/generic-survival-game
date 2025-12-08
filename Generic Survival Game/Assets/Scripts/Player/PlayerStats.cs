using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    [Header("Health")]
    public float maxHP = 20f;
    public float currentHP; //placeholder

    [Header("Hunger")]
    public float maxHunger = 20f;
    public float currentHunger; //placeholder
    public float hungerDrainIdle = 0.066f;   // walking/standing
    public float hungerDrainSprint = 0.1f;  // sprinting rate
    public float starvationDamage = 0.5f;   // HP lost per second when hunger = 0

    [Header("Saturation")]
    public float maxSaturation = 20f;
    public float currentSaturation;

    [Header("Regen")]
    private float regenTimer = 0f;
    [SerializeField] private float regenInterval = 2f;
    [SerializeField] private float regenAmount = 0.5f;

    private PlayerMovement movement;
    public static PlayerStats Instance { get; private set; }

    void Start()
    {
        currentHP = maxHP;
        currentHunger = maxHunger;
        currentSaturation = maxSaturation;

        movement = GetComponent<PlayerMovement>();
    }
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
    void Update()
    {
        HandleHunger();
        HandleStarvation();
        HandleRegen();
    }

    private void HandleRegen()
    {
        if (currentHP >= maxHP) return;
        if (currentSaturation > 0 && currentHunger > 0)
        {
            regenTimer += Time.deltaTime;
            if (regenTimer >= regenInterval)
            {
                currentHP += regenAmount;
                currentSaturation -= regenAmount;
                regenTimer = 0f;
                currentHP = Mathf.Clamp(currentHP, 0, maxHP);
                currentSaturation = Mathf.Clamp(currentSaturation, 0, maxSaturation);
            }
        } else if (currentSaturation <= 0 && currentHunger > 0)
        {
            regenTimer += Time.deltaTime;
            if (regenTimer >= regenInterval * 2)
            {
                currentHP += regenAmount;
                currentHunger -= regenAmount;
                regenTimer = 0f;
                currentHP = Mathf.Clamp(currentHP, 0, maxHP);
                currentHunger = Mathf.Clamp(currentHunger, 0, maxHunger);
            }
        }
    }

    void HandleHunger()
    {
        if (movement.IsSprinting)
        {
            if (currentSaturation > 0)
            {
                currentSaturation -= hungerDrainSprint * Time.deltaTime;
            }
            else
            {
                currentHunger -= hungerDrainSprint * Time.deltaTime;
            }
        }
        else
        {
            if (currentSaturation > 0)
            {
                currentSaturation -= hungerDrainIdle * Time.deltaTime;
            }
            else
            {
                currentHunger -= hungerDrainIdle * Time.deltaTime;
            }
            currentHunger = Mathf.Clamp(currentHunger, 0, maxHunger);
        }
    }

    public void ConsumeItem(float hungerGain, float hpGain, float saturationGain)
    {
        RestoreHunger(hungerGain);
        RestoreHP(hpGain);
        RestoreSaturation(saturationGain);
    }

    private void RestoreHunger(float amount)
    {
        currentHunger += amount;
        currentHunger = Mathf.Clamp(currentHunger, 0, maxHunger);
    }

    private void RestoreHP(float amount)
    {
        currentHP += amount;
        currentHP = Mathf.Clamp(currentHP, 0, maxHP);
    }

    private void RestoreSaturation(float amount)
    {
        currentSaturation += amount;
        currentSaturation = Mathf.Clamp(currentSaturation, 0, maxSaturation);
    }

    public void TakeDamage(float damage)
    {
        currentHP -= damage;
        currentHP = Mathf.Clamp(currentHP, 0, maxHP);
    }

    void HandleStarvation()
    {
        if (currentHunger <= 0)
        {
            currentHP -= starvationDamage * Time.deltaTime;
            currentHP = Mathf.Clamp(currentHP, 0, maxHP);
        }
    }
}

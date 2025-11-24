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

    private PlayerMovement movement;

    void Start()
    {
        currentHP = maxHP;
        currentHunger = maxHunger;

        movement = GetComponent<PlayerMovement>();
    }

    void Update()
    {
        HandleHunger();
        HandleStarvation();
    }

    void HandleHunger()
    {
        if (movement.IsSprinting)
            currentHunger -= hungerDrainSprint * Time.deltaTime;
        else
            currentHunger -= hungerDrainIdle * Time.deltaTime;

        currentHunger = Mathf.Clamp(currentHunger, 0, maxHunger);
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

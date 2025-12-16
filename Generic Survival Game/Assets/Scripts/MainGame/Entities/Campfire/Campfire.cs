using UnityEngine;

public class Campfire : MonoBehaviour
{
    [SerializeField] private GameObject fkey;
    private CampfireFuelBar fuelBar;
    [SerializeField] private GameObject unlitCampfire;
    [SerializeField] private GameObject litCampfire;
    private bool isWithinRange = false;
    public bool isLit;
    [SerializeField] private MonsterSpawnZone waveSpawnZone;

    public float fireRadius = 3f;

    public static Campfire Instance;

    [Header("Fuel Settings")]
    public float maxFuel = 40f;
    public float currentFuel;
    public float burnRate = 0.5f;

    [Header("Cooked Food to Give")]
    [SerializeField] private Item[] foods;

    [Header("Fire Sound")]
    [SerializeField] private string fireSoundID = "Campfire";
    [SerializeField] private float maxHearingDistance = 6f;

    private AudioSource fireAudioSource;
    private Transform playerTransform;

    private void Awake()
    {
        if (Instance == null) Instance = this;
    }

    private void Start()
    {
        currentFuel = maxFuel;
        fuelBar = GetComponentInChildren<CampfireFuelBar>();
        isLit = true;
        UpdateUI();

        playerTransform = GameObject.FindWithTag("Player")?.transform;

        // 🔊 Setup looping fire AudioSource
        fireAudioSource = gameObject.AddComponent<AudioSource>();
        fireAudioSource.clip = SoundManager.Instance.GetClip(fireSoundID);
        fireAudioSource.loop = true;
        fireAudioSource.playOnAwake = false;
        fireAudioSource.spatialBlend = 0f; // 2D with manual fade
        fireAudioSource.volume = 0f;
    }

    private void Update()
    {
        if (currentFuel > 0)
        {
            waveSpawnZone.gameObject.SetActive(false);
            LitCampfire(true);

            currentFuel -= burnRate * Time.deltaTime;
            if (currentFuel < 0) currentFuel = 0;

            UpdateUI();
        }
        else
        {
            MonsterSpawnManager spawnManager = MonsterSpawnManager.Instance;
            waveSpawnZone.gameObject.SetActive(true);

            DayNightController daynightController = DayNightController.Instance;
            if (daynightController.currentState == DayNightController.TimeState.Night)
            {
                spawnManager.NightStarted();
            }

            LitCampfire(false);
        }

        HandleFireSound();

        if (isWithinRange && Input.GetKeyDown(KeyCode.F))
        {
            InventoryUIManager inventory = InventoryUIManager.Instance;
            for (int i = 0; i < inventory.itemSlot.Length; i++)
            {
                if (!inventory.itemSlot[i].isSlotSelected) continue;
                if (inventory.itemSlot[i].item == null) return;

                if (inventory.itemSlot[i].item.isConsumable && currentFuel > 0)
                {
                    GiveCookedFood(inventory.itemSlot[i]);
                }
                else if (inventory.itemSlot[i].item.isFuel)
                {
                    AddFuel(inventory.itemSlot[i].item.fuelRestore);
                    inventory.itemSlot[i].DeductItem(1);
                }
            }
        }
    }

    // 🔥 SAME LOGIC STYLE AS Chicken.cs
    private void HandleFireSound()
    {
        if (fireAudioSource == null || playerTransform == null) return;

        if (!isLit || currentFuel <= 0f)
        {
            fireAudioSource.volume = Mathf.Lerp(fireAudioSource.volume, 0f, Time.deltaTime * 4f);
            if (fireAudioSource.volume <= 0.01f && fireAudioSource.isPlaying)
                fireAudioSource.Stop();
            return;
        }

        float distance = Vector2.Distance(transform.position, playerTransform.position);

        if (distance > maxHearingDistance)
        {
            fireAudioSource.volume = Mathf.Lerp(fireAudioSource.volume, 0f, Time.deltaTime * 4f);
            if (fireAudioSource.volume <= 0.01f && fireAudioSource.isPlaying)
                fireAudioSource.Stop();
            return;
        }

        if (!fireAudioSource.isPlaying)
            fireAudioSource.Play();

        float distanceVolume = 1f - (distance / maxHearingDistance);
        float fuelVolume = Mathf.Clamp01(currentFuel / maxFuel);

        float targetVolume = distanceVolume * fuelVolume;
        fireAudioSource.volume = Mathf.Lerp(fireAudioSource.volume, targetVolume, Time.deltaTime * 4f);
    }

    private void GiveCookedFood(ItemSlotManager itemSlot)
    {
        Item cookedFood = null;
        string foodOutcome = null;

        switch (itemSlot.itemName)
        {
            case "Raw Chicken": foodOutcome = "Cooked Chicken"; break;
            case "Raw Ham": foodOutcome = "Cooked Ham"; break;
        }

        foreach (Item food in foods)
        {
            if (food.itemName == foodOutcome)
            {
                cookedFood = food;
                break;
            }
        }

        if (cookedFood == null) return;
        itemSlot.ReplaceItem(cookedFood);
    }

    private void LitCampfire(bool b)
    {
        litCampfire.SetActive(b);
        unlitCampfire.SetActive(!b);
        isLit = b;
    }

    private void UpdateUI()
    {
        if (fuelBar != null)
            fuelBar.SetFuel(currentFuel, maxFuel);
    }

    public void AddFuel(float amount)
    {
        currentFuel = Mathf.Clamp(currentFuel + amount, 0, maxFuel);
        UpdateUI();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            fkey.SetActive(true);
            isWithinRange = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        fkey.SetActive(false);
        isWithinRange = false;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, fireRadius);
    }
}

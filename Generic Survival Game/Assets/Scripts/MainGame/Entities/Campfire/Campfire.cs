using UnityEngine;

public class Campfire : MonoBehaviour
{
    [Header("Fuel Settings")]
    public float maxFuel = 40f;
    public float currentFuel;
    public float burnRate = 0.5f; // fuel per second

    private CampfireFuelBar fuelBar;
    [SerializeField] private GameObject unlitCampfire;
    [SerializeField] private GameObject litCampfire;

    private void Start()
    {
        currentFuel = maxFuel;
        fuelBar = GetComponentInChildren<CampfireFuelBar>();
        UpdateUI();
    }

    private void Update()
    {
        if (currentFuel > 0)
        {
            LitCampfire(true);

            currentFuel -= burnRate * Time.deltaTime;
            if (currentFuel < 0) currentFuel = 0;

            UpdateUI();
        } else
        {
            LitCampfire(false);
        }
    }

    private void LitCampfire(bool b)
    {
        litCampfire.SetActive(b);
        unlitCampfire.SetActive(!b);
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
}

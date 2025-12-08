using UnityEngine;

public class Campfire : MonoBehaviour
{
    [SerializeField] private GameObject fkey;
    private CampfireFuelBar fuelBar;
    [SerializeField] private GameObject unlitCampfire;
    [SerializeField] private GameObject litCampfire;
    private bool isWithinRange = false;

    [Header("Fuel Settings")]
    public float maxFuel = 40f;
    public float currentFuel;
    public float burnRate = 0.5f; // fuel per second

    [Header("Cooked Food to Give")]
    [SerializeField] private Item[] foods;

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

        if (isWithinRange == true)
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                InventoryUIManager inventory = InventoryUIManager.Instance;
                for (int i = 0; i < inventory.itemSlot.Length; i++)
                {
                    if (inventory.itemSlot[i].isSlotSelected == true)
                    {
                        if (inventory.itemSlot[i].item == null) return;
                        if (inventory.itemSlot[i].item.isConsumable == true && currentFuel > 0)
                        {
                            GiveCookedFood(inventory.itemSlot[i]);
                        } else if (inventory.itemSlot[i].item.isFuel == true)
                        {
                            AddFuel(inventory.itemSlot[i].item.fuelRestore);
                            inventory.itemSlot[i].DeductItem(1);
                        }
                    }
                }
            }
        }

    }

    private void GiveCookedFood(ItemSlotManager itemSlot)
    {
        Item cookedFood = null;
        string foodOutcome = null;

        switch (itemSlot.itemName)
        {
            case "Raw Chicken":
                foodOutcome = "Cooked Chicken";
                break;
            case "Raw Ham":
                foodOutcome = "Cooked Ham";
                break;
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
        if (collision.gameObject.tag == "Player")
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
}

using UnityEngine;
using static UnityEditor.Progress;

public class CraftingBench : MonoBehaviour
{
    [SerializeField] private GameObject fkey;
    [SerializeField] private GameObject craftingUI;
    [SerializeField] private GameObject crosshair;
    private bool isWithinRange = false;
    private bool isMenuToggled = false;
    private void Start()
    {
        craftingUI.SetActive(false);
    }
    void Update()
    {
        if (isWithinRange == true)
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                isMenuToggled = !isMenuToggled;
                craftingUI.SetActive(isMenuToggled);
            }
            if (isMenuToggled == true) crosshair.SetActive(false);
            else crosshair.SetActive(true);
        } else
        {
            isMenuToggled = false;
            craftingUI.SetActive(false);
            crosshair.SetActive(true);
        }
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

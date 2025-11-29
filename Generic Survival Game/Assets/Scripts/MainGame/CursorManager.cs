using UnityEngine;

public class CursorManager : MonoBehaviour
{
    [SerializeField] private GameObject inventoryUI;   // Assign your inventory UI GameObject
    [SerializeField] private GameObject pauseMenuUI;   // Assign your pause menu UI GameObject

    public bool isUiOpen;

    void Update()
    {
        // Check if inventory or pause menu is active
        isUiOpen = (inventoryUI.activeSelf || pauseMenuUI.activeSelf);

        if (isUiOpen)
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None; // free cursor movement
        }
        else
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Confined; // lock or confine as you prefer
        }
    }
}

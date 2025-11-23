using UnityEngine;

public class CursorManager : MonoBehaviour
{
    public GameObject inventoryUI;   // Assign your inventory UI GameObject
    public GameObject pauseMenuUI;   // Assign your pause menu UI GameObject

    void Update()
    {
        // Check if inventory or pause menu is active
        bool uiOpen = (inventoryUI.activeSelf || pauseMenuUI.activeSelf);

        if (uiOpen)
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

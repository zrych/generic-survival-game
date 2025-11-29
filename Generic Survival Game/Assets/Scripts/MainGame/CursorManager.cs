using UnityEngine;

public class CursorManager : MonoBehaviour
{
    [SerializeField] private GameObject inventoryUI;
    [SerializeField] private GameObject pauseMenuUI;  
    [SerializeField] private GameObject craftMenuUI;  

    public bool isUiOpen;

    void Update()
    {
        // Check if inventory or pause menu is active
        isUiOpen = (inventoryUI.activeSelf || pauseMenuUI.activeSelf || craftMenuUI.activeSelf);

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

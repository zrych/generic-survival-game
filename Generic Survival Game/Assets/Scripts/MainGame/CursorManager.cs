using UnityEngine;

public class CursorManager : MonoBehaviour
{
    [SerializeField] private GameObject inventoryUI;
    [SerializeField] private GameObject pauseMenuUI;  
    [SerializeField] private GameObject craftMenuUI;  
    [SerializeField] private GameObject crosshair;  

    public bool isUiOpen;

    [SerializeField] private SpriteRenderer chSprite;
    private Sprite tempSprite;

    private void Start()
    {
        tempSprite = chSprite.sprite;
    }

    void Update()
    {
        // Check if inventory or pause menu is active
        isUiOpen = (inventoryUI.activeSelf || pauseMenuUI.activeSelf || craftMenuUI.activeSelf);

        if (isUiOpen)
        {
            chSprite.sprite = null;
            crosshair.SetActive(false);
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None; // free cursor movement
        }
        else
        {
            chSprite.sprite = tempSprite;
            crosshair.SetActive(true);
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Confined; // lock or confine as you prefer
        }
    }
}

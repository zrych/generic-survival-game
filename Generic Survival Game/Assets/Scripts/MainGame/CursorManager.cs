using UnityEngine;

public class CursorManager : MonoBehaviour
{
    [SerializeField] private GameObject inventoryUI;
    [SerializeField] private GameObject pauseMenuUI;  
    [SerializeField] private GameObject craftMenuUI;  
    [SerializeField] private GameObject gameOverUI;  
    [SerializeField] private GameObject crosshair;  
    [SerializeField] private PauseMenuUI pauseMenu;  

    public bool isUiOpen;

    [SerializeField] private SpriteRenderer chSprite;
    private Sprite tempSprite;

    private void Start()
    {
        tempSprite = chSprite.sprite;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) pauseMenu.PauseGame();
        // Check if inventory or pause menu is active
        isUiOpen = (inventoryUI.activeSelf || pauseMenuUI.activeSelf || craftMenuUI.activeSelf || gameOverUI.activeSelf);

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

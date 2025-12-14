using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseManager : MonoBehaviour
{
    public GameObject pausePanel;      // Assign your PausePanel
    public GameObject customCursor;    // Your in-game custom cursor

    private bool isPaused = false;

    void Start()
    {
        // Start unpaused
        isPaused = false;
        pausePanel.SetActive(false);

        // Show custom cursor in gameplay
        if (customCursor != null) customCursor.SetActive(true);
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Confined;
    }

    void Update()
    {
        // Always toggle pause/resume with ESC
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause();
        }

        // Update custom cursor position if visible
        if (customCursor != null && !Cursor.visible)
        {
            Vector3 mousePos = Input.mousePosition;
            mousePos.z = 0;
            customCursor.transform.position = mousePos;
        }
    }

    private void TogglePause()
    {
        if (isPaused)
            ResumeGame();
        else
            PauseGame();
    }

    public void PauseGame()
    {
        isPaused = true;

        // Show pause menu
        if (pausePanel != null) pausePanel.SetActive(true);

        // Freeze game
        Time.timeScale = 0f;

        // Show system cursor for UI
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        // Hide custom cursor
        if (customCursor != null) customCursor.SetActive(false);
    }

    public void ResumeGame()
    {
        isPaused = false;

        // Hide pause menu
        if (pausePanel != null) pausePanel.SetActive(false);

        // Resume game
        Time.timeScale = 1f;

        // Hide system cursor, show custom cursor
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Confined;
        if (customCursor != null) customCursor.SetActive(true);
    }

    public void QuitToMainMenu()
    {
        // Reset pause state
        isPaused = false;
        Time.timeScale = 1f;

        // Show system cursor
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        // Hide custom cursor
        if (customCursor != null) customCursor.SetActive(false);

        SceneManager.LoadScene("TitleScreen");
    }
}

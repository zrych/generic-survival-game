using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenuUI : MonoBehaviour
{
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private GameObject pauseMenuPanel;
    [SerializeField] private GameObject confirmationMenu;
    [SerializeField] private GameObject confirmationMenuRestart;

    public void ResumeGame()
    {
        Time.timeScale = 1f;
        pauseMenuPanel.SetActive(false);
    }

    public void RestartGame()
    {
        pauseMenu.SetActive(false);
        confirmationMenuRestart.SetActive(true);
    }
    public void ConfirmRestart()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("World");
    }

    public void CancelRestart()
    {
        pauseMenu.SetActive(true);
        confirmationMenuRestart.SetActive(false);
    }

    public void OnExit()
    {
        pauseMenu.SetActive(false);
        confirmationMenu.SetActive(true);
    }

    public void OnConfirm()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("TitleScreen");
    }

    public void OnReturn()
    {
        pauseMenu.SetActive(true);
        confirmationMenu.SetActive(false);
    }

    public void PauseGame()
    {
        Time.timeScale = 0f;
        pauseMenuPanel.SetActive(true);
    }
}

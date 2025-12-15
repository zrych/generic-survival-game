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
        ResumeMusic();
    }

    public void RestartGame()
    {
        PlayButtonClick();
        pauseMenu.SetActive(false);
        confirmationMenuRestart.SetActive(true);
    }

    public void ConfirmRestart()
    {
        PlayButtonClick();
        Time.timeScale = 1f;
        ResumeMusic();
        SceneManager.LoadScene("World");
    }

    public void CancelRestart()
    {
        PlayButtonClick();
        pauseMenu.SetActive(true);
        confirmationMenuRestart.SetActive(false);
    }

    public void OnExit()
    {
        PlayButtonClick();
        pauseMenu.SetActive(false);
        confirmationMenu.SetActive(true);
    }

    public void OnConfirm()
    {
        PlayButtonClick();
        Time.timeScale = 1f;
        ResumeMusic();
        SceneManager.LoadScene("TitleScreen");
    }

    public void OnReturn()
    {
        PlayButtonClick();
        pauseMenu.SetActive(true);
        confirmationMenu.SetActive(false);
    }

    public void PauseGame()
    {
        Time.timeScale = 0f;
        pauseMenuPanel.SetActive(true);
        PauseMusic();
    }

    // --- Music Handling ---
    private void PauseMusic()
    {
        if (MusicManager.Instance != null && MusicManager.Instance.MusicSource != null)
        {
            MusicManager.Instance.MusicSource.Pause();
        }
    }

    private void ResumeMusic()
    {
        if (MusicManager.Instance != null && MusicManager.Instance.MusicSource != null)
        {
            MusicManager.Instance.MusicSource.UnPause();
        }
    }

    // --- Button Sound ---
    private void PlayButtonClick()
    {
        if (SoundManager.Instance != null)
        {
            SoundManager.Instance.PlaySound2D("ButtonClick");
        }
    }
}

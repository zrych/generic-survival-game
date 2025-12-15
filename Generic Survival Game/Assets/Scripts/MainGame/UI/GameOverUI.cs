using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverUI : MonoBehaviour
{
    public void ReturnToTitle()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("TitleScreen");
    }
    public void PlayAgain()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("World");
    }
}

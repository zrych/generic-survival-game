using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    public void PlayButtonClicked()
    {
        SceneManager.LoadScene("TWorldSelect");
    }
}

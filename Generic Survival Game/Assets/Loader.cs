using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
    public Animator transition;         // Your crossfade animator
    public float transitionTime = 1f;   // Animation duration

    // This is what your Play button will call
    public void Play()
    {
        StartCoroutine(LoadScene("nTWorldSelect"));
    }

    IEnumerator LoadScene(string sceneName)
    {
        // Play transition animation
        transition.SetTrigger("Start");

        // Wait for animation to finish
        yield return new WaitForSeconds(transitionTime);

        // Load next scene
        SceneManager.LoadScene(TWorldSelect);
    }
}

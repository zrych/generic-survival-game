using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverSound : MonoBehaviour
{
    [Header("Sound Settings")]
    [SerializeField] private string gameOverSoundID = "GAMEOVER";

    private AudioSource tempAudio;
    private bool hasPlayed = false;

    private void OnEnable()
    {
        if (hasPlayed) return;

        if (SoundManager.Instance != null)
        {
            AudioClip clip = SoundManager.Instance.GetClipPublic(gameOverSoundID);
            if (clip != null)
            {
                // Create a temporary AudioSource to play the sound
                GameObject tempObj = new GameObject("TempGameOverAudio");
                tempAudio = tempObj.AddComponent<AudioSource>();
                tempAudio.clip = clip;
                tempAudio.volume = SoundManager.Instance.CurrentSFXVolume;
                tempAudio.Play();

                Destroy(tempObj, clip.length); // clean up after clip finishes
            }

            hasPlayed = true;
        }

        // Stop the sound automatically when the scene is about to change
        SceneManager.sceneUnloaded += OnSceneUnloaded;
    }

    private void OnDisable()
    {
        hasPlayed = false;
        SceneManager.sceneUnloaded -= OnSceneUnloaded;
    }

    private void OnSceneUnloaded(Scene current)
    {
        if (tempAudio != null && tempAudio.isPlaying)
        {
            tempAudio.Stop();
        }
    }
}

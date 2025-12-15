using System.Collections;
using System.Diagnostics; // You can keep this if needed elsewhere
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public static MusicManager Instance;

    [SerializeField] private MusicLibrarys musicLibrarys;
    [SerializeField] private AudioSource musicSource;

    private Coroutine currentFadeCoroutine;

    // Public accessor for PauseMenuUI
    public AudioSource MusicSource => musicSource;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    /// <summary>
    /// Play music track by name with optional crossfade.
    /// </summary>
    public void PlayMusic(string trackName, float fadeDuration = 0.5f)
    {
        AudioClip nextClip = musicLibrarys.GetClipFromName(trackName);
        if (nextClip == null)
        {
            UnityEngine.Debug.LogWarning($"MusicManager: Track '{trackName}' not found in library.");
            return;
        }

        // Stop previous fade if one is running
        if (currentFadeCoroutine != null)
        {
            StopCoroutine(currentFadeCoroutine);
        }

        // Start crossfade
        currentFadeCoroutine = StartCoroutine(AnimateMusicCrossfade(nextClip, fadeDuration));
    }

    private IEnumerator AnimateMusicCrossfade(AudioClip nextTrack, float fadeDuration)
    {
        float startVolume = musicSource.volume;
        float percent = 0f;

        // Fade out current music
        while (percent < 1f)
        {
            percent += Time.deltaTime / fadeDuration;
            musicSource.volume = Mathf.Lerp(startVolume, 0f, percent);
            yield return null;
        }

        // Switch clip
        musicSource.clip = nextTrack;
        musicSource.Play();

        // Fade in new music
        percent = 0f;
        while (percent < 1f)
        {
            percent += Time.deltaTime / fadeDuration;
            musicSource.volume = Mathf.Lerp(0f, 1f, percent);
            yield return null;
        }

        musicSource.volume = 1f; // ensure full volume
        currentFadeCoroutine = null;
    }
}

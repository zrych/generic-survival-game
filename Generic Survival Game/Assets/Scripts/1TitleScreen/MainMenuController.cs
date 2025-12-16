using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;
using UnityEngine.UI;

public class MainMenuController : MonoBehaviour
{
    public AudioMixer audioMixer;
    public Slider musicSlider;
    public Slider sfxSlider;

    private void Start()
    {
        LoadVolume();
        // Play MainMenu track with smooth fade
        MusicManager.Instance.PlayMusic("MainMenu", 1f);
    }

    public void PlayButtonClicked()
    {
        SceneManager.LoadScene("World");
        // Optional: transition to Morning music when gameplay scene loads
        StartCoroutine(DelayedMusicTransition("Morning", 1f));
    }

    public void Quit()
    {
        // Explicitly use UnityEngine.Application to fix ambiguity
        UnityEngine.Application.Quit();
    }

    public void UpdateMusicVolume(float volume)
    {
        audioMixer.SetFloat("MusicVolume", volume);
        PlayerPrefs.SetFloat("MusicVolume", volume);
    }

    public void UpdateSoundVolume(float volume)
    {
        audioMixer.SetFloat("SFXVolume", volume);
        PlayerPrefs.SetFloat("SFXVolume", volume);

        // Update 2D SFX AudioSource in SoundManager
        if (SoundManager.Instance != null)
            SoundManager.Instance.SetSFXVolume(volume);
    }

    public void SaveVolume()
    {
        audioMixer.GetFloat("MusicVolume", out float musicVolume);
        PlayerPrefs.SetFloat("MusicVolume", musicVolume);

        audioMixer.GetFloat("SFXVolume", out float sfxVolume);
        PlayerPrefs.SetFloat("SFXVolume", sfxVolume);
    }

    public void LoadVolume()
    {
        float music = PlayerPrefs.GetFloat("MusicVolume", 0f);
        float sfx = PlayerPrefs.GetFloat("SFXVolume", 0f);

        musicSlider.value = music;
        sfxSlider.value = sfx;

        // Apply to AudioMixer
        if (audioMixer != null)
        {
            audioMixer.SetFloat("MusicVolume", music);
            audioMixer.SetFloat("SFXVolume", sfx);
        }

        // Apply to SoundManager 2D source
        if (SoundManager.Instance != null)
            SoundManager.Instance.SetSFXVolume(sfx);
    }

    private IEnumerator DelayedMusicTransition(string trackName, float fadeDuration)
    {
        // Wait one frame for the new scene to load
        yield return null;
        MusicManager.Instance.PlayMusic(trackName, fadeDuration);
    }
}

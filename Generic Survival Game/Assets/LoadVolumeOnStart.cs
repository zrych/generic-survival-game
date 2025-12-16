using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class PauseVolumeController : MonoBehaviour
{
    [Header("Mixer and sliders")]
    public AudioMixer audioMixer;
    public Slider musicSlider;
    public Slider sfxSlider;

    private const string MUSIC_KEY = "MusicVolume";
    private const string SFX_KEY = "SFXVolume";

    private void Start()
    {
        // Load saved volume values
        LoadVolume();

        // Add slider listeners
        if (musicSlider != null)
            musicSlider.onValueChanged.AddListener(UpdateMusicVolume);
        if (sfxSlider != null)
            sfxSlider.onValueChanged.AddListener(UpdateSFXVolume);
    }

    private void LoadVolume()
    {
        float music = PlayerPrefs.GetFloat(MUSIC_KEY, 0f);
        float sfx = PlayerPrefs.GetFloat(SFX_KEY, 0f);

        // Apply to AudioMixer
        if (audioMixer != null)
        {
            audioMixer.SetFloat("MusicVolume", music);
            audioMixer.SetFloat("SFXVolume", sfx);
        }

        // Apply to sliders
        if (musicSlider != null) musicSlider.value = music;
        if (sfxSlider != null) sfxSlider.value = sfx;

        // Apply to SoundManager 2D source
        if (SoundManager.Instance != null)
            SoundManager.Instance.SetSFXVolume(sfx);
    }

    public void UpdateMusicVolume(float value)
    {
        if (audioMixer != null)
            audioMixer.SetFloat("MusicVolume", value);

        PlayerPrefs.SetFloat(MUSIC_KEY, value);
    }

    public void UpdateSFXVolume(float value)
    {
        if (audioMixer != null)
            audioMixer.SetFloat("SFXVolume", value);

        // Update 2D SFX AudioSource via SoundManager
        if (SoundManager.Instance != null)
            SoundManager.Instance.SetSFXVolume(value);

        PlayerPrefs.SetFloat(SFX_KEY, value);
    }

    private void OnDestroy()
    {
        // Remove listeners
        if (musicSlider != null) musicSlider.onValueChanged.RemoveListener(UpdateMusicVolume);
        if (sfxSlider != null) sfxSlider.onValueChanged.RemoveListener(UpdateSFXVolume);
    }
}

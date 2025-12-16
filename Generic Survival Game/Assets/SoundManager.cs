using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;

    [SerializeField]
    private SoundLibrary sfxLibrary;
    [SerializeField]
    private AudioSource sfx2DSource;

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

    public void PlaySound3D(AudioClip clip, Vector3 pos)
    {
        if (clip != null)
        {
            AudioSource.PlayClipAtPoint(clip, pos);
        }
    }

    public void PlaySound3D(string soundName, Vector3 pos)
    {
        PlaySound3D(sfxLibrary.GetClipFromName(soundName), pos);
    }

    public void PlaySound2D(string soundName)
    {
        sfx2DSource.PlayOneShot(sfxLibrary.GetClipFromName(soundName));
    }

    public AudioClip GetClipPublic(string name)
    {
        return sfxLibrary.GetClipFromName(name);
    }

    public AudioClip GetClip(string soundName)
    {
        return sfxLibrary.GetClipFromName(soundName);
    }

    public void SetSFXVolume(float value)
    {
        if (sfx2DSource != null)
            sfx2DSource.volume = Mathf.Pow(10, value / 20f);
    }

    public float CurrentSFXVolume
    {
        get
        {
            if (sfx2DSource != null)
                return sfx2DSource.volume;
            return 1f; // default full volume
        }
    }


}
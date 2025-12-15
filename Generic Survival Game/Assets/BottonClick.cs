using UnityEngine;

public class ButtonSFX : MonoBehaviour
{
    public string soundName = "Click"; // Set the sound ID in Inspector

    // Call this function in the Button OnClick()
    public void PlaySound()
    {
        if (SoundManager.Instance != null)
            SoundManager.Instance.PlaySound2D(soundName);
    }
}

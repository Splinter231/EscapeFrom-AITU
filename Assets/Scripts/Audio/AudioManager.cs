using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    public const string VolumeKey = "MasterVolume";

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        float savedVolume = PlayerPrefs.GetFloat(VolumeKey, 1f);
        SetVolume(savedVolume);
    }

    public void SetVolume(float volume)
    {
        volume = Mathf.Clamp01(volume);

        AudioListener.volume = volume;

        PlayerPrefs.SetFloat(VolumeKey, volume);
        PlayerPrefs.Save();
    }

    public float GetVolume()
    {
        return PlayerPrefs.GetFloat(VolumeKey, 1f);
    }
}
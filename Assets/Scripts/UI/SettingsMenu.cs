using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour
{
    public Slider volumeSlider;
    public Toggle shadowsToggle;

    void Start()
    {
        EnsureAudioManagerExists();

        float savedVolume = PlayerPrefs.GetFloat(AudioManager.VolumeKey, 1f);
        int shadows = PlayerPrefs.GetInt("Shadows", 1);

        if (volumeSlider != null)
        {
            volumeSlider.SetValueWithoutNotify(savedVolume);
            volumeSlider.onValueChanged.RemoveListener(ChangeVolume);
            volumeSlider.onValueChanged.AddListener(ChangeVolume);
        }

        if (shadowsToggle != null)
        {
            shadowsToggle.SetIsOnWithoutNotify(shadows == 1);
            shadowsToggle.onValueChanged.RemoveListener(ToggleShadows);
            shadowsToggle.onValueChanged.AddListener(ToggleShadows);
        }

        AudioListener.volume = savedVolume;
        QualitySettings.shadows = shadows == 1 ? ShadowQuality.All : ShadowQuality.Disable;
    }

    void EnsureAudioManagerExists()
    {
        if (AudioManager.Instance != null)
        {
            return;
        }

        GameObject audioManagerObject = new GameObject("AudioManager");
        audioManagerObject.AddComponent<AudioManager>();
    }

    public void ChangeVolume(float value)
    {
        EnsureAudioManagerExists();
        AudioManager.Instance.SetVolume(value);

        Debug.Log("Volume changed to: " + value);
    }

    public void ToggleShadows(bool isOn)
    {
        QualitySettings.shadows = isOn ? ShadowQuality.All : ShadowQuality.Disable;

        PlayerPrefs.SetInt("Shadows", isOn ? 1 : 0);
        PlayerPrefs.Save();
    }

    public void BackToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
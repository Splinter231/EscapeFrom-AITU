using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour
{
    public Slider volumeSlider;
    public Toggle shadowsToggle;

    void Start()
    {
        float volume = PlayerPrefs.GetFloat("Volume", 1f);
        int shadows = PlayerPrefs.GetInt("Shadows", 1);

        volumeSlider.value = volume;
        shadowsToggle.isOn = shadows == 1;

        AudioListener.volume = volume;
        QualitySettings.shadows = shadows == 1 ? ShadowQuality.All : ShadowQuality.Disable;
    }

    public void ChangeVolume(float value)
    {
        AudioListener.volume = value;
        PlayerPrefs.SetFloat("Volume", value);
        PlayerPrefs.Save();
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
using TMPro;
using UnityEngine;

public class HUDManager : MonoBehaviour
{
    public static HUDManager Instance;

    public TMP_Text scoreText;
    public TMP_Text healthText;
    public TMP_Text keyText;
    public TMP_Text highScoreText;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        if (keyText != null)
        {
            keyText.text = "Key: Not collected";
        }

        int highScore = PlayerPrefs.GetInt("HighScore", 0);

        if (highScoreText != null)
        {
            highScoreText.text = "High Score: " + highScore;
        }
    }

    public void UpdateScore(int score)
    {
        if (scoreText != null)
        {
            scoreText.text = "Score: " + score;
        }
    }

    public void UpdateHealth(int currentHealth, int maxHealth)
    {
        if (healthText != null)
        {
            healthText.text = "HP: " + currentHealth + " / " + maxHealth;
        }
    }

    public void ShowKeyCollected()
    {
        if (keyText != null)
        {
            keyText.text = "Key: Collected";
        }
    }
}

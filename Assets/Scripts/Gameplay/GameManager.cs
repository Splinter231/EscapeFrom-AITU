using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public int score;
    public bool hasKey;

    public GameObject winPanel;
    public GameObject losePanel;

    private bool gameEnded;

    void Awake()
    {
        Instance = this;
        Time.timeScale = 1f;
    }

    void Start()
    {
        score = 0;
        hasKey = false;

        if (HUDManager.Instance != null)
        {
            HUDManager.Instance.UpdateScore(score);
        }

        if (winPanel != null)
        {
            winPanel.SetActive(false);
        }

        if (losePanel != null)
        {
            losePanel.SetActive(false);
        }
    }

    public void AddScore(int amount)
    {
        score += amount;

        if (HUDManager.Instance != null)
        {
            HUDManager.Instance.UpdateScore(score);
        }
    }

    public void CollectKey()
    {
        hasKey = true;

        if (HUDManager.Instance != null)
        {
            HUDManager.Instance.ShowKeyCollected();
        }
    }

    public void WinGame()
    {
        if (gameEnded)
        {
            return;
        }

        gameEnded = true;
        SaveHighScore();

        if (winPanel != null)
        {
            winPanel.SetActive(true);
        }

        Time.timeScale = 0f;
    }

    public void LoseGame()
    {
        if (gameEnded)
        {
            return;
        }

        gameEnded = true;

        if (losePanel != null)
        {
            losePanel.SetActive(true);
        }

        Time.timeScale = 0f;
    }

    void SaveHighScore()
    {
        int oldHighScore = PlayerPrefs.GetInt("HighScore", 0);

        if (score > oldHighScore)
        {
            PlayerPrefs.SetInt("HighScore", score);
            PlayerPrefs.Save();
        }
    }

    public void RestartLevel()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void GoToMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }
}

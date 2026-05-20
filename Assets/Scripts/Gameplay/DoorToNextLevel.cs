using UnityEngine;
using UnityEngine.SceneManagement;

public class DoorToNextLevel : MonoBehaviour
{
    public string nextSceneName = "Level2";
    public bool requiresKey = true;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player"))
        {
            return;
        }

        if (requiresKey && !GameManager.Instance.hasKey)
        {
            Debug.Log("Access card required.");
            return;
        }

        SceneManager.LoadScene(nextSceneName);
    }
}
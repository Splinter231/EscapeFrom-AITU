using UnityEngine;

public class WinZone : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player"))
        {
            return;
        }

        GameManager.Instance.WinGame();
    }
}
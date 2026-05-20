using UnityEngine;

public class WinZone : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player"))
        {
            return;
        }

        Animator playerAnimator = other.GetComponentInChildren<Animator>();

        if (playerAnimator != null)
        {
            playerAnimator.SetTrigger("Win");
        }

        GameManager.Instance.WinGame();
    }
}
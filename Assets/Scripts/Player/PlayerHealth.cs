using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 100;
    public int currentHealth;

    private Animator animator;

    void Start()
    {
        currentHealth = maxHealth;
        animator = GetComponentInChildren<Animator>();

        if (HUDManager.Instance != null)
        {
            HUDManager.Instance.UpdateHealth(currentHealth, maxHealth);
        }
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        if (HUDManager.Instance != null)
        {
            HUDManager.Instance.UpdateHealth(currentHealth, maxHealth);
        }

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public void Heal(int amount)
    {
        currentHealth += amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        if (HUDManager.Instance != null)
        {
            HUDManager.Instance.UpdateHealth(currentHealth, maxHealth);
        }
    }

    void Die()
    {
        if (animator != null)
        {
            animator.SetTrigger("Lose");
        }

        if (GameManager.Instance != null)
        {
            GameManager.Instance.LoseGame();
        }
    }
}
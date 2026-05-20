using UnityEngine;

public enum PickupType
{
    Score,
    Health,
    Key
}

public class Pickup : MonoBehaviour
{
    public PickupType pickupType;
    public int value = 25;
    public AudioClip pickupSound;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player"))
        {
            return;
        }

        if (pickupType == PickupType.Score)
        {
            GameManager.Instance.AddScore(value);
        }

        if (pickupType == PickupType.Health)
        {
            PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();

            if (playerHealth != null)
            {
                playerHealth.Heal(value);
            }
        }

        if (pickupType == PickupType.Key)
        {
            GameManager.Instance.CollectKey();
            GameManager.Instance.AddScore(value);
        }

        if (pickupSound != null)
        {
            AudioSource.PlayClipAtPoint(pickupSound, transform.position);
        }

        Destroy(gameObject);
    }
}
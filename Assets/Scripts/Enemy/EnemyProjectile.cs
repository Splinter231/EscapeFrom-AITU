using UnityEngine;

public class EnemyProjectile : MonoBehaviour
{
    private Vector2 direction;
    private float speed;
    private int damage;

    public float lifetime = 4f;

    public void Initialize(Vector2 newDirection, float newSpeed, int newDamage)
    {
        direction = newDirection.normalized;
        speed = newSpeed;
        damage = newDamage;

        if (direction == Vector2.zero)
        {
            direction = Vector2.left;
        }

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);

        Destroy(gameObject, lifetime);
    }

    void Update()
    {
        if (GameManager.Instance != null && GameManager.Instance.IsGameEnded)
        {
            return;
        }

        transform.position += (Vector3)(direction * speed * Time.deltaTime);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();

        if (playerHealth != null)
        {
            playerHealth.TakeDamage(damage);
            Destroy(gameObject);
            return;
        }

        if (!other.CompareTag("Enemy") && !other.isTrigger)
        {
            Destroy(gameObject);
        }
    }
}
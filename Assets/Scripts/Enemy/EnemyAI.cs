using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public float moveSpeed = 2f;
    public float detectionRange = 6f;
    public float attackRange = 1f;
    public int damage = 10;
    public float attackCooldown = 1.2f;

    private Transform player;
    private Rigidbody2D rb;
    private float nextAttackTime;

    void Start()
    {
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");

        if (playerObject != null)
        {
            player = playerObject.transform;
        }

        rb = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        if (player == null)
        {
            return;
        }

        float distance = Vector2.Distance(transform.position, player.position);

        if (distance <= detectionRange && distance > attackRange)
        {
            Vector2 direction = (player.position - transform.position).normalized;
            rb.MovePosition(rb.position + direction * moveSpeed * Time.fixedDeltaTime);
        }

        if (distance <= attackRange)
        {
            AttackPlayer();
        }
    }

    void AttackPlayer()
    {
        if (Time.time < nextAttackTime)
        {
            return;
        }

        PlayerHealth playerHealth = player.GetComponent<PlayerHealth>();

        if (playerHealth != null)
        {
            playerHealth.TakeDamage(damage);
        }

        nextAttackTime = Time.time + attackCooldown;
    }
}
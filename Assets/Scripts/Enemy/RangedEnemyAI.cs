using UnityEngine;

public class RangedEnemyAI : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 2f;
    public float detectionRange = 8f;
    public float preferredDistance = 5f;
    public float tooCloseDistance = 3f;

    [Header("Shooting")]
    public GameObject enemyProjectilePrefab;
    public Transform shootPoint;
    public float projectileSpeed = 6f;
    public int projectileDamage = 10;
    public float shootingCooldown = 1.5f;

    private Transform player;
    private Rigidbody2D rb;
    private float nextShootTime;

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
        if (GameManager.Instance != null && GameManager.Instance.IsGameEnded)
        {
            return;
        }

        if (player == null)
        {
            return;
        }

        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        if (distanceToPlayer <= detectionRange)
        {
            HandleMovement(distanceToPlayer);
            TryShoot(distanceToPlayer);
        }
    }

    void HandleMovement(float distanceToPlayer)
    {
        Vector2 directionToPlayer = (player.position - transform.position).normalized;

        if (distanceToPlayer > preferredDistance)
        {
            rb.MovePosition(rb.position + directionToPlayer * moveSpeed * Time.fixedDeltaTime);
        }
        else if (distanceToPlayer < tooCloseDistance)
        {
            rb.MovePosition(rb.position - directionToPlayer * moveSpeed * Time.fixedDeltaTime);
        }
        else
        {
            rb.linearVelocity = Vector2.zero;
        }
    }

    void TryShoot(float distanceToPlayer)
    {
        if (distanceToPlayer > detectionRange)
        {
            return;
        }

        if (Time.time < nextShootTime)
        {
            return;
        }

        Shoot();

        nextShootTime = Time.time + shootingCooldown;
    }

    void Shoot()
    {
        if (enemyProjectilePrefab == null)
        {
            Debug.LogWarning("Enemy projectile prefab is not assigned.");
            return;
        }

        if (shootPoint == null)
        {
            Debug.LogWarning("Enemy shoot point is not assigned.");
            return;
        }

        Vector2 shootDirection = (player.position - shootPoint.position).normalized;

        GameObject projectileObject = Instantiate(
            enemyProjectilePrefab,
            shootPoint.position,
            Quaternion.identity
        );

        EnemyProjectile projectile = projectileObject.GetComponent<EnemyProjectile>();

        if (projectile != null)
        {
            projectile.Initialize(shootDirection, projectileSpeed, projectileDamage);
        }
    }
}
using UnityEngine;

public enum WeaponType
{
    Melee,
    Ranged
}

public class PlayerAttack : MonoBehaviour
{
    [Header("Current Weapon")]
    public WeaponType currentWeapon = WeaponType.Melee;

    [Header("Melee Attack")]
    public Transform attackPoint;
    public float meleeAttackRange = 1.4f;
    public float meleeAttackAngle = 90f;
    public int meleeDamage = 25;
    public LayerMask enemyLayer;

    [Header("Ranged Attack")]
    public GameObject projectilePrefab;
    public Transform shootPoint;
    public float projectileSpeed = 8f;
    public int projectileDamage = 20;

    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip meleeAttackSound;
    public AudioClip rangedAttackSound;
    public AudioClip weaponSwitchSound;

    private Animator animator;
    private Camera mainCamera;

    void Start()
    {
        animator = GetComponent<Animator>();
        mainCamera = Camera.main;

        if (audioSource == null)
        {
            audioSource = GetComponent<AudioSource>();
        }
    }

    void Update()
    {
        if (GameManager.Instance != null && GameManager.Instance.IsGameEnded)
        {
            return;
        }

        UpdateAttackPointsDirection();

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            SwitchWeapon(WeaponType.Melee);
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            SwitchWeapon(WeaponType.Ranged);
        }

        if (Input.GetMouseButtonDown(0))
        {
            Attack();
        }
    }

    void UpdateAttackPointsDirection()
    {
        Vector2 directionToMouse = GetDirectionToMouse();

        if (attackPoint != null)
        {
            attackPoint.localPosition = directionToMouse * 0.8f;
        }

        if (shootPoint != null)
        {
            shootPoint.localPosition = directionToMouse * 0.8f;
        }
    }

    Vector2 GetDirectionToMouse()
    {
        if (mainCamera == null)
        {
            mainCamera = Camera.main;
        }

        Vector3 mouseScreenPosition = Input.mousePosition;
        Vector3 mouseWorldPosition = mainCamera.ScreenToWorldPoint(mouseScreenPosition);

        mouseWorldPosition.z = 0f;

        Vector2 direction = mouseWorldPosition - transform.position;

        if (direction.magnitude < 0.1f)
        {
            return Vector2.right;
        }

        return direction.normalized;
    }

    void SwitchWeapon(WeaponType newWeapon)
    {
        currentWeapon = newWeapon;

        if (weaponSwitchSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(weaponSwitchSound);
        }

        Debug.Log("Switched weapon to: " + currentWeapon);
    }

    void Attack()
    {
        if (currentWeapon == WeaponType.Melee)
        {
            MeleeAttack();
        }
        else if (currentWeapon == WeaponType.Ranged)
        {
            RangedAttack();
        }
    }

    void MeleeAttack()
    {
        if (animator != null)
        {
            animator.SetTrigger("Attack");
        }

        if (meleeAttackSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(meleeAttackSound);
        }

        Vector2 attackDirection = GetDirectionToMouse();

        Collider2D[] possibleEnemies = Physics2D.OverlapCircleAll(
            transform.position,
            meleeAttackRange,
            enemyLayer
        );

        foreach (Collider2D enemyCollider in possibleEnemies)
        {
            Vector2 directionToEnemy = enemyCollider.transform.position - transform.position;

            float angleToEnemy = Vector2.Angle(attackDirection, directionToEnemy);

            if (angleToEnemy <= meleeAttackAngle / 2f)
            {
                EnemyHealth enemyHealth = enemyCollider.GetComponent<EnemyHealth>();

                if (enemyHealth != null)
                {
                    enemyHealth.TakeDamage(meleeDamage);
                }
            }
        }
    }

    void RangedAttack()
    {
        if (projectilePrefab == null)
        {
            Debug.LogWarning("Projectile Prefab is not assigned.");
            return;
        }

        if (shootPoint == null)
        {
            Debug.LogWarning("Shoot Point is not assigned.");
            return;
        }

        if (animator != null)
        {
            animator.SetTrigger("Attack");
        }

        if (rangedAttackSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(rangedAttackSound);
        }

        Vector2 shootDirection = GetDirectionToMouse();

        GameObject projectileObject = Instantiate(
            projectilePrefab,
            shootPoint.position,
            Quaternion.identity
        );

        Projectile projectile = projectileObject.GetComponent<Projectile>();

        if (projectile != null)
        {
            projectile.Initialize(shootDirection, projectileSpeed, projectileDamage);
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, meleeAttackRange);

        Vector2 direction;

        if (Application.isPlaying)
        {
            direction = GetDirectionToMouse();
        }
        else
        {
            direction = Vector2.right;
        }

        Vector3 leftBoundary = Quaternion.Euler(0, 0, meleeAttackAngle / 2f) * direction;
        Vector3 rightBoundary = Quaternion.Euler(0, 0, -meleeAttackAngle / 2f) * direction;

        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(transform.position, transform.position + leftBoundary * meleeAttackRange);
        Gizmos.DrawLine(transform.position, transform.position + rightBoundary * meleeAttackRange);
    }
}
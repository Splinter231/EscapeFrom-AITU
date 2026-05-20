using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public enum WeaponType
{
    Melee,
    Ranged
}

public class PlayerAttack : MonoBehaviour
{
    [Header("Current Weapon")]
    public WeaponType currentWeapon = WeaponType.Melee;

    [Header("Melee Flashlight Attack")]
    public Transform attackPoint;
    public float meleeAttackRange = 2.2f;
    public float meleeAttackAngle = 90f;
    public int meleeDamage = 25;
    public LayerMask enemyLayer;

    [Header("Melee Spot Light")]
    public Light2D meleeSpotLight;
    public float spotLightIntensity = 5f;
    public float spotLightDuration = 0.18f;

    [Tooltip("How far the spotlight is moved forward toward the cursor.")]
    public float spotLightForwardOffset = 0.2f;

    [Tooltip("Local offset for placing the spotlight near the character's hands.")]
    public Vector2 spotLightLocalOffset = new Vector2(0f, 0.35f);

    [Tooltip("Use 0, 90, or -90 if the cone points in the wrong direction.")]
    public float spotLightRotationOffset = 0f;

    [Header("Ranged Attack")]
    public GameObject projectilePrefab;
    public Transform shootPoint;
    public float projectileSpeed = 8f;
    public int projectileDamage = 20;

    [Header("Cooldowns")]
    public float meleeCooldown = 0.45f;
    public float rangedCooldown = 0.6f;

    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip meleeAttackSound;
    public AudioClip rangedAttackSound;
    public AudioClip weaponSwitchSound;

    [Header("Animation")]
    public Animator animator;

    private Camera mainCamera;
    private float nextAttackTime;

    void Start()
    {
        if (animator == null)
        {
            animator = GetComponentInChildren<Animator>();
        }

        mainCamera = Camera.main;

        if (audioSource == null)
        {
            audioSource = GetComponent<AudioSource>();
        }

        if (meleeSpotLight != null)
        {
            meleeSpotLight.intensity = 0f;
        }
    }

    void Update()
    {
        if (GameManager.Instance != null && GameManager.Instance.IsGameEnded)
        {
            return;
        }

        UpdateAttackDirectionObjects();

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

    void UpdateAttackDirectionObjects()
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

        if (meleeSpotLight != null)
        {
            Vector2 finalSpotPosition = directionToMouse * spotLightForwardOffset + spotLightLocalOffset;
            meleeSpotLight.transform.localPosition = finalSpotPosition;

            float angle = Mathf.Atan2(directionToMouse.y, directionToMouse.x) * Mathf.Rad2Deg;
            meleeSpotLight.transform.rotation = Quaternion.Euler(0f, 0f, angle + spotLightRotationOffset);
        }
    }

    Vector2 GetDirectionToMouse()
    {
        if (mainCamera == null)
        {
            mainCamera = Camera.main;
        }

        if (mainCamera == null)
        {
            return Vector2.right;
        }

        Vector3 mouseWorldPosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
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
        if (Time.time < nextAttackTime)
        {
            return;
        }

        if (currentWeapon == WeaponType.Melee)
        {
            nextAttackTime = Time.time + meleeCooldown;
            MeleeFlashlightAttack();
        }
        else if (currentWeapon == WeaponType.Ranged)
        {
            nextAttackTime = Time.time + rangedCooldown;
            RangedAttack();
        }
    }

    void MeleeFlashlightAttack()
    {
        if (animator != null)
        {
            animator.SetTrigger("Attack");
        }

        if (meleeAttackSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(meleeAttackSound);
        }

        StartCoroutine(FlashSpotLight());

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

    IEnumerator FlashSpotLight()
    {
        if (meleeSpotLight == null)
        {
            yield break;
        }

        meleeSpotLight.intensity = spotLightIntensity;

        yield return new WaitForSeconds(spotLightDuration);

        meleeSpotLight.intensity = 0f;
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
            animator.SetTrigger("Shoot");
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
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, meleeAttackRange);

        Vector2 direction = Application.isPlaying ? GetDirectionToMouse() : Vector2.right;

        Vector3 leftBoundary = Quaternion.Euler(0, 0, meleeAttackAngle / 2f) * direction;
        Vector3 rightBoundary = Quaternion.Euler(0, 0, -meleeAttackAngle / 2f) * direction;

        Gizmos.DrawLine(transform.position, transform.position + leftBoundary * meleeAttackRange);
        Gizmos.DrawLine(transform.position, transform.position + rightBoundary * meleeAttackRange);
    }
}
using System.Collections;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    public float walkSpeed = 4f;
    public float runSpeed = 7f;

    [Header("Dash Settings")]
    public float dashDistance = 3f;
    public float dashDuration = 0.15f;
    public float dashCooldown = 1f;

    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip dashSound;

    private Rigidbody2D rb;
    private Animator animator;
    private Camera mainCamera;

    private Vector2 movement;
    private bool isRunning;
    private bool isDashing;
    private bool canDash = true;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        mainCamera = Camera.main;

        if (audioSource == null)
        {
            audioSource = GetComponent<AudioSource>();
        }
    }

    void Update()
    {
        if (isDashing)
        {
            return;
        }

        ReadMovementInput();
        UpdateAnimation();

        if (Input.GetKeyDown(KeyCode.Space) && canDash)
        {
            StartCoroutine(Dash());
        }
    }

    void FixedUpdate()
    {
        if (isDashing)
        {
            return;
        }

        MovePlayer();
    }

    void ReadMovementInput()
    {
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");
        movement = movement.normalized;

        isRunning = Input.GetKey(KeyCode.LeftShift);
    }

    void MovePlayer()
    {
        float currentSpeed = isRunning ? runSpeed : walkSpeed;
        rb.MovePosition(rb.position + movement * currentSpeed * Time.fixedDeltaTime);
    }

    void UpdateAnimation()
    {
        bool isMoving = movement.magnitude > 0;

        if (animator == null)
        {
            return;
        }

        animator.SetBool("isMoving", isMoving);
        animator.SetBool("isRunning", isRunning && isMoving);

        if (isMoving)
        {
            animator.SetFloat("moveX", movement.x);
            animator.SetFloat("moveY", movement.y);
        }
    }

    IEnumerator Dash()
    {
        canDash = false;
        isDashing = true;

        Vector2 dashDirection = GetDashDirection();
        Vector2 startPosition = rb.position;
        Vector2 targetPosition = startPosition + dashDirection * dashDistance;

        if (animator != null)
        {
            animator.SetTrigger("Jump");
        }

        if (dashSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(dashSound);
        }

        float elapsedTime = 0f;

        while (elapsedTime < dashDuration)
        {
            float t = elapsedTime / dashDuration;
            Vector2 newPosition = Vector2.Lerp(startPosition, targetPosition, t);

            rb.MovePosition(newPosition);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        rb.MovePosition(targetPosition);

        isDashing = false;

        yield return new WaitForSeconds(dashCooldown);

        canDash = true;
    }

    Vector2 GetDashDirection()
    {
        Vector2 directionToMouse = GetDirectionToMouse();

        if (directionToMouse != Vector2.zero)
        {
            return directionToMouse;
        }

        if (movement != Vector2.zero)
        {
            return movement.normalized;
        }

        return Vector2.right;
    }

    Vector2 GetDirectionToMouse()
    {
        if (mainCamera == null)
        {
            mainCamera = Camera.main;
        }

        if (mainCamera == null)
        {
            return Vector2.zero;
        }

        Vector3 mouseScreenPosition = Input.mousePosition;
        Vector3 mouseWorldPosition = mainCamera.ScreenToWorldPoint(mouseScreenPosition);
        mouseWorldPosition.z = 0f;

        Vector2 direction = mouseWorldPosition - transform.position;

        if (direction.magnitude < 0.1f)
        {
            return Vector2.zero;
        }

        return direction.normalized;
    }
}
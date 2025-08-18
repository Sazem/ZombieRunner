using UnityEngine;

public class ZombieMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 2f;
    [SerializeField] private float rotationSpeed = 5f;
    [SerializeField] Animator animator;

    [Header("Target")]
    public Transform target;

    private Rigidbody2D rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        if (target == null)
        {
            rb.linearVelocity = Vector2.zero;
            return;
        }

        // Direction to target
        Vector2 direction = (target.position - transform.position).normalized;

        // Smooth rotation toward target
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;
        Quaternion targetRotation = Quaternion.Euler(0, 0, angle);
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, rotationSpeed * Time.fixedDeltaTime);

        // Move forward in facing direction
        Vector2 forward = transform.up * moveSpeed;
        rb.linearVelocity = forward;

        if (animator != null)
        {
            animator.SetFloat("speed", rb.linearVelocity.magnitude);
        }
    }
}



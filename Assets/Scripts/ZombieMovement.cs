using UnityEngine;

public class ZombieMovement : MonoBehaviour, IPushable
{
    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 2f;
    [SerializeField] private float rotationSpeed = 5f;
    [SerializeField] Animator animator;

    [Header("Target")]
    public Transform target;

    private Rigidbody2D rb;

    private Vector2 pushVelocity;
    private float pushDownTimer = 0f;
    [SerializeField] private float pushDecay = 5f;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        target = GameManager.Instance.GetPlayer().transform;
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
        if (target != null)
        {
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;
            Quaternion targetRotation = Quaternion.Euler(0, 0, angle);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, rotationSpeed * Time.fixedDeltaTime);
        }

        // Move forward in facing direction
        Vector2 forward = transform.up * moveSpeed;

        // Gradually reduce knockback
        pushVelocity = Vector2.Lerp(pushVelocity, Vector2.zero, pushDecay * Time.fixedDeltaTime);

        rb.linearVelocity = forward + pushVelocity;

        if (animator != null)
        {
            animator.SetFloat("speed", rb.linearVelocity.magnitude);
        }
    }
    public void ReceivePush(Vector3 dir, float pushAmount)
    {
        pushVelocity = dir * pushAmount;
    }
}



using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 8f;
    [SerializeField] private float turnSpeed = 200f;
    [SerializeField] private Animator animator;
    private Rigidbody2D rb;

    private Vector3 prevPosition; // last position for calculating the speed from the moved distance.

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        prevPosition = transform.position;
    }


    // TODO
    // Player runs directly to wall (perpendicular), it gets stuck.
    //      How to Fix: 
    //      A) Player can force rotation and/or sliding along the normal of the obstacle. 
    //      B) Send raycast forward to player, when obstacle in front, stop movement. 
    void FixedUpdate()
    {
        // Move forward constantly, its an infinite runner!
        Vector2 forward = transform.up * moveSpeed * Time.fixedDeltaTime;
        rb.MovePosition(rb.position + forward);

        // Rotate with A/D keys (testing before UI & mobile export)
        float turnInput = Input.GetAxisRaw("Horizontal"); // A = -1, D = 1
        float rotation = -turnInput * turnSpeed * Time.fixedDeltaTime;
        rb.MoveRotation(rb.rotation + rotation);

        // calculate speed for animation
        float currentVelocity = Vector3.Distance(prevPosition, transform.position);
        currentVelocity /= Time.deltaTime;
        prevPosition = transform.position;
        // Movement animation
        if (animator != null)
        {
            animator.SetFloat("speed", currentVelocity);
        }
    }
}

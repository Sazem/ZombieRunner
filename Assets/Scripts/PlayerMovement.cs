using System;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 8f;
    [SerializeField] private float turnSpeed = 200f;
    [SerializeField] private Animator animator;
    private Rigidbody2D rb;

    private Vector3 prevPosition; // last position for calculating the speed from the moved distance.
    private Vector2 pushVelocity;
    private float pushDownTimer = 0f;
    [SerializeField] private float pushDecay = 5f;
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
        // Always move forward based on facing direction
        Vector2 forwardVelocity = transform.up * moveSpeed;

        if (Input.GetKey(KeyCode.A) && Input.GetKey(KeyCode.D))
        {
            pushDownTimer += Time.deltaTime; // added small delay to reverse. The player movement is a lot smoother. Without this, the player can change the rotation in same frame and it started to look jumpy.
            if (pushDownTimer > 0.1)
            {
                forwardVelocity *= -1; // reverse movement when both keys are hold
            }
        }
        else
        {
            pushDownTimer = 0.0f;
        }
        // Blend knockback and movement
        rb.linearVelocity = forwardVelocity + pushVelocity;

        // Gradually reduce knockback
        pushVelocity = Vector2.Lerp(pushVelocity, Vector2.zero, pushDecay * Time.fixedDeltaTime);

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


    public void ReceivePush(Vector3 dir, float pushAmount)
    {
        pushVelocity = dir * pushAmount;
    }
}

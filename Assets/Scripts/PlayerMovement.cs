using System;
using UnityEngine;
using MoreMountains.Feedbacks;
using UnityEngine.Analytics;
public class PlayerMovement : MonoBehaviour, IPushable
{
    [SerializeField] private float moveSpeed = 8f;
    private float originalSpeed;
    [SerializeField] private float turnSpeed = 200f;
    [SerializeField] private Animator animator;
    private Rigidbody2D rb;
    [SerializeField] private LayerMask obstacleLayerMask;
    [SerializeField] private float obsticleCheckDistance = 1.0f;

    private Vector3 prevPosition; // last position for calculating the speed from the moved distance.
    private Vector2 pushVelocity;
    private float pushDownTimer = 0f;
    [SerializeField] private float pushDecay = 5f;
    [SerializeField] private MMF_Player walkFeedback;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        prevPosition = transform.position;
        originalSpeed = moveSpeed;
    }

    void FixedUpdate()
    {
        // Always move forward based on facing direction
        Vector2 forwardVelocity = transform.up * moveSpeed;

                // Raycast parameters
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

        RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.up, obsticleCheckDistance, obstacleLayerMask);
        Debug.DrawRay(transform.position, transform.up * obsticleCheckDistance, Color.red);

        float sideAngle = 30f; 
        Vector2 forward = transform.up;
        Vector2 leftDir = Quaternion.Euler(0, 0, -sideAngle) * forward;
        Vector2 rightDir = Quaternion.Euler(0, 0, sideAngle) * forward;

        // Rotate with A/D keys (testing before UI & mobile export)
        float turnInput = Input.GetAxisRaw("Horizontal"); // A = -1, D = 1
        float rotation = -turnInput * turnSpeed * Time.fixedDeltaTime;

        // Cast rays for detecting obstacles. 
        RaycastHit2D centerHit = Physics2D.Raycast(transform.position, forward, obsticleCheckDistance, obstacleLayerMask);
        RaycastHit2D leftHit = Physics2D.Raycast(transform.position, leftDir, obsticleCheckDistance, obstacleLayerMask);
        RaycastHit2D rightHit = Physics2D.Raycast(transform.position, rightDir, obsticleCheckDistance, obstacleLayerMask);

        // Debug rays
        Debug.DrawRay(transform.position, forward * obsticleCheckDistance, Color.red);
        Debug.DrawRay(transform.position, leftDir * obsticleCheckDistance, Color.green);
        Debug.DrawRay(transform.position, rightDir * obsticleCheckDistance, Color.blue);

        // Determine steering
        Vector2 steerDirection = Vector2.zero;

        if (centerHit.collider != null)
        {
            // Steer away from center obstacle using normal
            steerDirection += centerHit.normal;
        }

        if (leftHit.collider != null && rightHit.collider == null)
        {
            // Obstacle on left only steer right
            steerDirection += rightDir;
        }
        else if (rightHit.collider != null && leftHit.collider == null)
        {
            // Obstacle on right only steer left
            steerDirection += leftDir;
        }
        else if (leftHit.collider != null && rightHit.collider != null)
        {
            // Both sides hit, steer away from closer one
            steerDirection += (leftHit.distance < rightHit.distance) ? rightDir : leftDir;
        }

        // Apply steering
        if (steerDirection != Vector2.zero)
        {
            // Ensure minimum turn angle
            float angleToSteer = Vector2.SignedAngle(forward, steerDirection.normalized);
            float minTurnAngle = 91f;
            float turnDirection = Mathf.Sign(angleToSteer);
            float finalTurnAngle = Mathf.Abs(angleToSteer) < minTurnAngle
                ? minTurnAngle * turnDirection
                : angleToSteer;

            float autoTurn = Mathf.Clamp(finalTurnAngle, -1f, 1f) * turnSpeed * Time.fixedDeltaTime;
            rb.linearVelocity = (forwardVelocity + pushVelocity) / 1.5f; // make the forward movement slightly slower.
            rb.MoveRotation(rb.rotation + autoTurn);
        } else { // Apply rotation from inputs, when no obstacles in front.
            // Blend knockback and movement
            rb.linearVelocity = forwardVelocity + pushVelocity;

            // Gradually reduce knockback
            pushVelocity = Vector2.Lerp(pushVelocity, Vector2.zero, pushDecay * Time.fixedDeltaTime);
            rb.MoveRotation(rb.rotation + rotation);
        }

        // calculate speed for animation
        float currentVelocity = Vector3.Distance(prevPosition, transform.position);
        currentVelocity /= Time.deltaTime;
        prevPosition = transform.position;

        // play walk feedback if player is moving more than threshold.
        if (currentVelocity > 1.0)
        {

            if (walkFeedback.IsPlaying == false)
            {
                walkFeedback?.PlayFeedbacks(); // MMFeel, add walking sounds, particles etc.
            }
        }
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
    // boost speed, used for ex; item pickups. Adjusted from Inventory. 
    public void AddSpeed(float amount)
    {
        moveSpeed += amount;
    }
    public void ResetSpeed()
    {
        moveSpeed = originalSpeed;
    }

}

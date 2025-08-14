using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 8f;
    [SerializeField] private float turnSpeed = 200f;

    private Rigidbody2D rigidbody2D;

    void Start()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        // Move forward constantly
        Vector2 forward = transform.up * moveSpeed * Time.fixedDeltaTime;
        rigidbody2D.MovePosition(rigidbody2D.position + forward);

        // Rotate with A/D keys
        float turnInput = Input.GetAxisRaw("Horizontal"); // A = -1, D = 1
        float rotation = -turnInput * turnSpeed * Time.fixedDeltaTime;
        rigidbody2D.MoveRotation(rigidbody2D.rotation + rotation);

    }
}

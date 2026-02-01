using UnityEngine;

public class BallMovement : MonoBehaviour {
    [SerializeField] private float speed = 5f;
    private Vector2 direction;
    private Rigidbody2D rb;

    public float GetSpeed() {
        return speed;
    }

    public void SetSpeed(float newSpeed) {
        speed = Mathf.Max(0f, newSpeed);
    }

    public Vector2 GetDirection() {
        return direction;
    }

    public void SetDirection(Vector2 newDirection) {
        direction = newDirection.normalized;
    }

    void Start() {
        rb = GetComponent<Rigidbody2D>();
        direction = new Vector2(1f, 1f).normalized;
    }

    void FixedUpdate() {
        rb.velocity = direction * speed;
    }

    void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.CompareTag("Paddle"))
        {
            direction = new Vector2(-direction.x, direction.y).normalized;
            return;
        }

        Vector2 normal = collision.contacts[0].normal;
        direction = Vector2.Reflect(direction, normal).normalized;
    }
}

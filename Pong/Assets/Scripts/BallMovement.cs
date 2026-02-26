using UnityEngine;
using Unity.Netcode;

public class BallMovement : NetworkBehaviour, ICollidable {
    [SerializeField] private float speed = 5f;
    private Vector2 direction;
    private Rigidbody2D rb;

    void Start() {
        rb = GetComponent<Rigidbody2D>();
        direction = new Vector2(1f, 1f).normalized;
    }

    void FixedUpdate() {
        if (!IsServer) return;
        rb.velocity = direction * speed;    
    }

    void OnCollisionEnter2D(Collision2D collision) {
        if (!IsServer) return;

        ICollidable collidable = collision.gameObject.GetComponent<ICollidable>();
        if (collidable != null) collidable.OnHit(collision);

        OnHit(collision);
    }

    public void OnHit(Collision2D collision) {
        if (collision.gameObject.CompareTag("Paddle")) {
            direction = new Vector2(-direction.x, direction.y).normalized;
            return;
        }

        Vector2 normal = collision.contacts[0].normal;
        direction = Vector2.Reflect(direction, normal).normalized;
    }
}
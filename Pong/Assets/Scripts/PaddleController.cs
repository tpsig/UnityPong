using Unity.Netcode;
using UnityEngine;

public abstract class PaddleController : NetworkBehaviour {
    [SerializeField] protected float speed = 8f;
    protected Rigidbody2D rb;

    public NetworkVariable<float> NetworkY = new NetworkVariable<float>();

    protected virtual void Start() {
        rb = GetComponent<Rigidbody2D>();
    }

    protected virtual void FixedUpdate() {
        if (IsOwner) {
            float input = GetMovementInput();
            rb.velocity = new Vector2(0f, input * speed);

            NetworkY.Value = transform.position.y;
        } else {
            Vector3 pos = transform.position;
            pos.y = NetworkY.Value;
            transform.position = pos;
        }
    }

    protected abstract float GetMovementInput();

    public virtual void OnHit(Collision2D collision) { }
}
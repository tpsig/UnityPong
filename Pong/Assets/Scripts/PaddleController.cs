using UnityEngine;

public abstract class PaddleController : MonoBehaviour {
    [SerializeField] protected float speed = 8f;
    protected Rigidbody2D rb;

    protected virtual void Start() {
        rb = GetComponent<Rigidbody2D>();
    }

    protected virtual void FixedUpdate() {
        float input = GetMovementInput();
        rb.velocity = new Vector2(0f, input * speed);
    }

    protected abstract float GetMovementInput();
    
    public virtual void OnHit(Collision2D collision) {
        
    }
}
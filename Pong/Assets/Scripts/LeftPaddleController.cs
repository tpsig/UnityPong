using UnityEngine;

public class LeftPaddle : PaddleController {
    private SpriteRenderer spriteRenderer;

    protected override void Start() {
        base.Start();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    protected override float GetMovementInput() {
        return Input.GetAxis("LeftPaddle");
    }

    public override void OnHit(Collision2D collision) {
        
    }
}

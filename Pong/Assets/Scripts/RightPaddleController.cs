using UnityEngine;

public class RightPaddle : PaddleController {
    private SpriteRenderer spriteRenderer;
    
     protected override void Start() {
        base.Start();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    
    protected override float GetMovementInput() {
        return Input.GetAxis("RightPaddle");
    }
    
    public override void OnHit(Collision2D collision) {
        
    }
}

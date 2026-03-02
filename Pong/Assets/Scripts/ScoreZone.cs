using UnityEngine;

public class ScoreZone : MonoBehaviour {
    public enum ZoneType { Left, Right }
    public ZoneType zoneType;

    private void OnTriggerEnter2D(Collider2D collision) {
        if (!collision.CompareTag("Ball")) return;

        if (GameManager.Instance == null) {
            Debug.LogWarning("GameManager not found!");
            return;
        }

        switch (zoneType) {
            case ZoneType.Left:
                GameManager.Instance.IncrementRightScore();
                break;
            case ZoneType.Right:
                GameManager.Instance.IncrementLeftScore();
                break;
        }
    }
}
using UnityEngine;
using Unity.Netcode;
using TMPro;
using UnityEngine.UI;

public class GameManager : NetworkBehaviour {
    public NetworkVariable<int> leftScore = new NetworkVariable<int>(0);
    public NetworkVariable<int> rightScore = new NetworkVariable<int>(0);
    public NetworkVariable<bool> gameStarted = new NetworkVariable<bool>(false);
    public NetworkVariable<bool> gameOver = new NetworkVariable<bool>(false);

    public static GameManager Instance;

    [Header("Game Settings")]
    [SerializeField] private int pointsToWin = 5;
    [SerializeField] private GameObject ball;

    [Header("UI Elements")]
    [SerializeField] private TextMeshProUGUI leftScoreText;
    [SerializeField] private TextMeshProUGUI rightScoreText;
    [SerializeField] private TextMeshProUGUI winMessageText;
    [SerializeField] private Button startButton;

    private void Awake() {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        if (startButton != null)
            startButton.gameObject.SetActive(false);
    }

    public override void OnNetworkSpawn() {
        if (startButton != null)
            startButton.gameObject.SetActive(IsServer && !gameStarted.Value);

        gameStarted.OnValueChanged += (_, _) => {
            if (startButton != null)
                startButton.gameObject.SetActive(IsServer && !gameStarted.Value);
        };
    }

    private void Start() {
        if (startButton != null)
            startButton.onClick.AddListener(StartGame);

        if (winMessageText != null)
            winMessageText.gameObject.SetActive(false);

        leftScore.OnValueChanged += (_, _) => UpdateScoreUI();
        rightScore.OnValueChanged += (_, _) => UpdateScoreUI();
        gameOver.OnValueChanged += (_, _) => UpdateScoreUI();

        UpdateScoreUI();
    }

    private void UpdateStartButton() {
        if (startButton == null) return;

        startButton.gameObject.SetActive(IsServer && !gameStarted.Value);
    }

    private void UpdateScoreUI() {
        if (leftScoreText != null) leftScoreText.text = leftScore.Value.ToString();
        if (rightScoreText != null) rightScoreText.text = rightScore.Value.ToString();

        if (winMessageText != null) {
            if (gameOver.Value) {
                winMessageText.gameObject.SetActive(true);
                winMessageText.text = leftScore.Value > rightScore.Value
                    ? "Player 2 Wins!"
                    : "Player 1 Wins!";
            } else {
                winMessageText.gameObject.SetActive(false);
            }
        }
    }

    public void IncrementLeftScore() {
        if (!IsServer || gameOver.Value) return;

        leftScore.Value++;
        CheckWinCondition();
        if (!gameOver.Value)
            ResetBall(false);
        Debug.Log("Left Score: " + leftScore.Value);
    }

    public void IncrementRightScore() {
        if (!IsServer || gameOver.Value) return;

        rightScore.Value++;
        CheckWinCondition();
        if (!gameOver.Value)
            ResetBall(true);
        Debug.Log("Right Score: " + rightScore.Value);
    }

    private void CheckWinCondition() {
        if (leftScore.Value >= pointsToWin) {
            gameOver.Value = true;
            gameStarted.Value = false;
            Debug.Log("Player 2 Wins!");
            StopBall();
        }
        else if (rightScore.Value >= pointsToWin) {
            gameOver.Value = true;
            gameStarted.Value = false;
            Debug.Log("Player 1 Wins!");
            StopBall();
        }
    }

    private void StopBall() {
        if (ball == null) return;

        Rigidbody2D rb = ball.GetComponent<Rigidbody2D>();
        if (rb != null) {
            rb.velocity = Vector2.zero;
            rb.angularVelocity = 0f;
        }
        ball.transform.position = Vector3.zero;
    }

    private void ResetBall(bool towardLeftPlayer) {
        if (!IsServer || ball == null) return;

        ball.transform.position = Vector3.zero;

        Rigidbody2D rb = ball.GetComponent<Rigidbody2D>();
        rb.velocity = Vector2.zero;

        float minY = 0.3f;
        float maxY = 0.8f;
        float yAngle = Random.Range(minY, maxY);
        if (Random.value > 0.5f) yAngle *= -1f;

        Vector2 newDirection = new Vector2(towardLeftPlayer ? -1f : 1f, yAngle).normalized;

        BallMovement ballMovement = ball.GetComponent<BallMovement>();
        if (ballMovement != null)
            ballMovement.SetDirection(newDirection);

        rb.velocity = newDirection * 5f;
    }

    public void StartGame() {
        if (!IsServer) return;

        leftScore.Value = 0;
        rightScore.Value = 0;
        gameOver.Value = false;

        gameStarted.Value = true;

        ResetBall(Random.value > 0.5f);

        Debug.Log("Game Started!");
    }
}
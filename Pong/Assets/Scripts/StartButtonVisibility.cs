using UnityEngine;
using Unity.Netcode;
using UnityEngine.UI;

public class StartButtonController : NetworkBehaviour
{
    [SerializeField] private Button startButton;

    private void Awake()
    {
        // Make sure button is visible locally in Editor
        if (startButton != null)
            startButton.gameObject.SetActive(true);
    }

    public override void OnNetworkSpawn()
    {
        UpdateButtonVisibility();
    }

    private void UpdateButtonVisibility()
    {
        if (startButton == null) return;

        // Only host sees the start button
        startButton.gameObject.SetActive(IsServer);
    }

    // Call this after the game starts to hide it
    public void OnGameStarted()
    {
        if (startButton != null)
            startButton.gameObject.SetActive(false);
    }
}
using UnityEngine;
using UnityEngine.UI;

public class StartGameUI : MonoBehaviour
{
    public GameManager gameManager;
    public Player humanPlayer; // Drag Player0 manually in the Inspector
    public Button startButton;

    public void StartGame()
    {
        if (gameManager == null || humanPlayer == null)
        {
            Debug.LogWarning("GameManager or HumanPlayer is not assigned!");
            return;
        }

        // Add the human player (already existing in the scene)
        humanPlayer.isHuman = true;
        humanPlayer.Initialize(gameManager.GetNextPlayerIndex(), "Player");
        gameManager.players.Add(humanPlayer);

        // Add 5 AI players
        for (int i = 1; i <= 5; i++)
        {
            gameManager.AddPlayer($"Bot {i}");
        }

        // Start the game
        gameManager.StartGame();

        // Hide the start button
        startButton.gameObject.SetActive(false);
    }
}

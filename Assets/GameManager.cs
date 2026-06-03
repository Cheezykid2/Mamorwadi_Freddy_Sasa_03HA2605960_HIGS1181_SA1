using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    // Simple flag indicating if it's the player's turn
    public bool isPlayerTurn = true;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            // Optionally persist across scenes
            // DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // End the player's turn. In a real game this would trigger enemy turns.
    public void EndPlayerTurn()
    {
        isPlayerTurn = false;
        Debug.Log("Player turn ended.");
        // For now, immediately start player turn again to keep gameplay simple
        // In a full implementation, you'd run enemy turns here.
        StartEnemyPhase();
    }

    private void StartEnemyPhase()
    {
        // Placeholder: simply start player turn after a short delay
        // This method could be expanded to find enemies and call their TakeTurn()
        Invoke(nameof(StartPlayerTurn), 0.1f);
    }

    public void StartPlayerTurn()
    {
        isPlayerTurn = true;
        Debug.Log("Player turn started.");
    }

    public void GameOver(bool playerWon)
    {
        Debug.Log(playerWon ? "Player won!" : "Player lost!");
        // Implement game over behavior (UI, restart, etc.) as needed
    }
}

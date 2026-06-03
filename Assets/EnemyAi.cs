using UnityEngine;

/// <summary>
/// Enemy AI for a turn-based game.
/// Moves one tile closer to the player each turn.
/// </summary>
[RequireComponent(typeof(Rigidbody2D))]
public class EnemyAI : MonoBehaviour
{
    [Header("Movement Settings")]
    [Tooltip("Time (in seconds) it takes to move one tile.")]
    public float moveTime = 0.2f;

    [Header("Health")]
    public int health = 3;

    private Rigidbody2D rb2D;
    private float inverseMoveTime;
    private Vector2 targetPosition;

    private void Awake()
    {
        // Cache the Rigidbody2D component
        rb2D = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        // Calculate movement speed factor
        inverseMoveTime = 1f / moveTime;

        // Enemy starts at its current position
        targetPosition = rb2D.position;
    }

    /// <summary>
    /// Called by the GameManager when it's the enemy's turn.
    /// </summary>
    public void TakeTurn()
    {
        // Find the player's position
        GameObject player = GameObject.FindWithTag("Player");
        if (player == null)
        {
            Debug.LogWarning("No Player found in scene!");
            return;
        }

        Vector2 playerPos = player.transform.position;
        Vector2 enemyPos = rb2D.position;

        // Decide whether to move horizontally or vertically
        Vector2 moveDir = Vector2.zero;

        if (Mathf.Abs(playerPos.x - enemyPos.x) > Mathf.Abs(playerPos.y - enemyPos.y))
        {
            // Move horizontally closer
            moveDir = (playerPos.x > enemyPos.x) ? Vector2.right : Vector2.left;
        }
        else
        {
            // Move vertically closer
            moveDir = (playerPos.y > enemyPos.y) ? Vector2.up : Vector2.down;
        }

        // Calculate new target position
        targetPosition = enemyPos + moveDir;

        // Move enemy one tile
        rb2D.MovePosition(targetPosition);

        Debug.Log("Enemy moved toward player.");
    }

    /// <summary>
    /// Apply damage to the enemy. Destroys the enemy when health reaches zero.
    /// </summary>
    public void TakeDamage(int amount)
    {
        health -= amount;
        Debug.Log($"Enemy took {amount} damage. HP = {health}");
        if (health <= 0)
        {
            Destroy(gameObject);
            Debug.Log("Enemy destroyed");
        }
    }
}

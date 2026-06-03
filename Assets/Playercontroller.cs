using UnityEngine;

using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Player Settings")]
    public int playerHP = 5;                  // Player health
    public float moveTime = 0.2f;             // Time to move one tile
    public int attackDamage = 1;              // Damage dealt per attack

    private Rigidbody2D rb2D;
    private Vector2 targetPosition;
    private bool isMoving = false;

    private void Awake()
    {
        rb2D = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        targetPosition = rb2D.position;
    }

    private void Update()
    {
        // Only allow input if it's the player's turn
        if (GameManager.instance.isPlayerTurn && !isMoving)
        {
            HandleMovementInput();
            HandleAttackInput();
        }
    }

    private void HandleMovementInput()
    {
        Vector2 moveDir = Vector2.zero;

        if (Input.GetKeyDown(KeyCode.W)) moveDir = Vector2.up;
        else if (Input.GetKeyDown(KeyCode.S)) moveDir = Vector2.down;
        else if (Input.GetKeyDown(KeyCode.A)) moveDir = Vector2.left;
        else if (Input.GetKeyDown(KeyCode.D)) moveDir = Vector2.right;

        if (moveDir != Vector2.zero)
        {
            targetPosition = rb2D.position + moveDir;
            StartCoroutine(MoveToTile(targetPosition));
        }
    }

    private void HandleAttackInput()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            AttemptAttack();
        }
    }

    private System.Collections.IEnumerator MoveToTile(Vector2 destination)
    {
        isMoving = true;

        float elapsed = 0f;
        Vector2 startPos = rb2D.position;

        while (elapsed < moveTime)
        {
            rb2D.MovePosition(Vector2.Lerp(startPos, destination, elapsed / moveTime));
            elapsed += Time.deltaTime;
            yield return null;
        }

        rb2D.MovePosition(destination);
        isMoving = false;

        Debug.Log("Player moved to " + destination);

        // End player turn after moving
        GameManager.instance.EndPlayerTurn();
    }

    private void AttemptAttack()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

        foreach (GameObject enemy in enemies)
        {
            float distance = Vector2.Distance(rb2D.position, enemy.transform.position);

            if (distance <= 1.1f) // within 1 tile
            {
                EnemyAI enemyAI = enemy.GetComponent<EnemyAI>();
                if (enemyAI != null)
                {
                    enemyAI.TakeDamage(attackDamage);
                    Debug.Log("Player attacked enemy!");
                }

                // End player turn after attacking
                GameManager.instance.EndPlayerTurn();
                return;
            }
        }

        Debug.Log("No enemy in range to attack.");
    }

    public void TakeDamage(int damage)
    {
        playerHP -= damage;
        Debug.Log("Player took damage! HP = " + playerHP);

        if (playerHP <= 0)
        {
            GameManager.instance.GameOver(false); // false = player lost
        }
    }
}


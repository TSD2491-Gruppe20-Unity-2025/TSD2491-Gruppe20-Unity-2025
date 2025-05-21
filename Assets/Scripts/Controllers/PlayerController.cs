using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 1f;
    [SerializeField] private Vector2 minBounds;
    [SerializeField] private Vector2 maxBounds;
    [SerializeField] private int health = 3;
    [SerializeField] private float contactDamageCooldown = 1f;

    private Dictionary<GameObject, float> lastContactTime = new();
    private InputSystem_Actions inputActions;
    private Vector2 movemet;
    private Rigidbody2D rb;

    private void Awake()
    {
        inputActions = new InputSystem_Actions();
        rb = GetComponent<Rigidbody2D>();
    }

    private void OnEnable()
    {
        inputActions.Enable();
    }

    private void Update()
    {
        PlayerInput();
    }

    private void FixedUpdate()
    {
        Move();
    }

    private void PlayerInput()
    {
        movemet = inputActions.Player.Move.ReadValue<Vector2>();
    }

    private void Move()
    {
        Vector2 newPosition = rb.position + movemet * (moveSpeed * Time.fixedDeltaTime);
        newPosition.x = Mathf.Clamp(newPosition.x, minBounds.x, maxBounds.x);
        newPosition.y = Mathf.Clamp(newPosition.y, minBounds.y, maxBounds.y);
        rb.MovePosition(newPosition);
    }

    public virtual void TakeDamage(int amount)
    {
        health -= amount;
        Debug.Log("Player hit! Remaining HP: " + health);
        if (health <= 0)
        {
            Debug.Log("Player died.");
            // Add death logic here
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        HandleCollision(other);
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        HandleCollision(other);
    }

    private void HandleCollision(Collider2D other)
    {
        // Enemy bullet hits player
        if (other.CompareTag("EnemyBullet"))
        {
            TakeDamage(1);
            Destroy(other.gameObject);
        }

        // Player touches an enemy
        if (other.CompareTag("Enemy"))
        {
            GameObject enemy = other.gameObject;

            if (!lastContactTime.ContainsKey(enemy)) lastContactTime[enemy] = -100f;

            if (Time.time - lastContactTime[enemy] >= contactDamageCooldown)
            {
                TakeDamage(1);
                lastContactTime[enemy] = Time.time;
            }
        }
    }
}


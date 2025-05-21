using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public GameObject playerUI;

    [SerializeField] private float moveSpeed = 1f;
    [SerializeField] private Vector2 minBounds;
    [SerializeField] private Vector2 maxBounds;
    [SerializeField] private float contactDamageCooldown = 1f;

    [Header("Health Settings")]
    [SerializeField] public int maxHealth = 9;
    [SerializeField] public int startHealth = 9;

    [Header("UI")]
    [SerializeField] public UIScript uiScript;

    public int CurrentHealth { get; private set; }

    private Dictionary<GameObject, float> lastContactTime = new();
    private InputSystem_Actions inputActions;
    private Vector2 movement;
    private Rigidbody2D rb;
    private BaseWeapon weapon;


    private bool canMove = false;

    public delegate void OnHealthChanged(int currentHealth);
    public event OnHealthChanged HealthChanged;

    private void Awake()
    {
        inputActions = new InputSystem_Actions();
        rb = GetComponent<Rigidbody2D>();
        CurrentHealth = Mathf.Clamp(startHealth, 0, maxHealth);
        weapon = GetComponent<BaseWeapon>();

    }

    public void AssignUI(UIScript ui)
    {
        uiScript = ui;

        if (uiScript != null)
        {
            uiScript.Initialize(maxHealth);
            uiScript.SetHealth(CurrentHealth); // Sync on assignment
        }
    }

    private void OnEnable()
    {
        inputActions?.Enable();
        inputActions.Player.Action1.performed += ctx => FireWeapon();

    }

    private void OnDisable()
    {
        inputActions?.Disable();
        inputActions.Player.Action1.performed -= ctx => FireWeapon();
    }

    private void Update()
    {
        if (canMove)
        {
            PlayerInput();
        }
    }

    private void FixedUpdate()
    {
        if (canMove)
        {
            Move();
        }
    }

    private void PlayerInput()
    {
        movement = inputActions.Player.Move.ReadValue<Vector2>();
    }
    private void FireWeapon()
    {
        weapon?.Fire();
    }
    private void Move()
    {
        Vector2 newPosition = rb.position + movement * (moveSpeed * Time.fixedDeltaTime);
        newPosition.x = Mathf.Clamp(newPosition.x, minBounds.x, maxBounds.x);
        newPosition.y = Mathf.Clamp(newPosition.y, minBounds.y, maxBounds.y);
        rb.MovePosition(newPosition);
    }

    public void StartFlyIn(Vector2 targetPosition)
    {
        StartCoroutine(FlyInRoutine(targetPosition));
    }

    private IEnumerator FlyInRoutine(Vector2 targetPos)
    {
        float duration = 1.5f;
        float elapsed = 0f;
        Vector2 startPos = transform.position;
        canMove = false;

        Debug.Log("Fly-in started for: " + gameObject.name);

        while (elapsed < duration)
        {
            Vector2 newPos = Vector2.Lerp(startPos, targetPos, elapsed / duration);
            rb.MovePosition(newPos);
            elapsed += Time.deltaTime;
            yield return null;
        }

        rb.MovePosition(targetPos);
        canMove = true;

        Debug.Log("Fly-in complete for: " + gameObject.name);
    }

    public virtual void TakeDamage(int amount)
    {
        CurrentHealth = Mathf.Clamp(CurrentHealth - amount, 0, maxHealth);
        Debug.Log("Player hit! Remaining HP: " + CurrentHealth);

        if (uiScript != null)
        {
            uiScript.SetHealth(CurrentHealth);
        }

        HealthChanged?.Invoke(CurrentHealth);

        if (CurrentHealth <= 0)
        {
            Time.timeScale = 0f;
            Debug.Log("Game Over! Game stopped.");
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    //-----------------------------------------------------------------------------//
    // Inspector Variables

    public GameObject playerUI;

    [SerializeField] private float moveSpeed = 1f;
    [SerializeField] private Vector2 minBounds;
    [SerializeField] private Vector2 maxBounds;
    [SerializeField] private float contactDamageCooldown = 1f;

    [Header("Health Settings")]
    [SerializeField] public int maxHealth = 9;
    [SerializeField] public int startHealth = 9;

    [Header("Weapon Upgrades")]
    public bool doubleShotEnabled = false;


    [Header("UI")]
    [SerializeField] public UIScript uiScript;

    //-----------------------------------------------------------------------------//
    // Private Fields

    public int CurrentHealth { get; private set; }

    private Dictionary<GameObject, float> lastContactTime = new();
    private InputSystem_Actions inputActions;
    private Vector2 movement;
    private Rigidbody2D rb;
    private BaseWeapon weapon;
    private bool canMove = false;

    //-----------------------------------------------------------------------------//
    // Events

    public delegate void OnHealthChanged(int currentHealth);
    public event OnHealthChanged HealthChanged;

    //-----------------------------------------------------------------------------//
    // Unity Methods

    private void Awake()
    {
        inputActions = new InputSystem_Actions();
        rb = GetComponent<Rigidbody2D>();
        weapon = GetComponent<BaseWeapon>();
        CurrentHealth = Mathf.Clamp(startHealth, 0, maxHealth);
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

    //-----------------------------------------------------------------------------//
    // Input and Movement

    private void PlayerInput()
    {
        movement = inputActions.Player.Move.ReadValue<Vector2>();
    }

    private void Move()
    {
        //MOVEMENT FORWARD AND SIDEWAYS
        Vector2 Position = rb.position + movement * (moveSpeed * Time.fixedDeltaTime);

        // Restrict movement to camera bounds
        float cameraHeight = Camera.main.orthographicSize * 2;
        float cameraWidth = cameraHeight * Camera.main.aspect;

        float minX = -cameraWidth / 2;
        float maxX = cameraWidth / 2;
        float minY = -Camera.main.orthographicSize;
        float maxY = Camera.main.orthographicSize;

        Position.x = Mathf.Clamp(Position.x, minX, maxX);
        Position.y = Mathf.Clamp(Position.y, minY, maxY);

        rb.MovePosition(Position);
    }

    private void FireWeapon()
    {
        if (weapon == null) return;

        if (doubleShotEnabled)
        {
            (weapon as BaseWeapon)?.FireDouble();
        }
        else
        {
            weapon.Fire();
        }
    }


    //-----------------------------------------------------------------------------//
    // Fly-in Effect

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

    //-----------------------------------------------------------------------------//
    // Health Management

    public virtual void TakeDamage(int amount)
    {
        CurrentHealth = Mathf.Clamp(CurrentHealth - amount, 0, maxHealth);
        Debug.Log("Player hit! Remaining HP: " + CurrentHealth);

        uiScript?.SetHealth(CurrentHealth);
        HealthChanged?.Invoke(CurrentHealth);

        if (CurrentHealth <= 0)
        {
            Time.timeScale = 0f;
            Debug.Log("Game Over! Game stopped.");
        }
    }

    public void AssignUI(UIScript ui)
    {
        uiScript = ui;

        if (uiScript != null)
        {
            uiScript.Initialize(maxHealth);
            uiScript.SetHealth(CurrentHealth);
        }
    }

    public void Heal(int amount)
    {
        CurrentHealth = Mathf.Clamp(CurrentHealth + amount, 0, maxHealth);
        Debug.Log("Player healed! Current HP: " + CurrentHealth);

        uiScript?.SetHealth(CurrentHealth);
        HealthChanged?.Invoke(CurrentHealth);
    }


    //-----------------------------------------------------------------------------//
    // Collision Handling

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

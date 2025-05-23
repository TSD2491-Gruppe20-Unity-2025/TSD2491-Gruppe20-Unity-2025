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
    [SerializeField] private InputActionAsset inputAsset;
    [SerializeField] private Vector2 minBounds;
    [SerializeField] private Vector2 maxBounds;
    [SerializeField] private float contactDamageCooldown = 1f;
    [SerializeField] private float loopCooldown = 3f;

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
    private bool isLooping = false;
    private bool canLoop = true;
    private bool isInvulnerable = false;

    protected InputActionMap actionMap;
    protected InputAction moveAction;
    protected InputAction action1;
    protected InputAction action2;

    //-----------------------------------------------------------------------------//
    // Events

    public delegate void OnHealthChanged(int currentHealth);
    public event OnHealthChanged HealthChanged;

    //-----------------------------------------------------------------------------//
    // Unity Methods

    protected virtual string ActionMapName => "PlayerOne";

    protected virtual void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        weapon = GetComponent<BaseWeapon>();
        CurrentHealth = Mathf.Clamp(startHealth, 0, maxHealth);
        if (inputAsset == null)
        {
            Debug.LogError("Input Action Asset is not assigned!");
            return;
        }

        if (inputAsset != null)
        {
            actionMap = inputAsset.FindActionMap(ActionMapName);
            moveAction = actionMap.FindAction("Move");
            action1 = actionMap.FindAction("Action1");
            action2 = actionMap.FindAction("Action2");
        }
    }

    protected virtual void OnEnable()
    {
        actionMap?.Enable();

        if (action1 != null) action1.performed += OnAction1Performed;
        if (action2 != null) action2.performed += OnAction2Performed;
    }

    protected virtual void OnDisable()
    {
        if (action1 != null) action1.performed -= OnAction1Performed;
        if (action2 != null) action2.performed -= OnAction2Performed;

        actionMap?.Disable();
    }

    protected virtual void Update()
    {
        if (canMove && moveAction != null)
        {
            movement = moveAction.ReadValue<Vector2>();
        }
    }

    private void FixedUpdate()
    {
        if (canMove)
        {
            Move();
        }
    }

    protected virtual void OnAction1Performed(InputAction.CallbackContext ctx)
    {
        FireWeapon();
    }

    protected virtual void OnAction2Performed(InputAction.CallbackContext ctx)
    {
        PerformAction2();
    }

    //-----------------------------------------------------------------------------//
    // Input and Movement

    private void PlayerInput()
    {
        movement = inputActions.Player.Move.ReadValue<Vector2>();
    }

    private void Move()
    {
        Vector2 Position = rb.position + movement * (moveSpeed * Time.fixedDeltaTime);

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

  
      protected void PerformAction2()
    {
        Debug.Log("Action2 triggered");

        if (!isLooping && canLoop)
        {
            StartCoroutine(DoClunkyLoop());
        }
    }



private IEnumerator DoClunkyLoop()
    {
        isLooping = true;
        isInvulnerable = true;
        canLoop = false;

        float duration = 1.2f;
        float elapsed = 0f;

        Quaternion startRotation = transform.rotation;
        Quaternion halfLoop = Quaternion.Euler(180f, 0f, 0f);
        Quaternion fullLoop = Quaternion.Euler(360f, 0f, 0f);

        while (elapsed < duration / 2f)
        {
            transform.rotation = Quaternion.Slerp(startRotation, halfLoop, elapsed / (duration / 2f));
            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.rotation = halfLoop;
        elapsed = 0f;

        while (elapsed < duration / 2f)
        {
            transform.rotation = Quaternion.Slerp(halfLoop, fullLoop, elapsed / (duration / 2f));
            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.rotation = Quaternion.identity;
        isLooping = false;
        isInvulnerable = false;

        yield return new WaitForSeconds(loopCooldown);
        canLoop = true;
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
        if (isInvulnerable) return;

        CurrentHealth = Mathf.Clamp(CurrentHealth - amount, 0, maxHealth);
        Debug.Log("Player hit! Remaining HP: " + CurrentHealth);

        uiScript?.SetHealth(CurrentHealth);
        HealthChanged?.Invoke(CurrentHealth);

        if (CurrentHealth <= 0)
        {
            Destroy(gameObject);
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

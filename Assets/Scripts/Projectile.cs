using UnityEngine;

public class Projectile : MonoBehaviour
{
    //-----------------------------------------------------------------------------//
    // Projectile Properties

    public float speed = 20f;
    public Vector2 direction = Vector2.zero;
    public string targetTag;

    private Camera mainCamera;

    //-----------------------------------------------------------------------------//
    // Unity Methods

    private void Start()
    {
        mainCamera = Camera.main;

        // Apply initial rotation based on direction
        RotateToDirection();
    }

    void Update()
    {
        // Move the projectile in its set direction
        transform.Translate(direction * speed * Time.deltaTime, Space.World);

        // Despawn if outside the screen bounds
        Vector3 viewPos = mainCamera.WorldToViewportPoint(transform.position);
        if (viewPos.x < 0 || viewPos.x > 1 || viewPos.y < 0 || viewPos.y > 1)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Detect collision with target tag
        if (other.CompareTag(targetTag))
        {
            EnemyController enemy = other.GetComponent<EnemyController>();
            if (enemy != null)
            {
                enemy.TakeDamage(1);
            }

            PlayerController player = other.GetComponent<PlayerController>();
            if (player != null)
            {
                player.TakeDamage(1);
            }

            Destroy(gameObject);
        }
    }

    //-----------------------------------------------------------------------------//
    // Public Methods

    // Initialize projectile properties
    public void Initialize(string shooterTag, Vector2? customDirection = null)
    {
        if (customDirection.HasValue)
        {
            direction = customDirection.Value.normalized;
        }
        else if (shooterTag == "Player")
        {
            direction = Vector2.up;
        }
        else if (shooterTag == "Enemy" || shooterTag == "EnemyBoss")
        {
            direction = Vector2.down;
        }
        else
        {
            direction = Vector2.zero;
        }

        // Set appropriate target tag
        if (shooterTag == "Player")
            targetTag = "Enemy";
        else
            targetTag = "Player";

        RotateToDirection();
    }

    //-----------------------------------------------------------------------------//
    // Helper Methods

    private void RotateToDirection()
    {
        if (direction != Vector2.zero)
        {
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0f, 0f, angle - 90f);
        }
    }
}
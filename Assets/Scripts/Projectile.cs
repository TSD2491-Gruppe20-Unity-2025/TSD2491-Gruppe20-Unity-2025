using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float speed = 20f;
    public Vector2 direction = Vector2.zero;
    public string targetTag;

    private Camera mainCamera;

    private void Start()
    {
        mainCamera = Camera.main;

        // Apply initial rotation
        RotateToDirection();
    }

    // Call this to set direction and targetTag when projectile is created
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

        // Set appropriate targetTag
        if (shooterTag == "Player")
            targetTag = "Enemy";
        else
            targetTag = "Player";

        RotateToDirection();
    }

    void Update()
    {
        transform.Translate(direction * speed * Time.deltaTime, Space.World);

        // Despawn if outside screen
        Vector3 viewPos = mainCamera.WorldToViewportPoint(transform.position);
        if (viewPos.x < 0 || viewPos.x > 1 || viewPos.y < 0 || viewPos.y > 1)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
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

    private void RotateToDirection()
    {
        if (direction != Vector2.zero)
        {
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0f, 0f, angle - 90f);
        }
    }
}

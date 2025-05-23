using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float speed = 20f;
    public Vector2 direction = Vector2.zero;
    public string targetTag;

    public float damage = 1f;
    public PlayerController shooter; // ✅ Shooter reference for kill credit

    private Camera mainCamera;

    private void Start()
    {
        mainCamera = Camera.main;
        RotateToDirection();
    }

    private void Update()
    {
        transform.Translate(direction * speed * Time.deltaTime, Space.World);

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
        if (targetTag == "Enemy")
        {
            // Boss check first (inherits from EnemyController)
            EnemyBoss boss = other.GetComponent<EnemyBoss>();
            if (boss != null)
            {
                boss.TakeDamage(1, shooter);
            }
            else
            {
                EnemyController enemy = other.GetComponent<EnemyController>();
                if (enemy != null)
                {
                    enemy.TakeDamage(1, shooter);
                }
            }
        }
        else if (targetTag == "Player")
        {
            PlayerController player = other.GetComponent<PlayerController>();
            if (player != null)
            {
                player.TakeDamage(1);
            }
        }

        Destroy(gameObject);
    }
}



    public void Initialize(string shooterTag, PlayerController shooterRef, Vector2? customDirection = null)
    {
        shooter = shooterRef;

        if (customDirection.HasValue)
        {
            direction = customDirection.Value.normalized;
        }
        else
        {
            direction = shooterTag switch
            {
                "Player" => Vector2.up,
                "Enemy" or "EnemyBoss" => Vector2.down,
                _ => Vector2.up
            };
        }

        targetTag = shooterTag == "Player" ? "Enemy" : "Player";

        RotateToDirection();
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

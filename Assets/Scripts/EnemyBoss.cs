using UnityEngine;

public class EnemyBoss : EnemyController
{
    public float enterSpeed = 1.5f;
    public float moveSpeed = 2f;
    public float moveRange = 6f;
    public int startingHealth = 10;

    private BaseWeapon baseWeapon;
    private float fireInterval = 1.0f;
    private float nextFireTime;
    private Vector3 startPos;
    private bool hasEntered = false;
    private float direction = 1f;

    private void Awake()
    {
        if (health <= 0)
            health = startingHealth;
    }

    private void Start()
    {
        startPos = transform.position;
        baseWeapon = GetComponent<BaseWeapon>();
        nextFireTime = Time.time + fireInterval;
    }

    private void Update()
    {
        if (!hasEntered)
        {
            transform.Translate(Vector2.down * enterSpeed * Time.deltaTime);

            if (transform.position.y <= Camera.main.ViewportToWorldPoint(new Vector2(0, 0.8f)).y)
            {
                hasEntered = true;
                startPos = transform.position;
            }
        }
        else
        {
            transform.position += Vector3.right * direction * moveSpeed * Time.deltaTime;

            if (Mathf.Abs(transform.position.x - startPos.x) > moveRange)
                direction *= -1f;

            if (baseWeapon != null && Time.time >= nextFireTime)
            {
                GameObject player = GameObject.FindGameObjectWithTag("Player");
                if (player != null)
                {
                    Vector2 directionToPlayer = (player.transform.position - baseWeapon.FirePoint.position).normalized;
                    baseWeapon.Fire(directionToPlayer);
                }

                nextFireTime = Time.time + fireInterval;
            }
        }
    }

    protected override void Die(PlayerController killer)
    {
        SFXManager.Instance.Play(SFXEvent.PlayerDeathS);
        GameController.Instance.RegisterEnemyKill(killer);
        GameController.Instance.RegisterEnemyKill(killer); // Bonus kill point
        GameController.Instance.OnBossDefeated();
        Destroy(gameObject);
    }
}

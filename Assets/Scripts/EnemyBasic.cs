using UnityEngine;

public class EnemyBasic : EnemyController
{
    public float speed = 2f;
    public int startingHealth = 3;

    private BaseWeapon baseWeapon;
    private float fireInterval = 1f;
    private float nextFireTime;

    private void Awake()
    {
        if (health <= 0)
            health = startingHealth;
    }

    private void Start()
    {
        baseWeapon = GetComponent<BaseWeapon>();
        nextFireTime = Time.time + fireInterval;
    }

    private void Update()
    {
        transform.Translate(Vector2.down * speed * Time.deltaTime);

        if (baseWeapon != null && Time.time >= nextFireTime)
        {
            baseWeapon.Fire();
            SFXManager.Instance.Play(SFXEvent.EnemyFireS);
            nextFireTime = Time.time + fireInterval;
        }

        if (transform.position.y < Camera.main.ViewportToWorldPoint(Vector2.zero).y - 1f)
        {
            Destroy(gameObject);
        }
    }
}
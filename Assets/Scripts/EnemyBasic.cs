using UnityEngine;

public class EnemyBasic : EnemyController
{
    public float speed = 2f;

    private BaseWeapon baseWeapon;
    private float fireInterval = 1f;
    private float nextFireTime;

    void Start()
    {
        health = 3;

        // Get the BaseWeapon component attached to this enemy
        baseWeapon = GetComponent<BaseWeapon>();
        nextFireTime = Time.time + fireInterval;
    }

    void Update()
    {
        // Move downwards
        transform.Translate(Vector2.down * speed * Time.deltaTime);

        // Fire weapon every 1 second
        if (baseWeapon != null && Time.time >= nextFireTime)
        {
            baseWeapon.Fire();
            nextFireTime = Time.time + fireInterval;
        }

        // Destroy if below the bottom of the screen
        if (transform.position.y < Camera.main.ViewportToWorldPoint(Vector2.zero).y - 1f)
            Destroy(gameObject);
    }
}

using UnityEngine;

public class EnemyBasic : EnemyController
{
    //-----------------------------------------------------------------------------//
    // Movement and Firing Settings

    public float speed = 2f;

    private BaseWeapon baseWeapon;
    private float fireInterval = 1f;
    private float nextFireTime;

    //-----------------------------------------------------------------------------//
    // Unity Methods

    void Start()
    {
        health = 3;

        // Get the BaseWeapon component attached to this enemy
        baseWeapon = GetComponent<BaseWeapon>();
        nextFireTime = Time.time + fireInterval;
    }

    void Update()
    {
        // Move enemy downward
        transform.Translate(Vector2.down * speed * Time.deltaTime);

        // Fire weapon at regular intervals
        if (baseWeapon != null && Time.time >= nextFireTime)
        {
            baseWeapon.Fire();
            nextFireTime = Time.time + fireInterval;
        }

        // Destroy enemy if it goes off screen (below the view)
        if (transform.position.y < Camera.main.ViewportToWorldPoint(Vector2.zero).y - 1f)
        {
            Destroy(gameObject);
        }
    }
}
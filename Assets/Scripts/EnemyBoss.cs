using UnityEngine;

public class EnemyBoss : EnemyController
{
    public float enterSpeed = 1.5f;
    public float moveSpeed = 2f;
    public float moveRange = 3f;

    private Vector3 startPos;
    private bool hasEntered = false;
    private float direction = 1f;

    void Start()
    {
        startPos = transform.position;
        health = 25; // Boss has more health
    }

    void Update()
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
        }
    }

    protected override void Die()
    {
        // Add explosion or effects here if needed
        base.Die();
    }
}
